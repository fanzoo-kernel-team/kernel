using System.Reflection;
using Fanzoo.Kernel.DependencyInjection;
using Fanzoo.Kernel.SendGrid.Services;
using Fanzoo.Kernel.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanzoo.Kernel.SendGrid
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddSendGridCore(this IServiceCollection services, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var service = assembly.GetTypes()
                    .Where(t => t.IsClass)
                        .FirstOrDefault(t => t == typeof(SendGridEmailService));

                if (service is not null)
                {
                    services.AddTransient(typeof(IEmailService), service);

                    break;
                }
            }

            return services;
        }

        public static IServiceCollection AddSendGridCore(this IServiceCollection services, Action<IServiceTypeAssemblyBuilder> addTypes)
        {
            var serviceTypeBuilder = new ServiceTypeAssemblyBuilder();

            addTypes.Invoke(serviceTypeBuilder);

            return services.AddSendGridCore(serviceTypeBuilder.Assemblies.ToArray());

        }

        public static IServiceCollection AddSendGridCore(this IServiceCollection services, Assembly assembly) => services.AddSendGridCore(new[] { assembly });

        public static IServiceCollection AddSendGridCore(this IServiceCollection services, string assemblyName) => services.AddSendGridCore(Assembly.Load(assemblyName));
    }
}
