using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Net.Http.Headers;

namespace Fanzoo.Kernel.DependencyInjection
{
    // see: https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/Infrastructure/MvcCoreMvcOptionsSetup.cs
    // for the default values MVC configures

    public class AddMvcCoreConfiguration
    {
        public bool DisableBuiltInValidation { get; set; } = false;

        public IDictionary<string, CacheProfile> CacheProfiles { get; } = new Dictionary<string, CacheProfile>(StringComparer.OrdinalIgnoreCase);

        public IList<IPageConvention> Conventions { get; } = new List<IPageConvention>();

        public string? RootDirectory { get; set; }

        public FilterCollection Filters { get; } = new FilterCollection();

        public int CookieExpirationDays { get; set; } = 90;

        internal Dictionary<string, MediaTypeHeaderValue> FormatterMappings { get; } = new();

        public FormatterCollection<IInputFormatter> InputFormatters { get; } = new FormatterCollection<IInputFormatter>();

        public FormatterCollection<IOutputFormatter> OutputFormatters { get; } = new FormatterCollection<IOutputFormatter>();

        public IList<IModelBinderProvider> ModelBinderProviders { get; } = new List<IModelBinderProvider>();

        public IList<IMetadataDetailsProvider> ModelMetadataDetailsProviders { get; } = new List<IMetadataDetailsProvider>();

        public IList<IModelValidatorProvider> ModelValidatorProviders { get; } = new List<IModelValidatorProvider>();

        public IList<IValueProviderFactory> ValueProviderFactories { get; } = new List<IValueProviderFactory>();

        public void SetFormatterMapping(string format, MediaTypeHeaderValue contentType) => FormatterMappings[format] = contentType;

        public void SetFormatterMapping(string format, string contentType) => FormatterMappings[format] = MediaTypeHeaderValue.Parse(contentType);
    }
}
