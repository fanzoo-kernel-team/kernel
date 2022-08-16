using System.Text;

namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class AddressValue : ValueObject
    {
        private AddressValue() { } //ORM

        public AddressValue(string primaryAddress, string? secondaryAddress, string city, RegionValue region, PostalCodeValue postalCode)
        {
            Guard.Against.NullOrWhiteSpace(primaryAddress, nameof(primaryAddress));
            Guard.Against.NullOrWhiteSpace(city, nameof(city));
            PrimaryAddress = primaryAddress;
            SecondaryAddress = secondaryAddress;
            City = city;
            Region = region;
            PostalCode = postalCode;
        }

        public static Result<AddressValue, Error> Create(string primaryAddress, string? secondaryAddress, string city, RegionValue region, PostalCodeValue postalCode)
        {
            var isValid = Check.For
                .NullOrWhiteSpace(primaryAddress)
                .And
                .NullOrWhiteSpace(city)
                    .IsValid;

            return isValid
                ? new AddressValue(primaryAddress, secondaryAddress, city, region, postalCode)
                : Errors.ValueObjects.AddressValue.InvalidFormat;
        }

        public string PrimaryAddress { get; private set; } = default!;

        public string? SecondaryAddress { get; private set; } = default!;

        public string City { get; private set; } = default!;

        public RegionValue Region { get; private set; } = default!;

        public PostalCodeValue PostalCode { get; private set; } = default!;

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

        protected override IEnumerable<object> GetEqualityComponents()
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
    }
}
