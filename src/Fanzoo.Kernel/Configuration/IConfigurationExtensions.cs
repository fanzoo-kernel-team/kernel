using Microsoft.Extensions.Configuration.Json;

namespace Fanzoo.Kernel.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string? GetConnectionString(this IConfiguration configuration)
        {
            //first grab the explicitly configured connection string, if it exists
            var connectionString = configuration.GetConnectionString(configuration[ConfigurationKeys.ConnectionStringName.ToString()] ?? string.Empty);

            if (connectionString is null)
            {
                //try to grab the first connection string from the section
                var section = configuration.GetSection("ConnectionStrings");

                return section?.GetChildren().FirstOrDefault()?.Value;
            }

            return configuration.GetConnectionString(configuration[ConfigurationKeys.ConnectionStringName.ToString()] ?? string.Empty);
        }

        public static string? GetConnectionString(this ConfigurationManager configurationManager)
        {
            //first grab the explicitly configured connection string, if it exists
            var connectionString = configurationManager.GetConnectionString(configurationManager[ConfigurationKeys.ConnectionStringName.ToString()] ?? string.Empty);

            if (connectionString is null)
            {
                //try to grab the first connection string from the section
                var section = configurationManager.GetSection("ConnectionStrings");

                return section?.GetChildren().FirstOrDefault()?.Value;
            }

            return configurationManager.GetConnectionString(configurationManager[ConfigurationKeys.ConnectionStringName.ToString()] ?? string.Empty);
        }

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
        public static void RemoveAppSettingSection(this IConfiguration configuration, string sectionName)
        {
            if (configuration is not IConfigurationRoot configurationRoot)
            {
                throw new ArgumentException("Configuration is not IConfigurationRoot", nameof(configuration));
            }

            var configurationProvider = configurationRoot.Providers.SingleOrDefault(p => p is JsonConfigurationProvider) ?? throw new InvalidOperationException("No appsettings.json configuration provider found.");

            var propertyInfo = configurationProvider
                .GetType()
                    .GetProperty("Data", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?? throw new InvalidOperationException("Cannot load Data field from JsonConfigurationProvider.");


            if (propertyInfo.GetValue(configurationProvider) is not IDictionary<string, string> data)
            {
                throw new InvalidOperationException("Cannot load Data from JsonConfigurationProvider.");
            }

            if (data.ContainsKey(sectionName))
            {
                data.Remove(sectionName);
            }
        }
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
    }
}
