namespace Fanzoo.Kernel.DependencyInjection
{
    public static partial class ServiceProviderExtensions
    {
        public static IServiceCollection AddAutoMapperCore(this IServiceCollection services, Assembly[] assemblies) => services.AddAutoMapper(assemblies);

        public static IServiceCollection AddAutoMapperCore(this IServiceCollection services, Action<IServiceTypeAssemblyBuilder> addTypes)
        {
            var serviceTypeBuilder = new ServiceTypeAssemblyBuilder();

            addTypes.Invoke(serviceTypeBuilder);

            return services.AddAutoMapperCore(serviceTypeBuilder.Assemblies.ToArray());

        }

        public static IServiceCollection AddAutoMapperCore(this IServiceCollection services, Assembly assembly) => services.AddAutoMapperCore(new[] { assembly });

        public static IServiceCollection AddAutoMapperCore(this IServiceCollection services, string assemblyName) => services.AddAutoMapperCore(Assembly.Load(assemblyName));
    }
}
