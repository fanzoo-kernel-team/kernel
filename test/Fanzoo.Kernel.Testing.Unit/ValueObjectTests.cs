using Fanzoo.Kernel.Defaults.Domain.Values;
using Fanzoo.Kernel.Domain.Values;
using Xunit;

namespace Fanzoo.Kernel.Tests
{

    public class ValueObjectTests
    {
        [Fact]
        public void Test_EmailValue()
        {
            var blankEmail = " ";

            var validEmail = "test@test.com";

            var inValidEmail = "test@@test.com";

            var tooLongEmail = "test@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.com";

            var email = EmailValue.Create(validEmail).Value;

            Assert.True(email.Value == validEmail);

            Assert.True(EmailValue.Create(inValidEmail).IsFailure);

            Assert.True(EmailValue.Create(tooLongEmail).IsFailure);

            Assert.True(EmailValue.CanCreate(email));

            Assert.False(EmailValue.CanCreate(blankEmail));

            Assert.False(EmailValue.CanCreate(tooLongEmail));

            Assert.False(EmailValue.CanCreate(inValidEmail));

            Assert.Throws<ArgumentException>(() => new EmailValue(inValidEmail));

            Assert.Throws<ArgumentException>(() => new EmailValue(tooLongEmail));

            Assert.NotNull(new EmailValue(validEmail));
        }

        [Fact]
        public void Test_EmailUsernameValue()
        {
            var blankEmail = " ";

            var validEmail = "test@test.com";

            var inValidEmail = "test@@test.com";

            var tooLongEmail = "test@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.com";

            var email = EmailUsernameValue.Create(validEmail).Value;

            Assert.True(email.Value == validEmail);

            Assert.True(EmailUsernameValue.Create(inValidEmail).IsFailure);

            Assert.True(EmailUsernameValue.Create(tooLongEmail).IsFailure);

            Assert.True(EmailUsernameValue.CanCreate(email));

            Assert.False(EmailUsernameValue.CanCreate(blankEmail));

            Assert.False(EmailUsernameValue.CanCreate(tooLongEmail));

            Assert.False(EmailUsernameValue.CanCreate(inValidEmail));

            Assert.Throws<ArgumentException>(() => new EmailUsernameValue(inValidEmail));

            Assert.Throws<ArgumentException>(() => new EmailUsernameValue(tooLongEmail));

            Assert.NotNull(new EmailUsernameValue(validEmail));
        }

        [Fact]
        public void Test_HashedPasswordvalue()
        {
            var validBase64string = "cGFzc3dvcmQ=";

            var invalidbase64string = "@!#$%^&*()";

            var base64string = HashedPasswordValue.Create(validBase64string).Value;

            Assert.True(base64string.Value == validBase64string);

            Assert.True(HashedPasswordValue.Create(invalidbase64string).IsFailure);

            Assert.Throws<ArgumentException>(() => new HashedPasswordValue(invalidbase64string));

            Assert.NotNull(new HashedPasswordValue(validBase64string));


        }

        [Fact]
        public void Test_MoneyValue()
        {
            var validMoneyValueAmount = 12.55m;

            var invalidMoneyValueDecimal = 12.5555555m;

            var invalidMoneyValueAmount = -1.25m;

            var moneyValueAmount = MoneyValue.Create(validMoneyValueAmount, CurrencyValue.USDollar).Value;

            Assert.True(moneyValueAmount.Amount == validMoneyValueAmount);

            Assert.True(MoneyValue.Create(invalidMoneyValueAmount, CurrencyValue.USDollar).IsFailure);

            Assert.True(MoneyValue.Create(invalidMoneyValueDecimal, CurrencyValue.USDollar).IsFailure);

            Assert.True(MoneyValue.CanCreate(validMoneyValueAmount, CurrencyValue.USDollar));

            Assert.False(MoneyValue.CanCreate(invalidMoneyValueDecimal, CurrencyValue.USDollar));

            Assert.False(MoneyValue.CanCreate(invalidMoneyValueAmount, CurrencyValue.USDollar));

            Assert.Throws<ArgumentOutOfRangeException>(() => new MoneyValue(invalidMoneyValueAmount, CurrencyValue.USDollar));

            Assert.Throws<ArgumentOutOfRangeException>(() => new MoneyValue(invalidMoneyValueDecimal, CurrencyValue.USDollar));

            Assert.NotNull(new MoneyValue(validMoneyValueAmount, CurrencyValue.USDollar));

            Assert.Throws<KernelErrorException>(() => new MoneyValue(1, CurrencyValue.USDollar) + new MoneyValue(1, CurrencyValue.SwissFranc));

            var a = new MoneyValue(2, CurrencyValue.USDollar);
            var b = new MoneyValue(5, CurrencyValue.USDollar);

            var x = a + b;
            var y = b - a;

            Assert.True(x == 7);
            Assert.True(y == 3);

        }

