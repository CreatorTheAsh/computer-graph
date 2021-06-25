using Converter.ImageBase;
using Converter.ImageConcrete;
using System;
using System.IO;

namespace Converter.Readers
{
    public class PpmReader : ReaderBase
    {
        delegate int ReadSymbol();
        public override Color[,] ReadColors(Header header, BinaryReader imgFile)
        {
            return ((HeaderPpm) header).FormatType switch
            {
                "P6" => ReadP6(header, imgFile),
                _ => null
            };
        }

        public override Header ReadHeader(BinaryReader imgFile)
        {
            HeaderPpm header = new HeaderPpm();
            string type = String.Concat(imgFile.ReadChars(2));
            header.FormatType = type;

            imgFile.ReadChar();
            char currentSymbol = imgFile.ReadChar();
            // processing comments
            while (currentSymbol == '#')
            {
                while(imgFile.ReadChar()!= '\n') { }
                currentSymbol = imgFile.ReadChar();
            }

            header.Width = ReadNextNumber();
            header.Height = ReadNextNumber();
            header.MaxNumPerColor = ReadNextNumber();
            
            if(header.MaxNumPerColor <= byte.MaxValue)
            {
                header.BitsPerComponent = 8;
            }
            else if(header.MaxNumPerColor <= short.MaxValue)
            {
                header.BitsPerComponent = 16;
            }

            return header;

            int ReadNextNumber()
            {
                string number = "";
                do
                {
                    number += currentSymbol;
                    currentSymbol = imgFile.ReadChar();
                }
                while (currentSymbol != '\n' && currentSymbol != ' ');
                return int.Parse(number);
            }
        }

        private Color[,] ReadP6(Header header, BinaryReader imgFile)
        {
            Color[,] colors = new Color[header.Width, header.Height];

            try
            {    
                for (int i = 0; i < header.Width; i++)
                {
                    for (int j = 0; j < header.Height; j++)
                    {
                        colors[i, j] = new Color()
                        {
                            R = imgFile.ReadByte(),
                            G = imgFile.ReadByte(),
                            B = imgFile.ReadByte()
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Bad File XD");
            }
            finally
            {
                imgFile.Close();
            }

            NormalizeTo255(colors, (HeaderPpm)header);

            imgFile.Close();
            return colors;
        }

        private void NormalizeTo255(Color[,] colors, HeaderPpm header)
        {
            double coefficient = 255.0 / header.MaxNumPerColor;
            for (int i = 0; i < header.Width; i++)
            {
                for (int j = 0; j < header.Height; j++)
                {
                    colors[i, j].R = (int)(colors[i, j].R * coefficient);
                    colors[i, j].G = (int)(colors[i, j].G * coefficient);
                    colors[i, j].B = (int)(colors[i, j].B * coefficient);
                }
            }
        }
    }
}
