namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class IPAddressValue : RequiredStringValue
    {
        private IPAddressValue() { } //ORM

        public IPAddressValue(string ipAddress) : base(ipAddress)
        {
            Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));
            Guard.Against.NonMatchingRegex(ipAddress, IPAddressPattern, nameof(ipAddress));
        }

        public static ValueResult<IPAddressValue, Error> Create(string ipAddress)  => CanCreate(ipAddress)     
                ? new IPAddressValue(ipAddress)
                : Errors.ValueObjects.IPAddressValue.InvalidFormat;
        

        public static implicit operator IPAddressValue(string value) => new(value);

        private const string IPAddressPattern = @"^(\b25[0-5]|\b2[0-4][0-9]|\b[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$";

        public static bool CanCreate(string ipAddress) => Check.For.IsValidIPAddress(ipAddress);
    }
}
