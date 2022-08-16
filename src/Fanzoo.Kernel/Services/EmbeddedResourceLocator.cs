using System.Reflection;

namespace Fanzoo.Kernel.Services
{
    public class EmbeddedResourceLocator : ITemplateEmbeddedResourceLocator, IScriptEmbeddedResourceLocator
    {
        public EmbeddedResourceLocator(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; init; }
    }
}
