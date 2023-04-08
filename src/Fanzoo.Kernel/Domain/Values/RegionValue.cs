namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class RegionValue : RequiredStringValue
    {
        private RegionValue() { } //ORM

        public RegionValue(string region) : base(region.ToUpper())
        {
            Guard.Against.NullOrWhiteSpace(region.ToUpper(), nameof(region));
            Guard.Against.NotInList(Regions.Keys, region.ToUpper(), nameof(region));
        }

        public static ValueResult<RegionValue, Error> Create(string region) => CanCreate(region.ToUpper()) ? new RegionValue(region) : Errors.ValueObjects.RegionValue.InvalidRegionCode;

        public static bool CanCreate(string region) => Check.For.IsInList(Regions.Keys, region.ToUpper());

        public static readonly IReadOnlyDictionary<string, string> Regions = new Dictionary<string, string>()
        {
            { "AL", "Alabama" },
            { "AK", "Alaska" },
            { "AZ", "Arizona" },
            { "AR", "Arkansas" },
            { "CA", "California" },
            { "CO", "Colorado" },
            { "CT", "Connecticut" },
            { "DE", "Delaware" },
            { "DC", "District of Columbia" },
            { "FL", "Florida" },
            { "GA", "Georgia" },
            { "HI", "Hawaii" },
            { "ID", "Idaho" },
            { "IL", "Illinois" },
            { "IN", "Indiana" },
            { "IA", "Iowa" },
            { "KS", "Kansas" },
            { "KY", "Kentucky" },
            { "LA", "Louisiana" },
            { "ME", "Maine" },
            { "MD", "Maryland" },
            { "MA", "Massachusetts" },
            { "MI", "Michigan" },
            { "MN", "Minnesota" },
            { "MS", "Mississippi" },
            { "MO", "Missouri" },
            { "MT", "Montana" },
            { "NE", "Nebraska" },
            { "NV", "Nevada" },
            { "NH", "New Hampshire" },
            { "NJ", "New Jersey" },
            { "NM", "New Mexico" },
            { "NY", "New York" },
            { "NC", "North Carolina" },
            { "ND", "North Dakota" },
            { "OH", "Ohio" },
            { "OK", "Oklahoma" },
            { "OR", "Oregon" },
            { "PA", "Pennsylvania" },
            { "RI", "Rhode Island" },
            { "SC", "South Carolina" },
            { "SD", "South Dakota" },
            { "TN", "Tennessee" },
            { "TX", "Texas" },
            { "UT", "Utah" },
            { "VT", "Vermont" },
            { "VA", "Virginia" },
            { "WA", "Washington" },
            { "WV", "West Virginia" },
            { "WI", "Wisconsin" },
            { "WY", "Wyoming" }
        };

    }
}
