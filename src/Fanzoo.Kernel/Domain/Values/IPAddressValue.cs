namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class IPAddressValue : RequiredStringValue
    {
        private IPAddressValue() { } //ORM

        public IPAddressValue(string ipAddress) : base(ipAddress)
        {
            Guard.Against.NullOrWhiteSpace(ipAddress, nameof(ipAddress));
            Guard.Against.InvalidIPAddress(ipAddress, nameof(ipAddress));
        }

        public static ValueResult<IPAddressValue, Error> Create(string ipAddress) => CanCreate(ipAddress)
                ? new IPAddressValue(ipAddress)
                : Errors.ValueObjects.IPAddressValue.InvalidFormat;


        public static implicit operator IPAddressValue(string value) => new(value);

        public static bool CanCreate(string ipAddress) => Check.For.IsValidIPAddress(ipAddress);
    }
}
