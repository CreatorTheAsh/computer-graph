using Converter.ImageBase;

namespace Converter.ImageConcrete
{
    public class ImagePpm : Image
    {
        public override Header Header { get; set; }
        public override string Path { get; set; }
        public override Color[,] Bitmap { get; set; }
    }
}
