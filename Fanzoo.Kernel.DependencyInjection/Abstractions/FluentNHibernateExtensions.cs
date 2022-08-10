using Fanzoo.Kernel.Data;
using Fanzoo.Kernel.Data.Listeners;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Driver;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static class FluentNHibernateExtensions
    {
        public static FluentConfiguration AddSqlServerCore(this FluentConfiguration fluentConfiguration, string connectionString) =>
            fluentConfiguration
                .Database(MsSqlConfiguration
                    .MsSql2012
                        .Driver<MicrosoftDataSqlClientDriver>()
                        .ConnectionString(connectionString));

        public static FluentConfiguration AddSqlServerCore(this FluentConfiguration fluentConfiguration, IConfiguration configuration) =>
            fluentConfiguration
                .Database(MsSqlConfiguration
                    .MsSql2012
                        .Driver<MicrosoftDataSqlClientDriver>()
                        .ConnectionString(configuration.GetConnectionString()));

        public static FluentConfiguration AddSqlServerCore(this FluentConfiguration fluentConfiguration, ConfigurationManager configurationManager) =>
            fluentConfiguration
                .Database(MsSqlConfiguration
                    .MsSql2012
                        .Driver<MicrosoftDataSqlClientDriver>()
                        .ConnectionString(configurationManager.GetConnectionString()));

        public static FluentConfiguration AddMappingsCore(this FluentConfiguration fluentConfiguration) =>
            fluentConfiguration
                .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<IUnitOfWork>());

        public static FluentConfiguration AddListenersCore(this FluentConfiguration fluentConfiguration)
        {
            fluentConfiguration.ExposeConfiguration(cfg =>
            {
                cfg.AppendListeners(NHibernate.Event.ListenerType.PreInsert, new[] { new AuditListener() });
                cfg.AppendListeners(NHibernate.Event.ListenerType.PreUpdate, new[] { new AuditListener() });
            });

            return fluentConfiguration;
        }
    }
}