        [Fact]
        public void Test_NameValue()
        {
            var validFirstName = "Name";

            var validLastName = "Namington";

            var nameValue = new NameValue(validFirstName, validLastName);

            var invalidName = "";

            var name = NameValue.Create(validFirstName, validLastName);

            Assert.True(name.Value == nameValue);

            Assert.True(NameValue.Create(invalidName, validLastName).IsFailure);

            Assert.True(NameValue.Create(validFirstName, invalidName).IsFailure);

            Assert.True(NameValue.CanCreate(validFirstName, validLastName));

            Assert.False(NameValue.CanCreate(invalidName, validLastName));

            Assert.False(NameValue.CanCreate(validFirstName, invalidName));

            Assert.False(NameValue.CanCreate(invalidName, invalidName));

            Assert.Throws<ArgumentNullException>(() => new NameValue(invalidName, validLastName));

            Assert.Throws<ArgumentNullException>(() => new NameValue(validFirstName, invalidName));

            Assert.NotNull(new NameValue(validFirstName, validLastName));

        }

        [Fact]
        public void Test_AddressValue()
        {
            var validPrimaryAddress = "123 street str";

            var invalidPrimaryAddress = "";

            var invalidCity = "";

            var validCity = "City";

            var validPostalCode = "12345"
;
            var testingRegion = RegionValue.Create("TN");

            var testingPostalCode = USPostalCodeValue.Create("11111-1111");

            var testingSecondaryAddressBlank = "";

            var validAddressValue = new AddressValue(validPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value);

            var address = AddressValue.Create(validPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value);

            Assert.True(validAddressValue == address.Value);

            Assert.True(AddressValue.Create(invalidPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value).IsFailure);

            Assert.True(AddressValue.Create(validPrimaryAddress, testingSecondaryAddressBlank, invalidCity, testingRegion.Value, testingPostalCode.Value).IsFailure);

            Assert.True(AddressValue.CanCreate(validPrimaryAddress, validCity, validPostalCode));

            Assert.True(AddressValue.CanCreate(validPrimaryAddress, validCity, testingPostalCode.Value));

            Assert.Throws<ArgumentNullException>(() => new AddressValue(invalidPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value));

            Assert.Throws<ArgumentNullException>(() => new AddressValue(validPrimaryAddress, testingSecondaryAddressBlank, invalidCity, testingRegion.Value, testingPostalCode.Value));

            Assert.NotNull(new AddressValue(validPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value));
        }

        [Fact]
        public void Test_PhoneValue()
        {
            var validPhoneInput = "1(615) 555-5555";

            var sanitizedValidPhone = validPhoneInput
                .ToDigits()
                    .TrimStart('1');


            var invalidPhone = "76542 7654-9878675";

            var phone = PhoneValue.Create(validPhoneInput);

            Assert.False(PhoneValue.CanCreate("333.333.33"));

            Assert.True(PhoneValue.CanCreate(validPhoneInput));

            Assert.True(phone.Value == sanitizedValidPhone);

            Assert.True(PhoneValue.Create(invalidPhone).IsFailure);

            Assert.Throws<ArgumentException>(() => new PhoneValue(invalidPhone));

            Assert.NotNull(new PhoneValue(validPhoneInput));
        }

