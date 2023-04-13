namespace Fanzoo.Kernel.Services
{
    public record struct EmailAttachment(byte[] Data, string MIMEType, string Filename);

    public interface IEmailService
    {
        string Name => GetType().Name;

        ValueTask SendEmailAsync(string[] to, string subject, string? from = null, string[]? cc = null, string[]? bcc = null, string? htmlContent = null, string? plainTextContent = null, EmailAttachment[]? attachments = null);
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
