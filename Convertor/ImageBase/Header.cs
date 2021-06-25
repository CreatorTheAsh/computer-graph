namespace Converter.ImageBase
{
    public abstract class Header
    {
        public abstract int Height { get; set; }
        public abstract int Width { get; set; }
        public abstract int BitsPerComponent { get; set; }
    }
}