        [Fact]
        public void Test_IPAddressValue()
        {
            var validIPAddress = "192.168.1.15";

            var invalidIPAddress = "1925.168.1.15";

            var emptyIPAddress = "";

            var ipAddress = IPAddressValue.Create(validIPAddress);

            Assert.True(ipAddress.Value == validIPAddress);

            Assert.True(IPAddressValue.Create(invalidIPAddress).IsFailure);

            Assert.True(IPAddressValue.Create(emptyIPAddress).IsFailure);

            Assert.True(IPAddressValue.CanCreate(validIPAddress));

            Assert.False(IPAddressValue.CanCreate(invalidIPAddress));

            Assert.False(IPAddressValue.CanCreate(emptyIPAddress));

            Assert.Throws<ArgumentException>(() => new IPAddressValue(invalidIPAddress));

            Assert.Throws<ArgumentNullException>(() => new IPAddressValue(emptyIPAddress));

            Assert.NotNull(new IPAddressValue(validIPAddress));
        }

        [Fact]
        public void Test_PostalCodevalue()
        {
            var validPostalCode = "11111-1111";

            var invalidPostalCode = "111111-1111";

            var emptyPostalCode = "";

            var postalCode = USPostalCodeValue.Create(validPostalCode);

            Assert.True(postalCode.Value == validPostalCode);

            Assert.True(USPostalCodeValue.Create(invalidPostalCode).IsFailure);

            Assert.True(USPostalCodeValue.Create(emptyPostalCode).IsFailure);

            Assert.Throws<ArgumentException>(() => new USPostalCodeValue(invalidPostalCode));

            Assert.Throws<ArgumentNullException>(() => new USPostalCodeValue(emptyPostalCode));

            Assert.NotNull(new USPostalCodeValue(validPostalCode));
        }

        [Fact]
        public void Test_RegionValue()
        {
            var validRegion = "MI";
            var invalidRegion = "BB";
            var emptyRegion = "";

            Assert.True(RegionValue.CanCreate(validRegion));

            var region = RegionValue.Create(validRegion);

            Assert.True(region.Value == validRegion);

            Assert.True(RegionValue.Create(invalidRegion).IsFailure);

            Assert.True(RegionValue.Create(emptyRegion).IsFailure);

            Assert.Throws<ArgumentOutOfRangeException>(() => new RegionValue(invalidRegion));

            Assert.Throws<ArgumentNullException>(() => new RegionValue(emptyRegion));

            Assert.NotNull(new RegionValue(validRegion));
        }

        [Fact]
        public void Test_RefreshTokenValue()
        {
            var validBase64string = "cGFzc3dvcmQ=";

            var invalidbase64string = "@!#$%^&*()";

            var emptyBase64String = "";

            var base64String = RefreshTokenValue.Create(validBase64string).Value;

            Assert.True(RefreshTokenValue.CanCreate(validBase64string));

            Assert.True(base64String.Value == validBase64string);

            Assert.True(RefreshTokenValue.Create(invalidbase64string).IsFailure);

            Assert.True(RefreshTokenValue.Create(emptyBase64String).IsFailure);

            Assert.Throws<ArgumentException>(() => new RefreshTokenValue(invalidbase64string));

            Assert.Throws<ArgumentNullException>(() => new RefreshTokenValue(emptyBase64String));

            Assert.NotNull(new RefreshTokenValue(validBase64string));
        }

        [Fact]
        public void Test_MaxString_Value()
        {
            var test = "some test string";

            Assert.True(MaxStringValue.Create(test).IsSuccessful);

            Assert.True(MaxStringValue.Create(test).Value == test);

            MaxStringValue maxString = test;

            Assert.True(maxString == test);

            string testString = maxString;

            Assert.True(testString == test);

            Assert.True(maxString != "blah");
        }

        [Fact]
        public void Test_MaxRequiredString_Value()
        {
            var test = "some test string";

            Assert.True(MaxRequiredStringValue.Create(test).IsSuccessful);

            Assert.True(MaxRequiredStringValue.Create(test).Value == test);

            MaxRequiredStringValue maxString = test;

            Assert.True(maxString == test);

            string testString = maxString;

            Assert.True(testString == test);

            Assert.True(maxString != "blah");

            Assert.Throws<ArgumentNullException>(() => new MaxRequiredStringValue(""));

            Assert.True(MaxRequiredStringValue.Create("").IsFailure);
        }

