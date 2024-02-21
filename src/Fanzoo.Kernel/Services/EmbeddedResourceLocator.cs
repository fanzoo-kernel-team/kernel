namespace Fanzoo.Kernel.Services
{
    public class EmbeddedResourceLocator(Assembly assembly) : ITemplateEmbeddedResourceLocator, IScriptEmbeddedResourceLocator
    {
        public Assembly Assembly { get; init; } = assembly;
    }
}
