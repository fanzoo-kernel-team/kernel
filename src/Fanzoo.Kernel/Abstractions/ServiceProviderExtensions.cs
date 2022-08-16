using System.Collections;

namespace System
{
    public static class ServiceProviderExtensions
    {
        public static IEnumerable<Type> GetConfiguredServiceTypes(this IServiceProvider provider)
        {
            if (provider.GetType().Name == "ServiceProviderEngineScope")
            {
                var rootProvider = provider.GetPropertyValue("RootProvider");

                var callSiteFactory = rootProvider?.GetPropertyValue("CallSiteFactory");

                var descriptorLookup = callSiteFactory?.GetFieldValue("_descriptorLookup");

                if (descriptorLookup is IDictionary descriptors)
                {
                    foreach (DictionaryEntry entry in descriptors)
                    {
                        yield return (Type)entry.Key;
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"{nameof(provider)} is not supported.");
            }
        }
    }
}
