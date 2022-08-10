using Fanzoo.Kernel.Data;
using FluentNHibernate.Cfg;
using NHibernate;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddNHibernateCoreFromAssemblyOf<TMappingClass>(this IServiceCollection services, IConfiguration configuration)
        {
            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configuration)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<TMappingClass>());

            return AddNHibernateCore(services, fluentConfiguration);
        }

        public static IServiceCollection AddNHibernateCoreFromAssemblyOf<TMappingClass>(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configurationManager)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<TMappingClass>());

            return AddNHibernateCore(services, fluentConfiguration);
        }

        public static IServiceCollection AddNHibernateCoreFromAssembly(this IServiceCollection services, Assembly assembly, ConfigurationManager configurationManager)
        {
            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configurationManager)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssembly(assembly));

            return AddNHibernateCore(services, fluentConfiguration);
        }

        public static IServiceCollection AddNHibernateCoreFromAssembly(this IServiceCollection services, string assemblyName, ConfigurationManager configurationManager)
        {
            var assembly = Assembly.Load(assemblyName);

            var fluentConfiguration = Fluently
                .Configure()
                    .AddSqlServerCore(configurationManager)
                    .AddMappingsCore()
                    .AddListenersCore()
                    .Mappings(mapping => mapping.FluentMappings.AddFromAssembly(assembly));

            return AddNHibernateCore(services, fluentConfiguration);
        }

        public static IServiceCollection AddNHibernateCore(this IServiceCollection services, FluentConfiguration fluentConfiguration) =>
            services.AddNHibernateCore(fluentConfiguration.BuildSessionFactory());

        public static IServiceCollection AddNHibernateCore(this IServiceCollection services, ISessionFactory sessionFactory)
        {
            services.AddSingleton(sessionFactory);

            //Sessions are owned by either a UnitOfWork or a StandAloneRepository

            services.AddTransient(serviceProvider => sessionFactory
                .WithOptions()
                    .Interceptor(new KernelInterceptor(serviceProvider)) //used for dependency injection in Listeners
                        .OpenSession());


            //unit of work scoped to the request (should only be used in CommandHandlers and DomainEvents)
            services.AddScoped<IScopedUnitOfWork, NHibernateUnitOfWork>();

            //unit of work limited to context
            services.AddTransient<IUnitOfWork, NHibernateUnitOfWork>();
            services.AddTransient<IReadOnlyUnitOfWork, NHibernateReadOnlyUnitOfWork>();

            //when repositories are injected outside of a unit of work they are scoped to the context
            services.AddTransient(typeof(IStandAloneReadOnlyRepository<>), typeof(NHibernateStandAloneReadOnlyRepository<>));
            services.AddTransient(typeof(IStandAloneRepository<>), typeof(NHibernateStandAloneRepository<>));

            return services;
        }
    }
}
