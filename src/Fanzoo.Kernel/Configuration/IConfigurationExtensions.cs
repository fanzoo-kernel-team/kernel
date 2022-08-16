using Microsoft.Extensions.Configuration;

namespace Fanzoo.Kernel.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string? GetConnectionString(this IConfiguration configuration)
        {
            //first grab the explicitly configured connection string, if it exists
            var connectionString = configuration.GetConnectionString(configuration[ConfigurationKeys.ConnectionStringName.ToString()]);

            if (connectionString is null)
            {
                //try to grab the first connection string from the section
                var section = configuration.GetSection("ConnectionStrings");

                return section?.GetChildren().FirstOrDefault()?.Value;
            }

            return configuration.GetConnectionString(configuration[ConfigurationKeys.ConnectionStringName.ToString()]);
        }

        public static string? GetConnectionString(this ConfigurationManager configurationManager)
        {
            //first grab the explicitly configured connection string, if it exists
            var connectionString = configurationManager.GetConnectionString(configurationManager[ConfigurationKeys.ConnectionStringName.ToString()]);

            if (connectionString is null)
            {
                //try to grab the first connection string from the section
                var section = configurationManager.GetSection("ConnectionStrings");

                return section?.GetChildren().FirstOrDefault()?.Value;
            }

            return configurationManager.GetConnectionString(configurationManager[ConfigurationKeys.ConnectionStringName.ToString()]);
        }
    }
}
