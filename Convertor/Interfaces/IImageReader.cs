using Converter.ImageBase;

namespace Converter.Interfaces
{
    public interface IImageReader
    {
        public Image Read(string path);
    }
}
