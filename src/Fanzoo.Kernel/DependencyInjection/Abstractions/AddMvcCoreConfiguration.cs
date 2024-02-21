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

        public IList<IPageConvention> Conventions { get; } = [];

        public string? RootDirectory { get; set; }

        public FilterCollection Filters { get; } = [];

        public int CookieExpirationDays { get; set; } = 90;

        internal Dictionary<string, MediaTypeHeaderValue> FormatterMappings { get; } = [];

        public FormatterCollection<IInputFormatter> InputFormatters { get; } = [];

        public FormatterCollection<IOutputFormatter> OutputFormatters { get; } = [];

        public IList<IModelBinderProvider> ModelBinderProviders { get; } = [];

        public IList<IMetadataDetailsProvider> ModelMetadataDetailsProviders { get; } = [];

        public IList<IModelValidatorProvider> ModelValidatorProviders { get; } = [];

        public IList<IValueProviderFactory> ValueProviderFactories { get; } = [];

        public void SetFormatterMapping(string format, MediaTypeHeaderValue contentType) => FormatterMappings[format] = contentType;

        public void SetFormatterMapping(string format, string contentType) => FormatterMappings[format] = MediaTypeHeaderValue.Parse(contentType);
    }
}
