using System.Reflection;

namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class CurrencyValue : ValueObject
    {
        private bool _initialized = false;

        private string _code = default!;

        private CurrencyValue() { } //ORM

        private CurrencyValue(string name, string code, int number, int minorUnits)
        {
            Name = name;
            Code = code;
            Number = number;
            MinorUnits = minorUnits;
        }

        public string Name { get; private set; } = default!;

        public string Code
        {
            get => _code;

            set
            {
                if (_initialized)
                {
                    var currency = Find(value);

                    Name = currency.Name;
                    Number = currency.Number;
                    MinorUnits = currency.MinorUnits;
                }

                _code = value;
                _initialized = true;
            }
        }

        public int Number { get; private set; }

        public int MinorUnits { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }

        private static CurrencyValue Find(string code) =>
            typeof(CurrencyValue)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.FieldType == typeof(CurrencyValue))
                        .Select(f => f.GetValue(null))
                            .Cast<CurrencyValue>()
                                .Single(f => f.Code.Equals(code));

        //currencies based on ISO 4217 (October 1, 2021)
        //https://www.six-group.com/en/products-services/financial-information/data-standards.html#scrollTo=current-historical-lists

        public static readonly CurrencyValue Empty = new(string.Empty, string.Empty, 0, 0);

        public static readonly CurrencyValue Afghani = new("Afghani", "AFN", 971, 2);

        public static readonly CurrencyValue Euro = new("Euro", "EUR", 978, 2);

        public static readonly CurrencyValue Lek = new("Lek", "ALL", 8, 2);

        public static readonly CurrencyValue AlgerianDinar = new("Algerian Dinar", "DZD", 12, 2);

        public static readonly CurrencyValue USDollar = new("US Dollar", "USD", 840, 2);

        public static readonly CurrencyValue Kwanza = new("Kwanza", "AOA", 973, 2);

        public static readonly CurrencyValue EastCaribbeanDollar = new("East Caribbean Dollar", "XCD", 951, 2);

        public static readonly CurrencyValue ArgentinePeso = new("Argentine Peso", "ARS", 32, 2);

        public static readonly CurrencyValue ArmenianDram = new("Armenian Dram", "AMD", 51, 2);

        public static readonly CurrencyValue ArubanFlorin = new("Aruban Florin", "AWG", 533, 2);

        public static readonly CurrencyValue AustralianDollar = new("Australian Dollar", "AUD", 36, 2);

        public static readonly CurrencyValue AzerbaijanManat = new("Azerbaijan Manat", "AZN", 944, 2);

        public static readonly CurrencyValue BahamianDollar = new("Bahamian Dollar", "BSD", 44, 2);

        public static readonly CurrencyValue BahrainiDinar = new("Bahraini Dinar", "BHD", 48, 3);

        public static readonly CurrencyValue Taka = new("Taka", "BDT", 50, 2);

        public static readonly CurrencyValue BarbadosDollar = new("Barbados Dollar", "BBD", 52, 2);

        public static readonly CurrencyValue BelarusianRuble = new("Belarusian Ruble", "BYN", 933, 2);

        public static readonly CurrencyValue BelizeDollar = new("Belize Dollar", "BZD", 84, 2);

        public static readonly CurrencyValue CFAFrancBCEAO = new("CFA Franc BCEAO", "XOF", 952, 0);

        public static readonly CurrencyValue BermudianDollar = new("Bermudian Dollar", "BMD", 60, 2);

        public static readonly CurrencyValue IndianRupee = new("Indian Rupee", "INR", 356, 2);

        public static readonly CurrencyValue Ngultrum = new("Ngultrum", "BTN", 64, 2);

        public static readonly CurrencyValue Boliviano = new("Boliviano", "BOB", 68, 2);

        public static readonly CurrencyValue Mvdol = new("Mvdol", "BOV", 984, 2);

        public static readonly CurrencyValue ConvertibleMark = new("Convertible Mark", "BAM", 977, 2);

        public static readonly CurrencyValue Pula = new("Pula", "BWP", 72, 2);

        public static readonly CurrencyValue NorwegianKrone = new("Norwegian Krone", "NOK", 578, 2);

        public static readonly CurrencyValue BrazilianReal = new("Brazilian Real", "BRL", 986, 2);

        public static readonly CurrencyValue BruneiDollar = new("Brunei Dollar", "BND", 96, 2);

        public static readonly CurrencyValue BulgarianLev = new("Bulgarian Lev", "BGN", 975, 2);

        public static readonly CurrencyValue BurundiFranc = new("Burundi Franc", "BIF", 108, 0);

        public static readonly CurrencyValue CaboVerdeEscudo = new("Cabo Verde Escudo", "CVE", 132, 2);

        public static readonly CurrencyValue Riel = new("Riel", "KHR", 116, 2);

        public static readonly CurrencyValue CFAFrancBEAC = new("CFA Franc BEAC", "XAF", 950, 0);

        public static readonly CurrencyValue CanadianDollar = new("Canadian Dollar", "CAD", 124, 2);

        public static readonly CurrencyValue CaymanIslandsDollar = new("Cayman Islands Dollar", "KYD", 136, 2);

        public static readonly CurrencyValue ChileanPeso = new("Chilean Peso", "CLP", 152, 0);

        public static readonly CurrencyValue UnidaddeFomento = new("Unidad de Fomento", "CLF", 990, 4);

        public static readonly CurrencyValue YuanRenminbi = new("Yuan Renminbi", "CNY", 156, 2);

        public static readonly CurrencyValue ColombianPeso = new("Colombian Peso", "COP", 170, 2);

        public static readonly CurrencyValue UnidaddeValorReal = new("Unidad de Valor Real", "COU", 970, 2);

        public static readonly CurrencyValue ComorianFranc = new("Comorian Franc ", "KMF", 174, 0);

        public static readonly CurrencyValue CongoleseFranc = new("Congolese Franc", "CDF", 976, 2);

        public static readonly CurrencyValue NewZealandDollar = new("New Zealand Dollar", "NZD", 554, 2);

        public static readonly CurrencyValue CostaRicanColon = new("Costa Rican Colon", "CRC", 188, 2);

        public static readonly CurrencyValue Kuna = new("Kuna", "HRK", 191, 2);

        public static readonly CurrencyValue CubanPeso = new("Cuban Peso", "CUP", 192, 2);

        public static readonly CurrencyValue PesoConvertible = new("Peso Convertible", "CUC", 931, 2);

        public static readonly CurrencyValue NetherlandsAntilleanGuilder = new("Netherlands Antillean Guilder", "ANG", 532, 2);

        public static readonly CurrencyValue CzechKoruna = new("Czech Koruna", "CZK", 203, 2);

        public static readonly CurrencyValue DanishKrone = new("Danish Krone", "DKK", 208, 2);

        public static readonly CurrencyValue DjiboutiFranc = new("Djibouti Franc", "DJF", 262, 0);

        public static readonly CurrencyValue DominicanPeso = new("Dominican Peso", "DOP", 214, 2);

        public static readonly CurrencyValue EgyptianPound = new("Egyptian Pound", "EGP", 818, 2);

        public static readonly CurrencyValue ElSalvadorColon = new("El Salvador Colon", "SVC", 222, 2);

        public static readonly CurrencyValue Nakfa = new("Nakfa", "ERN", 232, 2);

        public static readonly CurrencyValue Lilangeni = new("Lilangeni", "SZL", 748, 2);

        public static readonly CurrencyValue EthiopianBirr = new("Ethiopian Birr", "ETB", 230, 2);

        public static readonly CurrencyValue FalklandIslandsPound = new("Falkland Islands Pound", "FKP", 238, 2);

        public static readonly CurrencyValue FijiDollar = new("Fiji Dollar", "FJD", 242, 2);

        public static readonly CurrencyValue CFPFranc = new("CFP Franc", "XPF", 953, 0);

        public static readonly CurrencyValue Dalasi = new("Dalasi", "GMD", 270, 2);

        public static readonly CurrencyValue Lari = new("Lari", "GEL", 981, 2);

        public static readonly CurrencyValue GhanaCedi = new("Ghana Cedi", "GHS", 936, 2);

        public static readonly CurrencyValue GibraltarPound = new("Gibraltar Pound", "GIP", 292, 2);

        public static readonly CurrencyValue Quetzal = new("Quetzal", "GTQ", 320, 2);

        public static readonly CurrencyValue PoundSterling = new("Pound Sterling", "GBP", 826, 2);

        public static readonly CurrencyValue GuineanFranc = new("Guinean Franc", "GNF", 324, 0);

        public static readonly CurrencyValue GuyanaDollar = new("Guyana Dollar", "GYD", 328, 2);

        public static readonly CurrencyValue Gourde = new("Gourde", "HTG", 332, 2);

        public static readonly CurrencyValue Lempira = new("Lempira", "HNL", 340, 2);

        public static readonly CurrencyValue HongKongDollar = new("Hong Kong Dollar", "HKD", 344, 2);

        public static readonly CurrencyValue Forint = new("Forint", "HUF", 348, 2);

        public static readonly CurrencyValue IcelandKrona = new("Iceland Krona", "ISK", 352, 0);

        public static readonly CurrencyValue Rupiah = new("Rupiah", "IDR", 360, 2);

        public static readonly CurrencyValue IranianRial = new("Iranian Rial", "IRR", 364, 2);

        public static readonly CurrencyValue IraqiDinar = new("Iraqi Dinar", "IQD", 368, 3);

        public static readonly CurrencyValue NewIsraeliSheqel = new("New Israeli Sheqel", "ILS", 376, 2);

        public static readonly CurrencyValue JamaicanDollar = new("Jamaican Dollar", "JMD", 388, 2);

        public static readonly CurrencyValue Yen = new("Yen", "JPY", 392, 0);

        public static readonly CurrencyValue JordanianDinar = new("Jordanian Dinar", "JOD", 400, 3);

        public static readonly CurrencyValue Tenge = new("Tenge", "KZT", 398, 2);

        public static readonly CurrencyValue KenyanShilling = new("Kenyan Shilling", "KES", 404, 2);

        public static readonly CurrencyValue NorthKoreanWon = new("North Korean Won", "KPW", 408, 2);

        public static readonly CurrencyValue Won = new("Won", "KRW", 410, 0);

        public static readonly CurrencyValue KuwaitiDinar = new("Kuwaiti Dinar", "KWD", 414, 3);

        public static readonly CurrencyValue Som = new("Som", "KGS", 417, 2);

        public static readonly CurrencyValue LaoKip = new("Lao Kip", "LAK", 418, 2);

        public static readonly CurrencyValue LebanesePound = new("Lebanese Pound", "LBP", 422, 2);

        public static readonly CurrencyValue Loti = new("Loti", "LSL", 426, 2);

        public static readonly CurrencyValue Rand = new("Rand", "ZAR", 710, 2);

        public static readonly CurrencyValue LiberianDollar = new("Liberian Dollar", "LRD", 430, 2);

        public static readonly CurrencyValue LibyanDinar = new("Libyan Dinar", "LYD", 434, 3);

        public static readonly CurrencyValue SwissFranc = new("Swiss Franc", "CHF", 756, 2);

        public static readonly CurrencyValue Pataca = new("Pataca", "MOP", 446, 2);

        public static readonly CurrencyValue Denar = new("Denar", "MKD", 807, 2);

        public static readonly CurrencyValue MalagasyAriary = new("Malagasy Ariary", "MGA", 969, 2);

        public static readonly CurrencyValue MalawiKwacha = new("Malawi Kwacha", "MWK", 454, 2);

        public static readonly CurrencyValue MalaysianRinggit = new("Malaysian Ringgit", "MYR", 458, 2);

        public static readonly CurrencyValue Rufiyaa = new("Rufiyaa", "MVR", 462, 2);

        public static readonly CurrencyValue Ouguiya = new("Ouguiya", "MRU", 929, 2);

        public static readonly CurrencyValue MauritiusRupee = new("Mauritius Rupee", "MUR", 480, 2);

        public static readonly CurrencyValue MexicanPeso = new("Mexican Peso", "MXN", 484, 2);

        public static readonly CurrencyValue MoldovanLeu = new("Moldovan Leu", "MDL", 498, 2);

        public static readonly CurrencyValue Tugrik = new("Tugrik", "MNT", 496, 2);

        public static readonly CurrencyValue MoroccanDirham = new("Moroccan Dirham", "MAD", 504, 2);

        public static readonly CurrencyValue MozambiqueMetical = new("Mozambique Metical", "MZN", 943, 2);

        public static readonly CurrencyValue Kyat = new("Kyat", "MMK", 104, 2);

        public static readonly CurrencyValue NamibiaDollar = new("Namibia Dollar", "NAD", 516, 2);

        public static readonly CurrencyValue NepaleseRupee = new("Nepalese Rupee", "NPR", 524, 2);

        public static readonly CurrencyValue CordobaOro = new("Cordoba Oro", "NIO", 558, 2);

        public static readonly CurrencyValue Naira = new("Naira", "NGN", 566, 2);

        public static readonly CurrencyValue RialOmani = new("Rial Omani", "OMR", 512, 3);

        public static readonly CurrencyValue PakistanRupee = new("Pakistan Rupee", "PKR", 586, 2);

        public static readonly CurrencyValue Balboa = new("Balboa", "PAB", 590, 2);

        public static readonly CurrencyValue Kina = new("Kina", "PGK", 598, 2);

        public static readonly CurrencyValue Guarani = new("Guarani", "PYG", 600, 0);

        public static readonly CurrencyValue Sol = new("Sol", "PEN", 604, 2);

        public static readonly CurrencyValue PhilippinePeso = new("Philippine Peso", "PHP", 608, 2);

        public static readonly CurrencyValue Zloty = new("Zloty", "PLN", 985, 2);

        public static readonly CurrencyValue QatariRial = new("Qatari Rial", "QAR", 634, 2);

        public static readonly CurrencyValue RomanianLeu = new("Romanian Leu", "RON", 946, 2);

        public static readonly CurrencyValue RussianRuble = new("Russian Ruble", "RUB", 643, 2);

        public static readonly CurrencyValue RwandaFranc = new("Rwanda Franc", "RWF", 646, 0);

        public static readonly CurrencyValue SaintHelenaPound = new("Saint Helena Pound", "SHP", 654, 2);

        public static readonly CurrencyValue Tala = new("Tala", "WST", 882, 2);

        public static readonly CurrencyValue Dobra = new("Dobra", "STN", 930, 2);

        public static readonly CurrencyValue SaudiRiyal = new("Saudi Riyal", "SAR", 682, 2);

        public static readonly CurrencyValue SerbianDinar = new("Serbian Dinar", "RSD", 941, 2);

        public static readonly CurrencyValue SeychellesRupee = new("Seychelles Rupee", "SCR", 690, 2);

        public static readonly CurrencyValue Leone = new("Leone", "SLL", 694, 2);

        public static readonly CurrencyValue SingaporeDollar = new("Singapore Dollar", "SGD", 702, 2);

        public static readonly CurrencyValue SolomonIslandsDollar = new("Solomon Islands Dollar", "SBD", 90, 2);

        public static readonly CurrencyValue SomaliShilling = new("Somali Shilling", "SOS", 706, 2);

        public static readonly CurrencyValue SouthSudanesePound = new("South Sudanese Pound", "SSP", 728, 2);

        public static readonly CurrencyValue SriLankaRupee = new("Sri Lanka Rupee", "LKR", 144, 2);

        public static readonly CurrencyValue SudanesePound = new("Sudanese Pound", "SDG", 938, 2);

        public static readonly CurrencyValue SurinamDollar = new("Surinam Dollar", "SRD", 968, 2);

        public static readonly CurrencyValue SwedishKrona = new("Swedish Krona", "SEK", 752, 2);

        public static readonly CurrencyValue WIREuro = new("WIR Euro", "CHE", 947, 2);

        public static readonly CurrencyValue WIRFranc = new("WIR Franc", "CHW", 948, 2);

        public static readonly CurrencyValue SyrianPound = new("Syrian Pound", "SYP", 760, 2);

        public static readonly CurrencyValue NewTaiwanDollar = new("New Taiwan Dollar", "TWD", 901, 2);

        public static readonly CurrencyValue Somoni = new("Somoni", "TJS", 972, 2);

        public static readonly CurrencyValue TanzanianShilling = new("Tanzanian Shilling", "TZS", 834, 2);

        public static readonly CurrencyValue Baht = new("Baht", "THB", 764, 2);

        public static readonly CurrencyValue Pa_anga = new("Pa’anga", "TOP", 776, 2);

        public static readonly CurrencyValue TrinidadandTobagoDollar = new("Trinidad and Tobago Dollar", "TTD", 780, 2);

        public static readonly CurrencyValue TunisianDinar = new("Tunisian Dinar", "TND", 788, 3);

        public static readonly CurrencyValue TurkishLira = new("Turkish Lira", "TRY", 949, 2);

        public static readonly CurrencyValue TurkmenistanNewManat = new("Turkmenistan New Manat", "TMT", 934, 2);

        public static readonly CurrencyValue UgandaShilling = new("Uganda Shilling", "UGX", 800, 0);

        public static readonly CurrencyValue Hryvnia = new("Hryvnia", "UAH", 980, 2);

        public static readonly CurrencyValue UAEDirham = new("UAE Dirham", "AED", 784, 2);

        public static readonly CurrencyValue PesoUruguayo = new("Peso Uruguayo", "UYU", 858, 2);

        public static readonly CurrencyValue UnidadPrevisional = new("Unidad Previsional", "UYW", 927, 4);

        public static readonly CurrencyValue UzbekistanSum = new("Uzbekistan Sum", "UZS", 860, 2);

        public static readonly CurrencyValue Vatu = new("Vatu", "VUV", 548, 0);

        public static readonly CurrencyValue BolivarSoberano = new("Bolívar Soberano", "VES", 928, 2);

        public static readonly CurrencyValue Dong = new("Dong", "VND", 704, 0);

        public static readonly CurrencyValue YemeniRial = new("Yemeni Rial", "YER", 886, 2);

        public static readonly CurrencyValue ZambianKwacha = new("Zambian Kwacha", "ZMW", 967, 2);

        public static readonly CurrencyValue ZimbabweDollar = new("Zimbabwe Dollar", "ZWL", 932, 2);
    }
}
