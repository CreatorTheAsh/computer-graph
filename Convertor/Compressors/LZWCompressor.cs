using System;
using System.Collections.Generic;
using System.Linq;
using Converter.ImageBase;

namespace Converter.Compressors
{
    public static class LzwCompressor
    {
        private static Dictionary<int, List<int>> _dictionary;
        private const int MaxDictionarySize = 4096;

        public static List<int> Compress(Color[,] colors, Color[] table, int tableSize)
        {
            List<int> codeStream = new List<int>();

            List<int> indexStream = colors.Cast<Color>()
                .Select(x => Array.FindIndex(table, y => 
                    y.B == x.B && 
                    y.R == x.R &&
                    y.G == x.G)).ToList();

            Initialize(tableSize);
           
            codeStream.Add(tableSize);

            List<int> buffer = new List<int>() {indexStream[0]};

            for (int i = 1; i < indexStream.Count; i++)
            {
                List<int> code = new List<int>() { indexStream[i] };

                List<int> bufferWithCode = buffer.Concat(code).ToList();

                if (_dictionary.Values.Any(x => x.SequenceEqual(bufferWithCode)))
                {
                    buffer = bufferWithCode;
                }
                else
                {
                    _dictionary.Add(_dictionary.Count, bufferWithCode);
                    codeStream.Add(GetCode(buffer));

                    if (_dictionary.Count == MaxDictionarySize)
                    {
                        Initialize(tableSize);
                        codeStream.Add(tableSize);
                        buffer = new List<int>() {indexStream[i]};
                        continue;
                    }
                    buffer = code;
                }
            }
            
            codeStream.Add(GetCode(buffer));
            codeStream.Add(_dictionary[tableSize + 1].First());

            return codeStream;
        }

        private static int GetCode(List<int> sequence)
        {
            return _dictionary.Keys.FirstOrDefault(key => _dictionary[key].SequenceEqual(sequence));
        }

        private static void Initialize(int tableSize)
        {
            _dictionary = new Dictionary<int, List<int>>();
            for (int i = 0; i < tableSize; i++)
            {
                _dictionary.Add(i, new List<int>() { i });
            }

            _dictionary.Add(tableSize, new List<int>() { tableSize });
            _dictionary.Add(tableSize + 1, new List<int>() { tableSize + 1 });
        }
    }
}
