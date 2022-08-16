using System.Text.RegularExpressions;
using Fanzoo.Kernel.SendGrid.Services.Configuration;
using Fanzoo.Kernel.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Attachment = SendGrid.Helpers.Mail.Attachment;

namespace Fanzoo.Kernel.SendGrid.Services
{
    public sealed class SendGridEmailService : IEmailService
    {
        private readonly SendGridSmtpSettings _sendGridSettings;

        public SendGridEmailService(IOptions<SendGridSmtpSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public async ValueTask SendEmailAsync(string[] to, string[] cc, string[] bcc, string fromEmail, string? fromName, string subject, string htmlContent, string? plainTextContent, EmailAttachment[] attachments)
        {
            var client = new SendGridClient(_sendGridSettings.SendGrid.ApiKey);

            if (plainTextContent.IsNullOrWhitespace())
            {
                plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
            }

            var email = new SendGridMessage()
            {
                From = fromEmail.ToEmailAddress(fromName),
                Subject = subject,
                HtmlContent = htmlContent,
                PlainTextContent = plainTextContent
            };

            email.AddTos(to.Select(t => t.ToEmailAddress()).ToList());

            if (cc.Length > 0)
            {
                email.AddCcs(cc.Select(c => c.ToEmailAddress()).ToList());
            }

            if (bcc.Length > 0)
            {
                email.AddBccs(bcc.Select(b => b.ToEmailAddress()).ToList());
            }

            if (attachments.Length > 0)
            {
                email.AddAttachments(attachments.Select(a => a.ToSendGridAttachment()).ToList());
            }

#if DEBUG
#pragma warning disable S1481 // Unused local variables should be removed
            var result = await client.SendEmailAsync(email);
#pragma warning restore S1481 // Unused local variables should be removed
#else
            _ = await client.SendEmailAsync(email);
#endif
        }

        public async ValueTask SendEmailAsync(string to, string subject, string htmlContent) =>
            await SendEmailAsync(new string[] { to }, Array.Empty<string>(), Array.Empty<string>(), _sendGridSettings.From, null, subject, htmlContent, null, Array.Empty<EmailAttachment>());

        public async ValueTask SendEmailAsync(string to, string fromEmail, string fromName, string subject, string htmlContent) =>
            await SendEmailAsync(new string[] { to }, Array.Empty<string>(), Array.Empty<string>(), fromEmail, fromName, subject, htmlContent, null, Array.Empty<EmailAttachment>());
    }

    internal static class Extensions
    {
        internal static EmailAddress ToEmailAddress(this string value, string? name = null) => new(value, name);

        internal static Attachment ToSendGridAttachment(this EmailAttachment attachment) => new()
        {
            Content = Convert.ToBase64String(attachment.Data),
            Filename = attachment.Filename,
            Type = attachment.MIMEType
        };
    }
}
