using System.Numerics;

namespace Converter.Interfaces
{
    public interface ICameraDirectionProvider
    {
        public Vector3 GetCameraDirection(int pixelHeight, int pixelWidth, int screenHeight, int screenWidth, int fov);
    }
}
