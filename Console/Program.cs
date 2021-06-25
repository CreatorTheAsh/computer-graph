using System;
using Converter.Interfaces;
using Converter.ServiceProviders;
using Ninject;
namespace ConsoleInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // args = new string[]{
                //     "--source=cow.obj",
                //     "--goal-format=bmp",
                //     "--output=newcow"
                // };
                var value = KeyHandler.GetValues(args);
                IKernel kernel = new StandardKernel(new ServiceProvider());

                var facade = kernel.Get<IFacade>();
                facade.InitiateConversion(value.source, value.destination, value.format);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
