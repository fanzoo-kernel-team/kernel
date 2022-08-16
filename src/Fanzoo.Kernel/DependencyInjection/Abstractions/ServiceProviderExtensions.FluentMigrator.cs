using FluentMigrator.Runner;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddFluentMigratorCoreFromAssembly(this IServiceCollection services, string assemblyName, ConfigurationManager configurationManager)
        {
            var assembly = Assembly.Load(assemblyName);

            return services.AddFluentMigratorCoreFromAssembly(assembly, configurationManager);
        }

        public static IServiceCollection AddFluentMigratorCoreFromAssembly(this IServiceCollection services, Assembly assembly, ConfigurationManager configurationManager)
        {
            services.AddFluentMigratorCore()
                .ConfigureRunner(r =>
                    r.AddSqlServer2016()
                        .WithGlobalConnectionString(configurationManager.GetConnectionString())
                        .ScanIn(assembly)
                            .For
                                .Migrations());

            return services;
        }
    }
}
