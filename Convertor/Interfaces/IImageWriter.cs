using Converter.ImageBase;

namespace Converter.Interfaces
{
    public interface IImageWriter
    {
        public void Write(string path, Image image);
    }
}
