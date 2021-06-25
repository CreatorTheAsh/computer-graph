using System;
using System.Collections.Generic;

namespace Converter.BytePackers
{
    public static class LzwBytePacker
    {
        private static int _currentCodeWidth;
        private static int _codeIndex;
        private static int _codeWidthIncrease;
        private static int _clearCode;

        public static List<byte> PackBytes(List<int> codeStream, byte minCodeSize)
        {
            List<bool> codeBits = new List<bool>();
            List<byte> result = new List<byte>();

            Initialize(minCodeSize);

            for (int i = 0; i < codeStream.Count; i++, _codeIndex++)
            {
                int code = codeStream[i];
                if (code == _clearCode && i != 0)
                {
                    List<bool> clearCodeBits = ConvertInt32ToBits(code);
                    codeBits.AddRange(clearCodeBits);
                    Initialize(minCodeSize);
                    continue;
                }
                if (_codeIndex == _codeWidthIncrease)
                {
                    _currentCodeWidth++;
                    _codeWidthIncrease = (int)Math.Pow(2, _currentCodeWidth);
                }

                List<bool> bits = ConvertInt32ToBits(code);
                codeBits.AddRange(bits);
            }

            int zeroesNeeded = 8 - (codeBits.Count % 8);

            for (int i = 0; i < zeroesNeeded; i++)
            {
                codeBits.Add(false);
            }

            for (int i = 0; i < codeBits.Count; i += 8)
            {
                List<bool> reversedByte = codeBits.GetRange(i, 8);
                reversedByte.Reverse();
                result.Add(ConvertBitsToByte(reversedByte));
            }

            return result;
        }

        private static List<bool> ConvertInt32ToBits(int num)
        {
            List<bool> bits = new List<bool>();

            int temp = num;

            while (temp > 0)
            {
                bits.Add((temp & 1) == 1);

                temp >>= 1;
            }

            while (bits.Count < _currentCodeWidth)
            {
                bits.Add(false);
            }

            return bits;
        }

        private static byte ConvertBitsToByte(List<bool> bits)
        {
            byte result = 0;
            int length = bits.Count;
            int index = 8 - length;

            for (int i = 0; i < length; i++, index++)
            {
                if (bits[i])
                {
                    result |= (byte) (1 << (7 - index));
                }
            }

            return result;
        }

        private static void Initialize(byte minCodeSize)
        {
            _currentCodeWidth = minCodeSize + 1;
            _codeIndex = (int)Math.Pow(2, minCodeSize);
            _codeWidthIncrease = (int)Math.Pow(2, _currentCodeWidth);
            _clearCode = _codeIndex;
        }
    }
}
