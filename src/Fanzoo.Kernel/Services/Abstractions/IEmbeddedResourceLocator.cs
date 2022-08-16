using System.Reflection;

namespace Fanzoo.Kernel.Services
{
    public interface IEmbeddedResourceLocator
    {
        public Assembly Assembly { get; }
    }
}
