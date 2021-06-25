namespace Converter.ImageBase
{
    public abstract class Image
    {
        public abstract Header Header { get; set; }
        public abstract string Path { get; set; }
        public abstract Color[,] Bitmap { get; set; }
    }
}
