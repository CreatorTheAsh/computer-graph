namespace Converter.ImageBase
{
    public class Color
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        // not all the formats support this component
        // reasonable to make it nullable
        public int? A { get; set; }
    }
}
