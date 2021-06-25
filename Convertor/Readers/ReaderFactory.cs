using Converter.Interfaces;

namespace Converter.Readers
{
    public enum ImageType
    {
        Ppm,
        Obj,
        Gif,
        Bmp
    }

    public class ReaderFactory : IFactory<IImageReader>
    {
        private readonly IRenderer _renderer;
        private readonly IVectorConverter _vectorConverter;
        public ReaderFactory(IRenderer renderer, IVectorConverter vectorConverter)
        {
            _renderer = renderer;
            _vectorConverter = vectorConverter;
        }
        public IImageReader Create(ImageType imageType)
        {
            return imageType switch
            {
                ImageType.Ppm => new PpmReader(),
                ImageType.Obj => new ObjReader(_renderer, _vectorConverter),
                _ => null
            };
        }
    }
}
