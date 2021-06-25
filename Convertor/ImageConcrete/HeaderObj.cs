using Converter.ImageBase;

namespace Converter.ImageConcrete
{
    public class HeaderObj : Header
    {
        public override int Height { get; set; }
        public override int Width { get; set; }
        public override int BitsPerComponent { get; set; }
    }
}
