namespace Fanzoo.Kernel.Services
{
    public interface IEmailServiceFactory : IService
    {
        IEmailService GetService();
    }
}
