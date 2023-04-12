using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Fanzoo.Kernel.Services
{
    public enum SmtpDeliveryMethod
    {
        Network,
        SpecifiedPickupDirectory
    }

    public sealed class SmtpSettings
    {
        public const string SectionName = $"{EmailServiceFactorySettings.SectionName}:Settings";

        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;

        public string Host { get; set; } = default!;

        public int Port { get; set; } = 587;

        public string? From { get; set; } = default!;

        public bool RequiresAuthentication { get; set; } = false;

        public string? Username { get; set; } = default!;

        public string? Password { get; set; } = default!;

        public string? PickupDirectoryLocation { get; set; } = default!;
    }

    public sealed class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _settings;

        public SmtpEmailService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public string Name => "Smtp";

        public async ValueTask SendEmailAsync(string[] to, string[] cc, string[] bcc, string from, string subject, string? htmlContent = null, string? plainTextContent = null, EmailAttachment[]? attachments = null)
        {
            //create the message
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(from));

            message.To.AddRange(to.Select(t => MailboxAddress.Parse(t)));

            message.Cc.AddRange(cc.Select(c => MailboxAddress.Parse(c)));

            message.Bcc.AddRange(bcc.Select(b => MailboxAddress.Parse(b)));

            var multipart = new Multipart("mixed");

            if (plainTextContent is not null || htmlContent is not null)
            {
                var alternative = new Multipart("alternative");

                if (plainTextContent is not null)
                {
                    alternative.Add(new TextPart("plain") { Text = plainTextContent });
                }

                if (htmlContent is not null)
                {
                    alternative.Add(new TextPart("html") { Text = htmlContent });
                }

                multipart.Add(alternative);
            }

            if (attachments is not null)
            {
                foreach (var attachment in attachments)
                {
                    multipart.Add(attachment.ToMimePart());
                }
            }

            message.Body = multipart;

            //handle message delivery
            switch (_settings.DeliveryMethod)
            {
                case SmtpDeliveryMethod.Network:
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);

                        if (_settings.RequiresAuthentication)
                        {
                            await client.AuthenticateAsync(_settings.Username, _settings.Password);
                        }

                        await client.SendAsync(message);

                        await client.DisconnectAsync(true);
                    }

                    break;

                case SmtpDeliveryMethod.SpecifiedPickupDirectory:
                    var filename = Path.Combine(_settings.OutputDirectory ?? string.Empty, $"{Guid.NewGuid()}.eml");

                    await message.WriteToAsync(filename);

                    break;
            }
        }

        public ValueTask SendEmailAsync(string to, string subject, string content, bool isHtml = true) => throw new NotImplementedException();

        public ValueTask SendEmailAsync(string to, string subject, string content, EmailAttachment[] attachments, bool isHtml = true) => throw new NotImplementedException();

        public ValueTask SendEmailAsync(string to, string from, string subject, string content, bool isHtml = true) => throw new NotImplementedException();

        public ValueTask SendEmailAsync(string to, string from, string subject, string content, EmailAttachment[] attachments, bool isHtml = true) => throw new NotImplementedException();

        public ValueTask SendEmailAsync(string[] recipients, string subject, string content, bool isHtml = true) => throw new NotImplementedException();

        public ValueTask SendEmailAsync(string[] recipients, string subject, string content, EmailAttachment[] attachments, bool isHtml = true) => throw new NotImplementedException();

        public ValueTask SendEmailAsync(string[] recipients, string from, string subject, string content, bool isHtml = true) => throw new NotImplementedException();

        public ValueTask SendEmailAsync(string[] recipients, string from, string subject, string content, EmailAttachment[] attachments, bool isHtml = true) => throw new NotImplementedException();
    }

    static file class Extensions
    {
        public static MimePart ToMimePart(this EmailAttachment attachment) => new(attachment.MIMEType)
        {
            Content = new MimeContent(new MemoryStream(attachment.Data), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = attachment.Filename
        };
    }
}
