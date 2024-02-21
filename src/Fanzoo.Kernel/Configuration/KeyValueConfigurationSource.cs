namespace Fanzoo.Kernel.Configuration
{
    public class KeyValueConfigurationSource(IDictionary<string, string?> keyValues) : IConfigurationSource
    {
        private readonly IDictionary<string, string?> _keyValues = keyValues;

        public IConfigurationProvider Build(IConfigurationBuilder builder) => new KeyValueConfigurationProvider(_keyValues);
    }
}
