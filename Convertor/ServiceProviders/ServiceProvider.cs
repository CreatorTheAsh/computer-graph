using Converter.Interfaces;
using Converter.OcTree;
using Converter.Providers;
using Converter.Readers;
using Converter.Renderers;
using Converter.Writers;
using Ninject.Modules;

namespace Converter.ServiceProviders
{
    public class ServiceProvider : NinjectModule
    {
        public override void Load()
        {
            Bind<IFacade>().To<ConversionFacade>();
            
            Bind<IFactory<IImageWriter>>().To<WriterFactory>();
            Bind<IFactory<IImageReader>>().To<ReaderFactory>();

            Bind<IRenderer>().To<Renderer>();
            
            Bind<ICameraDirectionProvider>().To<CameraDirectionProvider>();
            Bind<ICameraPositionProvider>().To<CameraPositionProvider>();
            Bind<IColorProvider>().To<ColorProvider>();
            Bind<ILightsProvider>().To<LightsProvider>();
            Bind<IScreenProvider>().To<ScreenProvider>();
            
            Bind<ITree>().To<Tree>();
            Bind<ITreeProvider>().To<TreeProvider>();
            
            Bind<IVectorConverter>().To<VectorConverter>();
        }
    }
}