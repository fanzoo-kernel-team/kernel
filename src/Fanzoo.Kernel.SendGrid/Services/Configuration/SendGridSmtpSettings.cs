namespace Fanzoo.Kernel.SendGrid.Services.Configuration
{
    public class SendGridSmtpSettings
    {
        public string From { get; set; } = default!;

        public SendGridSettings SendGrid { get; set; } = default!;

        public class SendGridSettings
        {
            public string ApiKey { get; set; } = default!;
        }
    }
}
