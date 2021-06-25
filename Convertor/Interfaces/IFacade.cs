namespace Converter.Interfaces
{
    public interface IFacade
    {
        public void InitiateConversion(string originalPath, string destinationPath, string outputFormat);
    }
}