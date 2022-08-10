﻿using System;
using Fanzoo.Kernel.Domain.Values;
using Fanzoo.Kernel.Domain.Values.Stripe;
using Xunit;

namespace Fanzoo.Kernel.Tests
{

    public class ValueObjectTests
    {
        [Fact]
        public void Test_EmailValue()
        {
            var validEmail = "test@test.com";

            var inValidEmail = "test@@test.com";

            var tooLongEmail = "test@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.com";

            var email = EmailValue.Create(validEmail).Value;

            Assert.True(email.Value == validEmail);

            Assert.True(EmailValue.Create(inValidEmail).IsFailure);

            Assert.True(EmailValue.Create(tooLongEmail).IsFailure);

            Assert.Throws<ArgumentException>(() => new EmailValue(inValidEmail));

            Assert.Throws<ArgumentException>(() => new EmailValue(tooLongEmail));

            Assert.NotNull(new EmailValue(validEmail));
        }

        [Fact]
        public void Test_EmailUsernameValue()
        {
            var validEmail = "test@test.com";

            var inValidEmail = "test@@test.com";

            var tooLongEmail = "test@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.com";

            var email = EmailUsernameValue.Create(validEmail).Value;

            Assert.True(email.Value == validEmail);

            Assert.True(EmailUsernameValue.Create(inValidEmail).IsFailure);

            Assert.True(EmailUsernameValue.Create(tooLongEmail).IsFailure);

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

            Assert.Throws<ArgumentOutOfRangeException>(() => new MoneyValue(invalidMoneyValueAmount, CurrencyValue.USDollar));

            Assert.Throws<ArgumentOutOfRangeException>(() => new MoneyValue(invalidMoneyValueDecimal, CurrencyValue.USDollar));

            Assert.NotNull(new MoneyValue(validMoneyValueAmount, CurrencyValue.USDollar));
        }

        [Fact]
        public void Test_NameValue()
        {
            var validFirstName = "Name";

            var validLastName = "Namington";

            var nameValue = new NameValue(validFirstName, validLastName);

            var invalidName = "";

            var name = NameValue.Create(validFirstName,validLastName);

            Assert.True(name.Value == nameValue);

            Assert.True(NameValue.Create(invalidName, validLastName).IsFailure);

            Assert.True(NameValue.Create(validFirstName, invalidName).IsFailure);

            Assert.Throws<ArgumentException>(() => new NameValue(invalidName, validLastName));

            Assert.Throws<ArgumentException>(() => new NameValue(validFirstName, invalidName));

            Assert.NotNull(new NameValue(validFirstName, validLastName));
            
        }

        [Fact]
        public void Test_AddressValue()
        {
            var validPrimaryAddress = "123 street str";

            var invalidPrimaryAddress = "";

            var invalidCity = "";

            var validCity = "City";

            var testingRegion = RegionValue.Create("TN");

            var testingPostalCode = PostalCodeValue.Create("11111-1111");

            var testingSecondaryAddressBlank = "";

            var validAddressValue = new AddressValue(validPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value);

            var address = AddressValue.Create(validPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value);

            Assert.True(validAddressValue == address.Value);

            Assert.True(AddressValue.Create(invalidPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value).IsFailure);

            Assert.True(AddressValue.Create(validPrimaryAddress, testingSecondaryAddressBlank, invalidCity, testingRegion.Value, testingPostalCode.Value).IsFailure);

            Assert.Throws<ArgumentException>(() => new AddressValue(invalidPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value));

            Assert.Throws<ArgumentException>(() => new AddressValue(validPrimaryAddress, testingSecondaryAddressBlank, invalidCity, testingRegion.Value, testingPostalCode.Value));

            Assert.NotNull(new AddressValue(validPrimaryAddress, testingSecondaryAddressBlank, validCity, testingRegion.Value, testingPostalCode.Value));
        }

