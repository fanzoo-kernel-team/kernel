using Fanzoo.Kernel.Web.Mvc.Filters;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddWebCore(this IServiceCollection services) =>
            services
                .AddHttpContextAccessor()
                .AddTransient<IContextAccessorService, HttpContextAccessorService>()
                .AddTransient<ICurrentUserService, CurrentUserService>();

        public static IServiceCollection AddMvcCore(this IServiceCollection services, AddMvcCoreConfiguration configuration)
        {
            services.AddMvcCore(o =>
            {
                o.CacheProfiles.AddRange(configuration.CacheProfiles);

                o.Filters.AddRange(configuration.Filters);

                foreach (var mapping in configuration.FormatterMappings)
                {
                    o.FormatterMappings.SetMediaTypeMappingForFormat(mapping.Key, mapping.Value);
                }

                o.InputFormatters.AddRange(configuration.InputFormatters);

                o.OutputFormatters.AddRange(configuration.OutputFormatters);

                foreach (var modelProvider in configuration.ModelBinderProviders)
                {
                    o.ModelBinderProviders.Insert(0, modelProvider);
                }

                foreach (var modelMetadataDetailProvider in configuration.ModelMetadataDetailsProviders)
                {
                    o.ModelMetadataDetailsProviders.Insert(0, modelMetadataDetailProvider);
                }

                // clear the built-in providers & add a filter to clear model state after the built-in model binding validation
                if (configuration.DisableBuiltInValidation)
                {
                    o.ModelValidatorProviders.Clear();

                    o.Filters.Add(new DisableBuiltInModelValidationFilter());
                }

                o.ModelValidatorProviders.AddRange(configuration.ModelValidatorProviders);

                o.ValueProviderFactories.AddRange(configuration.ValueProviderFactories);
            });

            return services;
        }

    }
}
