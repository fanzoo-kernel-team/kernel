using System.Text;

namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class AddressValue : ValueObject
    {
        private AddressValue() { } //ORM

        public AddressValue(string primaryAddress, string? secondaryAddress, string city, RegionValue region, USPostalCodeValue postalCode)
        {
            Guard.Against.NullOrWhiteSpace(primaryAddress, nameof(primaryAddress));
            Guard.Against.NullOrWhiteSpace(city, nameof(city));

            PrimaryAddress = primaryAddress;
            SecondaryAddress = secondaryAddress;
            City = city;
            Region = region;
            PostalCode = postalCode;
        }

        public static ValueResult<AddressValue, Error> Create(string primaryAddress, string? secondaryAddress, string city, RegionValue region, USPostalCodeValue postalCode) => CanCreate(primaryAddress, city, postalCode)
                ? new AddressValue(primaryAddress, secondaryAddress, city, region, postalCode)
                : Errors.ValueObjects.AddressValue.InvalidFormat;


        public string PrimaryAddress { get; private set; } = default!;

        public string? SecondaryAddress { get; private set; } = default!;

        public string City { get; private set; } = default!;

        public RegionValue Region { get; private set; } = default!;

        public USPostalCodeValue PostalCode { get; private set; } = default!;

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(PrimaryAddress);

            if (SecondaryAddress != null)
            {
                sb.AppendLine(SecondaryAddress);
            }

            sb.AppendLine($"{City}, {Region} {PostalCode}");

            return sb.ToString();
        }

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return PrimaryAddress;

            if (SecondaryAddress is not null)
            {
                yield return SecondaryAddress;
            }

            yield return City;
            yield return Region;
            yield return PostalCode;
        }

        public static bool CanCreate(string primaryAddress, string? city, USPostalCodeValue postalCode) => Check.For.IsValidAddress(primaryAddress, city, postalCode);
    }
}
