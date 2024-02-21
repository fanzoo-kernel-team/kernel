using Microsoft.Extensions.Options;

namespace Fanzoo.Kernel.Services
{
    public sealed class EmailServiceFactorySettings
    {
        public const string SectionName = "Email";

        public string Service { get; set; } = default!;
    }

    public interface IEmailServiceFactory
    {
        IEmailService GetService();
    }

    public sealed class EmailServiceFactory(IOptions<EmailServiceFactorySettings> settings, IEnumerable<IEmailService> services) : IEmailServiceFactory
    {
        private readonly string _serviceName = settings.Value.Service;
        private readonly IEnumerable<IEmailService> _services = services;

        public IEmailService GetService()
        {
            var service = _services.SingleOrDefault(s => s.Name == _serviceName);

            return service ?? throw new InvalidOperationException($"No email service found with name {_serviceName}");
        }
    }
}