        [Fact]
        public void Test_PasswordValue()
        {
            var validPassword = "PasswordPassword";

            var invalidPasswordShort = "meh";

            var invalidPasswordLong = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

            var password = PasswordValue.Create(validPassword);

            Assert.True(password.Value == validPassword);
            
            Assert.True(PasswordValue.Create(invalidPasswordShort).IsFailure);

            Assert.True(PasswordValue.Create(invalidPasswordLong).IsFailure);

            Assert.Throws<ArgumentOutOfRangeException>(() => new PasswordValue(invalidPasswordLong));

            Assert.Throws<ArgumentOutOfRangeException>(() => new PasswordValue(invalidPasswordShort));
            
            Assert.NotNull(new PasswordValue(validPassword));
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

            Assert.Throws<ArgumentException>(() => new IPAddressValue(invalidIPAddress));

            Assert.Throws<ArgumentException>(() => new IPAddressValue(emptyIPAddress));

            Assert.NotNull(new IPAddressValue(validIPAddress));
        }

        [Fact]
        public void Test_PostalCodevalue()
        {
            var validPostalCode = "11111-1111";

            var invalidPostalCode = "111111-1111";

            var emptyPostalCode = "";

            var postalCode = PostalCodeValue.Create(validPostalCode);

            Assert.True(postalCode.Value == validPostalCode);

            Assert.True(PostalCodeValue.Create(invalidPostalCode).IsFailure);

            Assert.True(PostalCodeValue.Create(emptyPostalCode).IsFailure);

            Assert.Throws<ArgumentException>(() => new PostalCodeValue(invalidPostalCode));

            Assert.Throws<ArgumentException>(() => new PostalCodeValue(emptyPostalCode));

            Assert.NotNull(new PostalCodeValue(validPostalCode));
        }

        [Fact]
        public void Test_RefreshTokenValue()
        {
            var validBase64string = "cGFzc3dvcmQ=";

            var invalidbase64string = "@!#$%^&*()";

            var emptyBase64String = "";

            var base64String = RefreshTokenValue.Create(validBase64string).Value;

            Assert.True(base64String.Value == validBase64string);

            Assert.True(RefreshTokenValue.Create(invalidbase64string).IsFailure);

            Assert.True(RefreshTokenValue.Create(emptyBase64String).IsFailure);

            Assert.Throws<ArgumentException>(() => new RefreshTokenValue(invalidbase64string));

            Assert.Throws<ArgumentException>(() => new RefreshTokenValue(emptyBase64String));

            Assert.NotNull(new RefreshTokenValue(validBase64string));
        }

        [Fact]
        public void Test_StripeCustomerIdentifierValue()
        {
            var validStripeIdentifier = "cus_123456789";

            var invalidStripeIdentifier = "123456789";

            var emptyStripeIdentifier = "";

            var stripeIdentifier = StripeCustomerIdentifierValue.Create(validStripeIdentifier).Value;

            Assert.True(stripeIdentifier.Value == validStripeIdentifier);

            Assert.True(StripeCustomerIdentifierValue.Create(invalidStripeIdentifier).IsFailure);

            Assert.True(StripeCustomerIdentifierValue.Create(emptyStripeIdentifier).IsFailure);

            Assert.Throws<ArgumentException>(() => new StripeCustomerIdentifierValue(invalidStripeIdentifier));

            Assert.Throws<ArgumentException>(() => new StripeCustomerIdentifierValue(emptyStripeIdentifier));

            Assert.NotNull(new StripeCustomerIdentifierValue(validStripeIdentifier));

        }

        [Fact]
        public void Test_StripePaymentIntentIdentifierValue()
        {
            var validStripeIdentifier = "pi_123456789";

            var invalidStripeIdentifier = "123456789";

            var emptyStripeIdentifier = "";

            var stripeIdentifier = StripePaymentIntentIdentifierValue.Create(validStripeIdentifier).Value;

            Assert.True(stripeIdentifier.Value == validStripeIdentifier);

            Assert.True(StripePaymentIntentIdentifierValue.Create(invalidStripeIdentifier).IsFailure);

            Assert.True(StripePaymentIntentIdentifierValue.Create(emptyStripeIdentifier).IsFailure);

            Assert.Throws<ArgumentException>(() => new StripePaymentIntentIdentifierValue(invalidStripeIdentifier));

            Assert.Throws<ArgumentException>(() => new StripePaymentIntentIdentifierValue(emptyStripeIdentifier));

            Assert.NotNull(new StripePaymentIntentIdentifierValue(validStripeIdentifier));

        }
    }
}