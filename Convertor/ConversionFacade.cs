using Converter.Interfaces;
using Converter.Readers;
using System;

namespace Converter
{
    public class ConversionFacade : IFacade
    {
        private readonly IFactory<IImageWriter> _writerFactory;
        private readonly IFactory<IImageReader> _readerFactory;
        public ConversionFacade(IFactory<IImageWriter> writerFactory, IFactory<IImageReader> readerFactory)
        {
            _writerFactory = writerFactory;
            _readerFactory = readerFactory;
        }
        
        public void InitiateConversion(string originalPath, string destinationPath, string outputFormat)
        {
            string originalTypeStr = GetExtension(originalPath);
            ImageType originalType;
            ImageType finalType;

            try
            {
                originalType = (ImageType)Enum.Parse(typeof(ImageType), originalTypeStr, true);
                finalType = (ImageType)Enum.Parse(typeof(ImageType), outputFormat, true);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Unsupported Image Type");
            }            
            
            IImageReader reader = _readerFactory.Create(originalType);
            IImageWriter writer = _writerFactory.Create(finalType);
                
            var image = reader.Read(originalPath);
            Converter converter = new Converter(writer);
            converter.Convert(image, destinationPath);
            
            string GetExtension(string path)
            {
                return path.Substring(path.LastIndexOf('.') + 1);
            }
        }

    }
}
