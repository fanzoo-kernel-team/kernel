namespace Fanzoo.Kernel.Builder
{
    public sealed class EmailFactoryBuilder
    {
        public EmailFactoryBuilder(WebApplicationBuilder builder)
        {
            WebApplicationBuilder = builder;
        }

        public WebApplicationBuilder WebApplicationBuilder { get; private set; }
    }

    public static class EmailFactoryBuilderExtensions
    {
        public static EmailFactoryBuilder AddSmtp(this EmailFactoryBuilder builder)
        {
            builder.WebApplicationBuilder.AddTransient<IEmailService, SmtpEmailService>();

            builder.WebApplicationBuilder.AddSetting<SmtpSettings>(SmtpSettings.SectionName);

            return builder;
        }
    }

    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddEmailFactory(this WebApplicationBuilder builder, Action<EmailFactoryBuilder> configuration)
        {
            builder.Services.AddTransient<IEmailServiceFactory, EmailServiceFactory>();

            builder.AddSetting<EmailServiceFactorySettings>(EmailServiceFactorySettings.SectionName);

            var factoryBuilder = new EmailFactoryBuilder(builder);

            configuration.Invoke(factoryBuilder);

            return builder;
        }
    }
}
