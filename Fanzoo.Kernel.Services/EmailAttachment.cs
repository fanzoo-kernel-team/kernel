namespace Fanzoo.Kernel.Services
{

    //TODO: make this a record and move to the interface

    public sealed class EmailAttachment
    {
        public EmailAttachment(byte[] data, string mimeType, string fileName)
        {
            Data = data;
            MIMEType = mimeType;
            Filename = fileName;
        }

        public byte[] Data { get; set; }

        public string MIMEType { get; set; }

        public string Filename { get; set; }
    }
}
