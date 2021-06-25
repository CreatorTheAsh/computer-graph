using System.Numerics;
using Converter.ImageBase;

namespace Converter.Interfaces
{
    public interface IVectorConverter
    {
        public Color[,] ConvertFromVectorToColors(Vector3[,] vectors);
    }
}
