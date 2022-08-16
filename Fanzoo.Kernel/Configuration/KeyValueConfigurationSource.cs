using Microsoft.Extensions.Configuration;

namespace Fanzoo.Kernel.Configuration
{
    public class KeyValueConfigurationSource : IConfigurationSource
    {
        private readonly IDictionary<string, string> _keyValues;

        public KeyValueConfigurationSource(IDictionary<string, string> keyValues)
        {
            _keyValues = keyValues;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) => new KeyValueConfigurationProvider(_keyValues);
    }
}
