using System.Collections.Concurrent;

namespace Fanzoo.Kernel.DependencyInjection
{
    public static class AppDomainExtensions
    {
        public static AppDomain LoadReferencedAssemblies(this AppDomain appDomain, bool includeFramework = false)
        {
            // Source: https://dotnetstories.com/blog/Dynamically-pre-load-assemblies-in-a-ASPNET-Core-or-any-C-project-en-7155735300

            var loaded = new ConcurrentDictionary<string, bool>();

            bool ShouldLoad(string? assemblyName, ConcurrentDictionary<string, bool> loaded) =>
                assemblyName is not null && (includeFramework || NotNetFramework(assemblyName)) && !loaded.ContainsKey(assemblyName);

            bool NotNetFramework(string? assemblyName) => assemblyName is not null && !assemblyName.StartsWith("Microsoft.") && !assemblyName.StartsWith("System.") && assemblyName != "netstandard";

            void LoadReferencedAssembly(Assembly assembly)
            {
                // Check all referenced assemblies of the specified assembly
                foreach (var an in assembly.GetReferencedAssemblies().Where(a => ShouldLoad(a.FullName, loaded)))
                {
                    // Load the assembly and load its dependencies
                    LoadReferencedAssembly(Assembly.Load(an)); // AppDomain.CurrentDomain.Load(name)

                    loaded.TryAdd(an.FullName, true);
                }
            }

            // Populate already loaded assemblies
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies().Where(a => ShouldLoad(a.FullName, loaded)))
            {
                if (a.FullName == null)
                {
                    continue;
                }

                loaded.TryAdd(a.FullName, true);
            }

            // Loop on loaded assemblies to load dependencies (it includes Startup assembly so should load all the dependency tree) 
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => NotNetFramework(a.FullName)))
            {
                LoadReferencedAssembly(assembly);
            }

            return appDomain;
        }
    }
}
