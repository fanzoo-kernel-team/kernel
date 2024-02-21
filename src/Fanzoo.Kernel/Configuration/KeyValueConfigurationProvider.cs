namespace Fanzoo.Kernel.Configuration
{
    public class KeyValueConfigurationProvider(IDictionary<string, string?> keyValues) : ConfigurationProvider
    {
        private readonly IDictionary<string, string?> _keyValues = keyValues;

        public override void Load() => Data = new Dictionary<string, string?>(_keyValues, StringComparer.OrdinalIgnoreCase);
    }
}
