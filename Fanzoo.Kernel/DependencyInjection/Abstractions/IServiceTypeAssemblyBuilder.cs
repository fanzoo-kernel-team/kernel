namespace Fanzoo.Kernel.DependencyInjection
{
    public interface IServiceTypeAssemblyBuilder
    {
        public IServiceTypeAssemblyBuilder WhereTheNameStartsWith(string namePart);

        public IServiceTypeAssemblyBuilder FromUnloadedAssembly(string assemblyName);

        public IServiceTypeAssemblyBuilder FromAssembly(Assembly assembly);

        public IServiceTypeAssemblyBuilder FromAssemblyOf<T>();

        public IServiceTypeAssemblyBuilder FromAssemblyOf(Type type);

        public IEnumerable<Assembly> Assemblies { get; }
    }
}
