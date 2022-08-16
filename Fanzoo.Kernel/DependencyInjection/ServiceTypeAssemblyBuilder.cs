namespace Fanzoo.Kernel.DependencyInjection
{
    public sealed class ServiceTypeAssemblyBuilder : IServiceTypeAssemblyBuilder
    {
        private readonly List<Assembly> _assemblies = new();

        public ServiceTypeAssemblyBuilder()
        {
            //pre-load assemblies
            AppDomain.CurrentDomain.LoadReferencedAssemblies();
        }

        public IEnumerable<Assembly> Assemblies => _assemblies;

        public IServiceTypeAssemblyBuilder FromAssembly(Assembly assembly)
        {
            AddAssemblies(new[] { assembly });

            return this;
        }

        public IServiceTypeAssemblyBuilder FromUnloadedAssembly(string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);

            return FromAssembly(assembly);
        }

        public IServiceTypeAssemblyBuilder FromAssemblyOf<T>()
        {
            var assembly = typeof(T).Assembly;

            return FromAssembly(assembly);
        }

        public IServiceTypeAssemblyBuilder FromAssemblyOf(Type type)
        {
            var assembly = type.Assembly;

            return FromAssembly(assembly);

        }

        public IServiceTypeAssemblyBuilder WhereTheNameStartsWith(string namePart)
        {
            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                    .Where(a => a.GetName().Name!.StartsWith(namePart))
                        .ToArray();

            AddAssemblies(assemblies);

            return this;

        }

        private void AddAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (!_assemblies.Contains(assembly))
                {
                    _assemblies.Add(assembly);
                }
            }
        }
    }
}
