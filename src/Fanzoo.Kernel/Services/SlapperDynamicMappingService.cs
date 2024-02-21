namespace Fanzoo.Kernel.Services
{
    public sealed class SlapperDynamicMappingService : IDynamicMappingService
    {
        public SlapperDynamicMappingService() => Slapper.AutoMapper.Configuration.ApplyDefaultIdentifierConventions();

        public IEnumerable<T> Map<T>(IEnumerable<dynamic> items) => Slapper.AutoMapper.MapDynamic<T>(items);
    }
}
