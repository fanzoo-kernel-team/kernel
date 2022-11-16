namespace Fanzoo.Kernel.Configuration
{
    public class KeyValueConfigurationProvider : ConfigurationProvider
    {
        private readonly IDictionary<string, string?> _keyValues;

        public KeyValueConfigurationProvider(IDictionary<string, string?> keyValues)
        {
            _keyValues = keyValues;
        }

        public override void Load() => Data = new Dictionary<string, string?>(_keyValues, StringComparer.OrdinalIgnoreCase);
    }
}
