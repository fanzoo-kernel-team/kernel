using Fanzoo.Kernel.Data;
using FluentNHibernate.Cfg;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddNHibernateCoreFromAssemblyOf<TMappingClass>(this IServiceCollection services, IConfiguration configuration, Action<NHibernate.Cfg.Configuration>? cfg = null)
        {
            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configuration)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<TMappingClass>());

            return AddNHibernateCore(services, fluentConfiguration, cfg);
        }

        public static IServiceCollection AddNHibernateCoreFromAssemblyOf<TMappingClass>(this IServiceCollection services, ConfigurationManager configurationManager, Action<NHibernate.Cfg.Configuration>? cfg = null)
        {
            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configurationManager)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<TMappingClass>());

            return AddNHibernateCore(services, fluentConfiguration, cfg);
        }

        public static IServiceCollection AddNHibernateCoreFromAssembly(this IServiceCollection services, Assembly assembly, ConfigurationManager configurationManager, Action<NHibernate.Cfg.Configuration>? cfg = null)
        {
            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configurationManager)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssembly(assembly));

            return AddNHibernateCore(services, fluentConfiguration, cfg);
        }

        public static IServiceCollection AddNHibernateCoreFromAssembly(this IServiceCollection services, string assemblyName, ConfigurationManager configurationManager, Action<NHibernate.Cfg.Configuration>? cfg = null)
        {
            var assembly = Assembly.Load(assemblyName);

            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configurationManager)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssembly(assembly));

            return AddNHibernateCore(services, fluentConfiguration, cfg);
        }

        public static IServiceCollection AddNHibernateCore(this IServiceCollection services, FluentConfiguration fluentConfiguration, Action<NHibernate.Cfg.Configuration>? cfg = null)
        {
            if (cfg is not null)
            {
                fluentConfiguration.ExposeConfiguration(cfg);
            }

            return services.AddNHibernateCore(fluentConfiguration.BuildSessionFactory());
        }

        public static IServiceCollection AddNHibernateCore(this IServiceCollection services, ISessionFactory sessionFactory) =>
            services
                .AddSingleton(sessionFactory)
                .AddScoped<IUnitOfWorkFactory, NHibernateUnitOfWorkFactory>();
    }
}