        [Fact]
        public void Test_DefaultString_Value()
        {
            var test = "some test string";

            Assert.True(DefaultStringValue.Create(test).IsSuccessful);

            Assert.True(DefaultStringValue.Create(test).Value == test);

            DefaultStringValue DefaultString = test;

            Assert.True(DefaultString == test);

            string testString = DefaultString;

            Assert.True(testString == test);

            Assert.True(DefaultString != "blah");

            var tooLong = new string('a', DatabaseCatalog.FieldLength.Default + 1);

            Assert.True(DefaultStringValue.Create(tooLong).IsFailure);
        }

        [Fact]
        public void Test_DefaultRequiredString_Value()
        {
            var test = "some test string";

            Assert.True(DefaultRequiredStringValue.Create(test).IsSuccessful);

            Assert.True(DefaultRequiredStringValue.Create(test).Value == test);

            DefaultRequiredStringValue DefaultString = test;

            Assert.True(DefaultString == test);

            string testString = DefaultString;

            Assert.True(testString == test);

            Assert.True(DefaultString != "blah");

            Assert.Throws<ArgumentNullException>(() => new DefaultRequiredStringValue(""));

            Assert.True(DefaultRequiredStringValue.Create("").IsFailure);

            var tooLong = new string('a', DatabaseCatalog.FieldLength.Default + 1);

            Assert.True(DefaultRequiredStringValue.Create(tooLong).IsFailure);
        }

        [Fact]
        public void Test_ShortString_Value()
        {
            var test = "some test string";

            Assert.True(ShortStringValue.Create(test).IsSuccessful);

            Assert.True(ShortStringValue.Create(test).Value == test);

            ShortStringValue ShortString = test;

            Assert.True(ShortString == test);

            string testString = ShortString;

            Assert.True(testString == test);

            Assert.True(ShortString != "blah");

            var tooLong = new string('a', DatabaseCatalog.FieldLength.Short + 1);

            Assert.True(ShortStringValue.Create(tooLong).IsFailure);
        }

        [Fact]
        public void Test_ShortRequiredString_Value()
        {
            var test = "some test string";

            Assert.True(ShortRequiredStringValue.Create(test).IsSuccessful);

            Assert.True(ShortRequiredStringValue.Create(test).Value == test);

            ShortRequiredStringValue ShortString = test;

            Assert.True(ShortString == test);

            string testString = ShortString;

            Assert.True(testString == test);

            Assert.True(ShortString != "blah");

            Assert.Throws<ArgumentNullException>(() => new ShortRequiredStringValue(""));

            Assert.True(ShortRequiredStringValue.Create("").IsFailure);

            var tooLong = new string('a', DatabaseCatalog.FieldLength.Short + 1);

            Assert.True(ShortRequiredStringValue.Create(tooLong).IsFailure);
        }

        [Fact]
        public void Test_LongString_Value()
        {
            var test = "some test string";

            Assert.True(LongStringValue.Create(test).IsSuccessful);

            Assert.True(LongStringValue.Create(test).Value == test);

            LongStringValue LongString = test;

            Assert.True(LongString == test);

            string testString = LongString;

            Assert.True(testString == test);

            Assert.True(LongString != "blah");

            var tooLong = new string('a', DatabaseCatalog.FieldLength.Long + 1);

            Assert.True(LongStringValue.Create(tooLong).IsFailure);
        }

        [Fact]
        public void Test_LongRequiredString_Value()
        {
            var test = "some test string";

            Assert.True(LongRequiredStringValue.Create(test).IsSuccessful);

            Assert.True(LongRequiredStringValue.Create(test).Value == test);

            LongRequiredStringValue LongString = test;

            Assert.True(LongString == test);

            string testString = LongString;

            Assert.True(testString == test);

            Assert.True(LongString != "blah");

            Assert.Throws<ArgumentNullException>(() => new LongRequiredStringValue(""));

            Assert.True(LongRequiredStringValue.Create("").IsFailure);

            var tooLong = new string('a', DatabaseCatalog.FieldLength.Long + 1);

            Assert.True(LongRequiredStringValue.Create(tooLong).IsFailure);
        }

        [Fact]
        public void Test_Url_Value()
        {
            var valid = "https://www.microsoft.com";

            Assert.True(UrlValue.CanCreate(valid));

            Assert.True(UrlValue.Create(valid).IsSuccessful);

            Assert.True(UrlValue.Create(valid).Value == valid);

            var tooLong = valid + "/" + new string('a', 3000);

            Assert.True(UrlValue.Create(tooLong).IsFailure);

            Assert.True(new UrlValue(valid) == valid);

            UrlValue urlValue = valid;

            Assert.True(urlValue == valid);
        }

