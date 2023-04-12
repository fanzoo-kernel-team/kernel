using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Services
{
    public sealed class EmailServiceFactorySettings
    {
        public const string SectionName = "Email";

        public string Service { get; set; } = default!;
    }

    public sealed class EmailServiceFactory : IEmailServiceFactory
    {
        private readonly string _serviceName;
        private readonly IEnumerable<IEmailService> _services;

        public EmailServiceFactory(IOptions<EmailServiceFactorySettings> settings, IEnumerable<IEmailService> services)
        {
            _services = services;
            _serviceName = settings.Value.Service;
        }

        public IEmailService GetService()
        {
            var service = _services.SingleOrDefault(s => s.Name == _serviceName);

            return service ?? throw new InvalidOperationException($"No email service found with name {_serviceName}");
        }
    }
}
