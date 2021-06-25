using Converter.ImageBase;

namespace Converter.ImageConcrete
{
    public class HeaderPpm : Header
    {
        public string FormatType { get; set; }
        public override int Height { get; set; }
        public override int Width { get; set; }
        public override int BitsPerComponent { get; set; }
        public int MaxNumPerColor { get; set; }
    }
}
