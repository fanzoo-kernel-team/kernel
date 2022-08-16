using System.Reflection;

namespace Fanzoo.Kernel.Services
{
    public interface IEmbeddedResourceReaderService : IService
    {
        ValueTask<string> ReadEmbeddedResourceFileAsync(string resourcePath, Assembly targetAssembly);

        ValueTask<string> ReadEmbeddedResourceFileAsync(string resourcePath, IEmbeddedResourceLocator locator);

    }
}
