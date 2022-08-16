namespace Fanzoo.Kernel.Services
{
    public interface IDynamicMappingService
    {
        IEnumerable<T> Map<T>(IEnumerable<dynamic> items);
    }
}
