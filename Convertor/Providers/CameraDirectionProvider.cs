using System;
using System.Numerics;
using Converter.Interfaces;

namespace Converter.Providers
{
    public class CameraDirectionProvider : ICameraDirectionProvider
    {
        public Vector3 GetCameraDirection(int pixelHeight, int pixelWidth, int screenHeight, int screenWidth, int fov)
        {
            float x = (float)(((2 * (pixelWidth + 0.5)) / screenWidth - 1f) *
                              Math.Tan(fov / 2f) *
                              screenWidth) /
                      screenHeight;

            float z = -((2f * (pixelHeight + 0.5f)) / screenHeight - 1f) *
                      (float)Math.Tan(fov / 2f);
            return new Vector3(x, -1, z);
        }
    }
}
