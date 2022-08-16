namespace Fanzoo.Kernel.Services
{
    public interface IEmailService : IService
    {
        ValueTask SendEmailAsync(string[] to, string[] cc, string[] bcc, string fromEmail, string fromName, string subject, string htmlContent, string plainTextContent, EmailAttachment[] attachments);

        ValueTask SendEmailAsync(string to, string subject, string htmlContent);

        ValueTask SendEmailAsync(string to, string fromEmail, string fromName, string subject, string htmlContent);
    }
}
