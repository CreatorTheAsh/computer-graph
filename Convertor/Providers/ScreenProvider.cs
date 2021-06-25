using Converter.Interfaces;

namespace Converter.Providers
{
    public class ScreenProvider : IScreenProvider
    {
        public int GetWidth() => 1920;

        public int GetHeight() => 1080;

        public int GetFov() => 150;
    }
}
