using Converter.ImageBase;
using Converter.ImageConcrete;
using System.IO;

namespace Converter.Readers
{
    public abstract class ReaderBase : Interfaces.IImageReader
    {
        protected string FilePath { get; private set; }
        public Image Read(string path)
        {
            FilePath = path;
            BinaryReader imgFile = OpenBinary(path);
            Header header = ReadHeader(imgFile);
            var colors = ReadColors(header, imgFile);
            return new ImagePpm() { Path = path, Header = header, Bitmap=colors};
        }

        public virtual BinaryReader OpenBinary(string path)
        {
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
            return reader;
        }
        public abstract Header ReadHeader(BinaryReader imgFile);
        public abstract Color[,] ReadColors(Header header, BinaryReader imgFile);
    }
}
