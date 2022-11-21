using System.Diagnostics.CodeAnalysis;

namespace Fanzoo.Kernel.Builder
{
    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddSetting<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this WebApplicationBuilder builder, string section)
            where TOptions : class
        {
            builder.Services.Configure<TOptions>(builder.Configuration.GetSection(section));

            return builder;
        }

        public static WebApplicationBuilder AddSingleton<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this WebApplicationBuilder builder)
            where TService : class
            where TImplementation : class, TService
        {
            builder.Services.AddSingleton<TService, TImplementation>();

            return builder;
        }

        public static WebApplicationBuilder AddTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this WebApplicationBuilder builder)
            where TService : class
            where TImplementation : class, TService
        {
            builder.Services.AddTransient<TService, TImplementation>();

            return builder;
        }

        public static WebApplicationBuilder AddScoped<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this WebApplicationBuilder builder)
            where TService : class
            where TImplementation : class, TService
        {
            builder.Services.AddScoped<TService, TImplementation>();

            return builder;
        }

        public static WebApplicationBuilder AddNHibernateCoreFromAssemblyOf<TMappingClass>(this WebApplicationBuilder builder)
        {
            builder.Services.AddNHibernateCoreFromAssemblyOf<TMappingClass>(builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder AddNHibernateCoreFromAssembly(this WebApplicationBuilder builder, Assembly assembly)
        {
            builder.Services.AddNHibernateCoreFromAssembly(assembly, builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder AddNHibernateCoreFromAssembly(this WebApplicationBuilder builder, string assemblyName)
        {
            builder.Services.AddNHibernateCoreFromAssembly(assemblyName, builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder AddFrameworkCoreFromAssemblies(this WebApplicationBuilder builder, Action<IServiceTypeAssemblyBuilder>? addTypes = null)
        {
            builder.Services.AddFrameworkCore(addTypes);

            return builder;
        }

        public static WebApplicationBuilder AddFluentMigratorCoreFromAssembly(this WebApplicationBuilder builder, Assembly assembly)
        {
            builder.Services.AddFluentMigratorCoreFromAssembly(assembly, builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder AddFluentMigratorCoreFromAssembly(this WebApplicationBuilder builder, string assemblyName)
        {
            builder.Services.AddFluentMigratorCoreFromAssembly(assemblyName, builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder AddScriptEmbeddedResourceLocatorFromAssembly(this WebApplicationBuilder builder, Assembly assembly)
        {
            builder.Services.AddScriptEmbeddedResourceLocator(assembly);

            return builder;
        }

        public static WebApplicationBuilder AddScriptEmbeddedResourceLocatorFromAssembly(this WebApplicationBuilder builder, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);

            return AddScriptEmbeddedResourceLocatorFromAssembly(builder, assembly);
        }

        public static WebApplicationBuilder AddHtmlTemplateGenerationFromAssembly(this WebApplicationBuilder builder, Assembly assembly)
        {
            builder.Services.AddHtmlTemplateGeneration(assembly);

            return builder;
        }

        public static WebApplicationBuilder AddHtmlTemplateGenerationFromAssembly(this WebApplicationBuilder builder, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);

            builder.Services.AddHtmlTemplateGeneration(assembly);

            return builder;
        }

        public static WebApplicationBuilder AddAutoMapperFromAssemblies(this WebApplicationBuilder builder, Assembly[] assemblies)
        {
            builder.Services.AddAutoMapperCore(assemblies);

            return builder;
        }

        public static WebApplicationBuilder AddAutoMapperFromAssemblies(this WebApplicationBuilder builder, Action<IServiceTypeAssemblyBuilder> addTypes)
        {
            builder.Services.AddAutoMapperCore(addTypes);

            return builder;
        }

        public static WebApplicationBuilder AddAutoMapperFromAssembly(this WebApplicationBuilder builder, Assembly assembly)
        {
            builder.Services.AddAutoMapperCore(assembly);

            return builder;
        }

        public static WebApplicationBuilder AddAutoMapperFromAssembly(this WebApplicationBuilder builder, string assemblyName)
        {
            builder.Services.AddAutoMapperCore(assemblyName);

            return builder;
        }

        public static WebApplicationBuilder AddApplicationModulesFromAssembly(this WebApplicationBuilder builder, Assembly assembly) => builder.AddApplicationModulesFromAssemblies(new[] { assembly });

        public static WebApplicationBuilder AddApplicationModulesFromAssembly(this WebApplicationBuilder builder, string assemblyName) => builder.AddApplicationModulesFromAssemblies(new[] { Assembly.Load(assemblyName) });

        public static WebApplicationBuilder AddApplicationModulesFromAssemblies(this WebApplicationBuilder builder, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var modules = assembly
                    .GetTypes()
                        .Where(t => t.IsClass)
                        .Where(t => t.IsAssignableTo(typeof(IApplicationModule)))
                            .Select(Activator.CreateInstance)
                                .Cast<IApplicationModule>();

                foreach (var module in modules)
                {
                    //add the module to the service registry
                    builder.Services.AddTransient(typeof(IApplicationModule), module.GetType());

                    //add it's services to the registry
                    module.RegisterServices(builder.Services);

                }
            }

            return builder;
        }

        public static WebApplicationBuilder AddApplicationModulesFromAssemblies(this WebApplicationBuilder builder, Action<IServiceTypeAssemblyBuilder> addTypes)
        {
            var serviceTypeBuilder = new ServiceTypeAssemblyBuilder();

            addTypes.Invoke(serviceTypeBuilder);

            return builder.AddApplicationModulesFromAssemblies(serviceTypeBuilder.Assemblies.ToArray());
        }
    }
}
