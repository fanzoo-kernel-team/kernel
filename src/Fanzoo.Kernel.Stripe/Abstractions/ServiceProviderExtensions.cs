using System.Reflection;
using Fanzoo.Kernel.DependencyInjection;
using Fanzoo.Kernel.Services;
using Fanzoo.Kernel.Stripe.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Fanzoo.Kernel.Stripe
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddStripeCore(this IServiceCollection services, Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var service = assembly.GetTypes()
                    .Where(t => t.IsClass)
                        .FirstOrDefault(t => t == typeof(StripePaymentService));

                if (service is not null)
                {
                    services.AddTransient(typeof(IPaymentService<StripePaymentRequest, StripePaymentResult, StripeCreateCustomerRequest, StripeCreateCustomerResult, StripeCancelPaymentRequest, object?>), service);

                    break;
                }
            }

            return services;
        }

        public static IServiceCollection AddStripeCore(this IServiceCollection services, Action<IServiceTypeAssemblyBuilder> addTypes)
        {
            var serviceTypeBuilder = new ServiceTypeAssemblyBuilder();

            addTypes.Invoke(serviceTypeBuilder);

            return services.AddStripeCore(serviceTypeBuilder.Assemblies.ToArray());

        }

        public static IServiceCollection AddStripeCore(this IServiceCollection services, Assembly assembly) => services.AddStripeCore(new[] { assembly });

        public static IServiceCollection AddStripeCore(this IServiceCollection services, string assemblyName) => services.AddStripeCore(Assembly.Load(assemblyName));
    }
}