        [Fact]
        public void Test_CssColorValue()
        {
            var validName = "green";
            var invalidName = "supergreen";

            var validHex1 = "#ffffff";
            var validHex2 = "#aa0";
            var invalidHex1 = "fff";
            var invalidHex2 = "#fffffff";
            var invalidHex3 = "#ffgg";

            Assert.True(CssColorValue.Create(validName).IsSuccessful);
            Assert.True(CssColorValue.Create(invalidName).IsFailure);

            Assert.True(CssColorValue.Create(validHex1).IsSuccessful);
            Assert.True(CssColorValue.Create(validHex2).IsSuccessful);
            Assert.True(CssColorValue.Create(invalidHex1).IsFailure);
            Assert.True(CssColorValue.Create(invalidHex2).IsFailure);
            Assert.True(CssColorValue.Create(invalidHex3).IsFailure);
        }

        [Fact]
        public void Test_PasswordValue()
        {
            var validPasswords = new[]
            {
                "aaF!1rfhti",
                "AABBCCd%22",
                "djelkjasdlkd!1"
            };

            var invalidPasswords = new[]
            {
                "aaaF1!uewbncjd",
                "ABCDEFGHIJKLMNOP",
                "Fg!1a",
                "aaF!1rfhtiaaF!1rfhtiaaF!1rfhtiaaF!1rfhtiaaF!1rfhtiaaF!1rfhtiaaF!1rfhtiaaF!1rfhtiaaF!1rfhtiaaF!1rfhtit"
            };

            foreach (var validPassword in validPasswords)
            {
                Assert.True(PasswordValue.Create(validPassword).IsSuccessful);
                Assert.True(PasswordValue.CanCreate(validPassword));
            }

            foreach (var invalidPassword in invalidPasswords)
            {
                Assert.True(PasswordValue.Create(invalidPassword).IsFailure);
                Assert.False(PasswordValue.CanCreate(invalidPassword));
            }
        }

        //[Fact]
        //public void Test_StripeCustomerIdentifierValue()
        //{
        //    var validStripeIdentifier = "cus_123456789";

        //    var invalidStripeIdentifier = "123456789";

        //    var emptyStripeIdentifier = "";

        //    var stripeIdentifier = StripeCustomerIdentifierValue.Create(validStripeIdentifier).Value;

        //    Assert.True(stripeIdentifier.Value == validStripeIdentifier);

        //    Assert.True(StripeCustomerIdentifierValue.Create(invalidStripeIdentifier).IsFailure);

        //    Assert.True(StripeCustomerIdentifierValue.Create(emptyStripeIdentifier).IsFailure);

        //    Assert.Throws<ArgumentNullException>(() => new StripeCustomerIdentifierValue(invalidStripeIdentifier));

        //    Assert.Throws<ArgumentNullException>(() => new StripeCustomerIdentifierValue(emptyStripeIdentifier));

        //    Assert.NotNull(new StripeCustomerIdentifierValue(validStripeIdentifier));

        //}

        //[Fact]
        //public void Test_StripePaymentIntentIdentifierValue()
        //{
        //    var validStripeIdentifier = "pi_123456789";

        //    var invalidStripeIdentifier = "123456789";

        //    var emptyStripeIdentifier = "";

        //    var stripeIdentifier = StripePaymentIntentIdentifierValue.Create(validStripeIdentifier).Value;

        //    Assert.True(stripeIdentifier.Value == validStripeIdentifier);

        //    Assert.True(StripePaymentIntentIdentifierValue.Create(invalidStripeIdentifier).IsFailure);

        //    Assert.True(StripePaymentIntentIdentifierValue.Create(emptyStripeIdentifier).IsFailure);

        //    Assert.Throws<ArgumentNullException>(() => new StripePaymentIntentIdentifierValue(invalidStripeIdentifier));

        //    Assert.Throws<ArgumentNullException>(() => new StripePaymentIntentIdentifierValue(emptyStripeIdentifier));

        //    Assert.NotNull(new StripePaymentIntentIdentifierValue(validStripeIdentifier));

        //}
    }
}
