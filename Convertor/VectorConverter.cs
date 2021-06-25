using System.Numerics;
using Converter.ImageBase;
using Converter.Interfaces;

namespace Converter
{
    public class VectorConverter : IVectorConverter
    {
        public Color[,] ConvertFromVectorToColors(Vector3[,] vectors)
        {
            Color[,] result = new Color[vectors.GetLength(0),vectors.GetLength(1)];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = new Color()
                    {
                        R = (int)vectors[i, j].X,
                        G = (int)vectors[i, j].Y,
                        B = (int)vectors[i, j].Z,
                    };
                }
            }

            return result;
        }
    }
}
