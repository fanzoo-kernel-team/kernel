using System.Reflection;
using Fanzoo.Kernel.Builder;
using Fanzoo.Kernel.DependencyInjection;
using Fanzoo.Kernel.SendGrid.Services.Configuration;
using Microsoft.AspNetCore.Builder;

namespace Fanzoo.Kernel.SendGrid
{
    public static class WebApplicationExtensions
    {
        public static WebApplicationBuilder AddSendGridFromAssemblies(this WebApplicationBuilder builder, Assembly[] assemblies, string section = "Smtp")
        {
            builder.Services.AddSendGridCore(assemblies);

            builder.AddSmtpSettings(section);

            return builder;
        }

        public static WebApplicationBuilder AddSendGridFromAssemblies(this WebApplicationBuilder builder, Action<IServiceTypeAssemblyBuilder> addTypes, string section = "Smtp")
        {
            builder.Services.AddSendGridCore(addTypes);

            builder.AddSmtpSettings(section);

            return builder;
        }

        public static WebApplicationBuilder AddSendGridFromAssembly(this WebApplicationBuilder builder, Assembly assembly, string section = "Smtp")
        {
            builder.Services.AddSendGridCore(assembly);

            builder.AddSmtpSettings(section);

            return builder;
        }

        public static WebApplicationBuilder AddSendGridFromAssembly(this WebApplicationBuilder builder, string assemblyName, string section = "Smtp")
        {
            builder.Services.AddSendGridCore(assemblyName);

            builder.AddSmtpSettings(section);

            return builder;
        }

        private static WebApplicationBuilder AddSmtpSettings(this WebApplicationBuilder builder, string section) => builder.AddSetting<SendGridSmtpSettings>(section);
    }
}
