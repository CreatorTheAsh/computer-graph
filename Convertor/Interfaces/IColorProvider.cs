using System.Numerics;

namespace Converter.Interfaces
{
    public interface IColorProvider
    {
        public Vector3 GetBackgroundColor();
        public Vector3 GetObjectColor();
        public float GetBias();
    }
}
