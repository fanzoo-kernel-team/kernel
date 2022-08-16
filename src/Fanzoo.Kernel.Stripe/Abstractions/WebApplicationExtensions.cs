using System.Reflection;
using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.DependencyInjection;
using Fanzoo.Kernel.Stripe.Services.Configuration;
using Microsoft.AspNetCore.Builder;

namespace Fanzoo.Kernel.Stripe.Abstractions
{
    public static class WebApplicationExtensions
    {
        public static WebApplicationBuilder AddStripeFromAssemblies(this WebApplicationBuilder builder, Assembly[] assemblies, string section = "Stripe")
        {
            builder.Services.AddStripeCore(assemblies);

            builder.AddSmtpSettings(section);

            return builder;
        }

        public static WebApplicationBuilder AddStripeFromAssemblies(this WebApplicationBuilder builder, Action<IServiceTypeAssemblyBuilder> addTypes, string section = "Stripe")
        {
            builder.Services.AddStripeCore(addTypes);

            builder.AddSmtpSettings(section);

            return builder;
        }

        public static WebApplicationBuilder AddStripeFromAssembly(this WebApplicationBuilder builder, Assembly assembly, string section = "Stripe")
        {
            builder.Services.AddStripeCore(assembly);

            builder.AddSmtpSettings(section);

            return builder;
        }

        public static WebApplicationBuilder AddStripeFromAssembly(this WebApplicationBuilder builder, string assemblyName, string section = "Stripe")
        {
            builder.Services.AddStripeCore(assemblyName);

            builder.AddSmtpSettings(section);

            return builder;
        }

        private static WebApplicationBuilder AddSmtpSettings(this WebApplicationBuilder builder, string section) => builder.AddSetting<StripeSettings>(section);
    }
}
