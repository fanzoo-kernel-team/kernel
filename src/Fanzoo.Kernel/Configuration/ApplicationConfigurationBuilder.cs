namespace Fanzoo.Kernel.Configuration
{
    public class ApplicationConfigurationBuilder
    {
        private IConfigurationBuilder _configurationBuilder;

        private Dictionary<string, string?> _keyValues;

        public ApplicationConfigurationBuilder()
        {
            _configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();

            _keyValues = new();
        }

        public ApplicationConfigurationBuilder AddJsonFile(string pathFileName, bool optional = true)
        {
            _configurationBuilder = _configurationBuilder.AddJsonFile(pathFileName, optional);

            return this;
        }

        public ApplicationConfigurationBuilder AddAppSettings(bool optional = true)
        {
            _configurationBuilder = _configurationBuilder.AddJsonFile("appsettings.json", optional);

            return this;
        }

        public ApplicationConfigurationBuilder AddEnvironmentVariables()
        {
            _configurationBuilder = _configurationBuilder.AddEnvironmentVariables();

            return this;
        }

        public ApplicationConfigurationBuilder AddEnvironmentVariables(string prefix)
        {
            _configurationBuilder = _configurationBuilder.AddEnvironmentVariables(prefix);

            return this;
        }

        public ApplicationConfigurationBuilder AddSetting(string key, string value)
        {
            _keyValues[key] = value;

            return this;
        }

        public ApplicationConfigurationBuilder AddSettings(IDictionary<string, string> settings)
        {
            _keyValues = new Dictionary<string, string>(_keyValues.Union(settings));

            return this;
        }

        public ApplicationConfigurationBuilder AddConfiguration(IConfiguration configuration)
        {
            _configurationBuilder = _configurationBuilder.AddConfiguration(configuration);

            return this;
        }

        public ApplicationConfigurationBuilder AddConnectionString(string name, string connectionString)
        {
            _keyValues.Add(ConfigurationKeys.ConnectionStringName.ToString(), name);
            _keyValues.Add($"ConnectionStrings:{name}", connectionString);

            return this;
        }

        public IConfiguration Build()
        {
            var source = new KeyValueConfigurationSource(_keyValues);

            _configurationBuilder.Add(source);

            return _configurationBuilder.Build();
        }
    }
}
