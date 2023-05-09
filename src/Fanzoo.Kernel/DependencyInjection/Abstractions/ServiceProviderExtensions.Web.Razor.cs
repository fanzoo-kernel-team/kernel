using Fanzoo.Kernel.Web.Mvc.Filters;
using Fanzoo.Kernel.Web.Validation.Cookies;
using Microsoft.AspNetCore.Authentication.Cookies;
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

    public class AddRazorPagesCoreConfiguration
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

    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddRazorPagesCore<TUserAuthenticationService, TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>(this IServiceCollection services, Action<AddRazorPagesCoreConfiguration>? configuration = null)
            where TUserAuthenticationService : class, IRazorPagesUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, ICookieUserAuthenticationService
            where TUserIdentifier : IdentifierValue<TUserIdentifierPrimitive>
            where TUserIdentifierPrimitive : notnull, new()
            where TUsername : IUsernameValue
            where TPassword : IPasswordValue
        {
            var configurationOptions = new AddRazorPagesCoreConfiguration();

            configuration?.Invoke(configurationOptions);

            services.AddMvcCore(o =>
            {
                o.CacheProfiles.AddRange(configurationOptions.CacheProfiles);

                o.Filters.AddRange(configurationOptions.Filters);

                foreach (var mapping in configurationOptions.FormatterMappings)
                {
                    o.FormatterMappings.SetMediaTypeMappingForFormat(mapping.Key, mapping.Value);
                }

                o.InputFormatters.AddRange(configurationOptions.InputFormatters);

                o.OutputFormatters.AddRange(configurationOptions.OutputFormatters);

                o.ModelBinderProviders.AddRange(configurationOptions.ModelBinderProviders);

                o.ModelMetadataDetailsProviders.AddRange(configurationOptions.ModelMetadataDetailsProviders);

                // clear the built-in providers & add a filter to clear model state after the built-in model binding validation
                if (configurationOptions.DisableBuiltInValidation)
                {
                    o.ModelValidatorProviders.Clear();

                    o.Filters.Add(new DisableBuiltInModelValidationFilter());
                }

                o.ModelValidatorProviders.AddRange(configurationOptions.ModelValidatorProviders);

                o.ValueProviderFactories.AddRange(configurationOptions.ValueProviderFactories);
            });

            services
                .AddCors()
                .AddRazorPages(o =>
                {
                    if (configurationOptions.RootDirectory is not null)
                    {
                        o.RootDirectory = configurationOptions.RootDirectory;
                    }

                    foreach (var convention in configurationOptions.Conventions)
                    {
                        o.Conventions.Add(convention);
                    }
                })
                .AddRazorRuntimeCompilation();

            services
                .AddTransient<IPasswordHashingService, IdentityPasswordHashingService>()
                .AddTransient<IRazorPagesUserAuthenticationService<TUserIdentifier, TUserIdentifierPrimitive, TUsername, TPassword>, TUserAuthenticationService>()
                .AddTransient<ICookieUserAuthenticationService, TUserAuthenticationService>()
                .AddScoped<PersistentCookieAuthenticationEvents>()
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.ExpireTimeSpan = TimeSpan.FromDays(90);
                        options.SlidingExpiration = true;
                        options.EventsType = typeof(PersistentCookieAuthenticationEvents);
                    });

            return services;
        }
    }
}
