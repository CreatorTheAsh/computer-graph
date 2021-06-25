using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Converter.BytePackers;
using Converter.Compressors;
using Converter.ImageBase;
using Converter.Interfaces;

namespace Converter.Writers
{
    public class GifWriter : IImageWriter
    {
        private const byte FullBlock = 255;
        private readonly char[] _header = {'G', 'I', 'F', '8', '9', 'a'};
        private const string GlobalTableBit = "1";
        private const string SortBit = "0";
        private const byte BgColorIndex = 0;
        private const byte PixelAspectRatio = 0;
        private const int ColorBytes = 3;
        private const byte EmptyColorByte = 0;
        private const byte ImageSeparator = 44;
        private const short ImageLeft = 0;
        private const short ImageRight = 0;
        private const byte ImagePackedField = 0;
        private const byte BlockTerminator = 0;
        private const byte Trailer = 59;

        public void Write(string path, Image image)
        {
            Color[,] colors = GetColors(image.Bitmap);
            Color[] table = GetTable(colors);

            int tableSize = AlignToPowerOfTwo(table.Length);
            byte power = (byte) ((byte) Math.Log2(tableSize) - 1);
            string stringPower =  Convert.ToString(power, 2).PadLeft(3, '0');

            List<int> codes = LzwCompressor.Compress(colors, table, tableSize);
            byte minCodeSize = (byte) (power + 1 < 2 ? 2 : power + 1);

            List<byte> bytes = LzwBytePacker.PackBytes(codes, minCodeSize);
            byte fullBlock = 255;

            int fullBlockQuantity = bytes.Count / fullBlock;
            byte partialBlockCount = (byte)(bytes.Count % fullBlock);

            short height = (short)image.Header.Height;
            short width = (short) image.Header.Width;

            BinaryWriter writer;
            try
            {
                writer = new BinaryWriter(File.Open(path + ".gif", FileMode.Create));
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            
            WriteHeader();
            WriteLogicalScreen();
            WriteColorTable();
            WriteImageDescriptor(); 
            WriteImageData();
            WriteTrailer();

            writer.Close();

            void WriteHeader()
            {
                foreach (var ch in _header)
                {
                    writer.Write(ch);
                }
            }

            void WriteLogicalScreen()
            {
                writer.Write(width);
                writer.Write(height);

                byte packedField = Convert.ToByte(GlobalTableBit + stringPower + SortBit + stringPower, 2);
                writer.Write(packedField);

                writer.Write(BgColorIndex);

                writer.Write(PixelAspectRatio);
            }

            void WriteColorTable()
            {
                for (int i = 0; i < tableSize; i++)
                {
                    if (i < table.Length)
                    {
                        writer.Write((byte)table[i].R);
                        writer.Write((byte)table[i].G);
                        writer.Write((byte)table[i].B);
                    }
                    else
                    {
                        for (int j = 0; j < ColorBytes; j++)
                        {
                            writer.Write(EmptyColorByte);
                        }
                    }
                }
            }

            void WriteImageDescriptor()
            {
                writer.Write(ImageSeparator);

                writer.Write(ImageLeft);
                writer.Write(ImageRight);

                writer.Write(width);
                writer.Write(height);

                writer.Write(ImagePackedField);
            }

            void WriteImageData()
            {
                writer.Write(minCodeSize);

                for (int i = 0; i < fullBlockQuantity; i++)
                {
                    writer.Write(fullBlock);
                    for (int j = 0; j < fullBlock; j++)
                    {
                        writer.Write(bytes[i * fullBlock + j]);
                    }
                }

                if (partialBlockCount != 0)
                {
                    writer.Write(partialBlockCount);
                    for (int i = 0; i < partialBlockCount; i++)
                    {
                        writer.Write(bytes[fullBlockQuantity * fullBlock + i]);
                    }
                }


                writer.Write(BlockTerminator);
            }

            void WriteTrailer()
            {
                writer.Write(Trailer);
            }
        }

        private static Color[,] GetColors(Color[,] colors)
        {
            int rows = colors.GetLength(0);
            int columns = colors.GetLength(1);

            Color[,] result = new Color[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i, j] = new Color()
                    {
                        R = (colors[i, j].R * 8 / 256) * 36 ,
                        G = (colors[i, j].G * 8 / 256) * 36,
                        B = (colors[i, j].B * 4 / 256) * 72
                    };

                }
            }

            return result;
        }

        private static Color[] GetTable(Color[,] colors)
        {
            Color[] result = colors.Cast<Color>().ToArray();
            return result.GroupBy(x => new {x.R, x.G, x.B})
                .Select(x => new Color()
                {
                    R = x.Key.R,
                    G = x.Key.G,
                    B = x.Key.B
                }).ToArray();
        }

        private static int AlignToPowerOfTwo(int length)
        {
            int minValue = 2;

            while (minValue < length)
            {
                minValue *= 2;
            }

            return minValue;
        }
    }
}
