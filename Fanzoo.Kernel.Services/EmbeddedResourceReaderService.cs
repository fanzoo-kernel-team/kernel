using System.Reflection;

namespace Fanzoo.Kernel.Services
{
    public class EmbeddedResourceReaderService : IEmbeddedResourceReaderService
    {
        public async ValueTask<string> ReadEmbeddedResourceFileAsync(string resourcePath, Assembly targetAssembly)
        {
            var resourceName = $"{targetAssembly.GetName().Name}.{resourcePath.Replace("\\", ".")}" ?? throw new Exception("Target assembly not found.");

            using var reader = new StreamReader(targetAssembly.GetManifestResourceStream(resourceName) ?? throw new Exception("Cannot open resource stream."));

            return await reader.ReadToEndAsync();
        }

        public async ValueTask<string> ReadEmbeddedResourceFileAsync(string resourcePath, IEmbeddedResourceLocator locator) =>
            await ReadEmbeddedResourceFileAsync(resourcePath, locator.Assembly);
    }
}
