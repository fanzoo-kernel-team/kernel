namespace Fanzoo.Kernel.Services
{
    public record struct EmailAttachment(byte[] Data, string MIMEType, string Filename);

    public interface IEmailService : IService
    {
        string Name => GetType().Name;

        ValueTask SendEmailAsync(string[] to, string[] cc, string[] bcc, string from, string subject, string? htmlContent = null, string? plainTextContent = null, EmailAttachment[]? attachments = null);

        ValueTask SendEmailAsync(string to, string subject, string content, bool isHtml = true);

        ValueTask SendEmailAsync(string to, string subject, string content, EmailAttachment[] attachments, bool isHtml = true);

        ValueTask SendEmailAsync(string to, string from, string subject, string content, bool isHtml = true);

        ValueTask SendEmailAsync(string to, string from, string subject, string content, EmailAttachment[] attachments, bool isHtml = true);

        ValueTask SendEmailAsync(string[] recipients, string subject, string content, bool isHtml = true);

        ValueTask SendEmailAsync(string[] recipients, string subject, string content, EmailAttachment[] attachments, bool isHtml = true);

        ValueTask SendEmailAsync(string[] recipients, string from, string subject, string content, bool isHtml = true);

        ValueTask SendEmailAsync(string[] recipients, string from, string subject, string content, EmailAttachment[] attachments, bool isHtml = true);
    }
}

namespace System
{
    public static class EmailExtensions
    {
        public static (string? DisplayName, string Email) ParseNameAndEmailAddress(this string nameAndEmailAddress)
        {
            var displayName = nameAndEmailAddress.ValueFromStart("<")?.Trim();

            var email = nameAndEmailAddress.ValueBetween("<", ">")?.Trim();

            if (email is null)
            {
                email = nameAndEmailAddress.Trim();

                if (email is null)
                {
                    throw new InvalidOperationException("No email found.");
                }
            }

            return (displayName, email);
        }
    }
}
