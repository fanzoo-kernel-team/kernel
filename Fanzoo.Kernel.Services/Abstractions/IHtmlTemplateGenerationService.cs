namespace Fanzoo.Kernel.Services
{
    public interface IHtmlTemplateGenerationService
    {
        ValueTask<string> GenerateAsync(string templatePathname, Dictionary<string, object> values, string? layoutPathName = "_Layout.html");
    }
}
