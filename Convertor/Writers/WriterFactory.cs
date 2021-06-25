using Converter.Interfaces;
using Converter.Readers;

namespace Converter.Writers
{
    public class WriterFactory : IFactory<IImageWriter>
    {
        public IImageWriter Create(ImageType imageType)
        {
            return imageType switch
            {
                ImageType.Gif => new GifWriter(),
                ImageType.Bmp => new BmpWriter(),
                _ => null
            };
        }
    }
}
