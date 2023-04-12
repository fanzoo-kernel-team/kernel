namespace Fanzoo.Kernel.Services
{
    public sealed class HtmlTemplateGenerationService : IHtmlTemplateGenerationService
    {
        private readonly ITemplateEmbeddedResourceLocator _templateEmbeddedResourceLocator;
        private readonly IEmbeddedResourceReaderService _embeddedResourceReader;

        public HtmlTemplateGenerationService(ITemplateEmbeddedResourceLocator templateEmbeddedResourceLocator, IEmbeddedResourceReaderService embeddedResourceReader)
        {
            _templateEmbeddedResourceLocator = templateEmbeddedResourceLocator;
            _embeddedResourceReader = embeddedResourceReader;
        }

        public async ValueTask<string> GenerateAsync(string templatePathname, Dictionary<string, object> values, string? layoutPathName = "_Layout.html")
        {
            var template = await _embeddedResourceReader.ReadEmbeddedResourceFileAsync(templatePathname, _templateEmbeddedResourceLocator.Assembly);

            var sb = new TemplateStringBuilder(template, values);

            if (layoutPathName is not null)
            {
                var layout = await _embeddedResourceReader.ReadEmbeddedResourceFileAsync(layoutPathName, _templateEmbeddedResourceLocator.Assembly);

                values.Remove("Content");

                values.Add("Content", sb.ToString());

                sb = new TemplateStringBuilder(layout, values);
            }

            return sb.ToString();
        }
    }
}
