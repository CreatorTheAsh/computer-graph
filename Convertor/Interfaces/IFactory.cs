using Converter.Readers;

namespace Converter.Interfaces
{
    public interface IFactory<out TCreationType>
    {
        TCreationType Create(ImageType imageType);
    }
}
