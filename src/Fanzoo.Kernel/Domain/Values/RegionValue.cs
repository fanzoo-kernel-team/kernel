namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class RegionValue : RequiredStringValue
    {
        private RegionValue() { } //ORM

        private RegionValue(string regionAbbreviation) : base(regionAbbreviation) { }

        public static ValueResult<RegionValue, Error> Create(string regionAbbreviation)
        {
            regionAbbreviation = regionAbbreviation.ToUpper();

            var isValid = Check.For
                .NullOrWhiteSpace(regionAbbreviation)
                .And
                .NotInList(GetRegions(), regionAbbreviation)
                    .IsValid;

            return isValid ? new RegionValue(regionAbbreviation) : Errors.ValueObjects.RegionValue.InvalidRegionCode;
        }

        public static IEnumerable<string> GetRegions() => new string[]
            {
                //"AB",
                //"BC",
                //"MB",
                //"NB",
                //"NL",
                //"NT",
                //"NS",
                //"NU",
                //"ON",
                //"PE",
                //"QC",
                //"SK",
                //"YT",
                //<- end canada

                "AL",
                "AK",
                "AS",
                "AZ",
                "AR",
                "CA",
                "CO",
                "CT",
                "DE",
                "DC",
                "FM",
                "FL",
                "GA",
                "GU",
                "HI",
                "ID",
                "IL",
                "IN",
                "IA",
                "KS",
                "KY",
                "LA",
                "ME",
                "MH",
                "MD",
                "MA",
                "MI",
                "MN",
                "MS",
                "MO",
                "MT",
                "NE",
                "NV",
                "NH",
                "NJ",
                "NM",
                "NY",
                "NC",
                "ND",
                "MP",
                "OH",
                "OK",
                "OR",
                "PW",
                "PA",
                "PR",
                "RI",
                "SC",
                "SD",
                "TN",
                "TX",
                "UT",
                "VT",
                "VI",
                "VA",
                "WA",
                "WV",
                "WI",
                "WY"
            };
    }
}
