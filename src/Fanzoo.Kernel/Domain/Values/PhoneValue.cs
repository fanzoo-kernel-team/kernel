﻿namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class PhoneValue : RequiredStringValue
    {
        private PhoneValue() { } //ORM

        public PhoneValue(string value) : base(value)
        {
            //sanitize
            var digits = value
                .ToDigits()
                    .TrimStart('1');

            Guard.Against.InvalidPhoneNumber(digits, nameof(value));

        }

        public static ValueResult<PhoneValue, Error> Create(string phone) => CanCreate(phone)
                ? new PhoneValue(GetDigits(phone))
                : Errors.ValueObjects.PhoneValue.InvalidFormat;

        public override string ToString() => Value.Format("{0:(###) ###-####}");

        public static implicit operator PhoneValue(string value) => new(value);

        public static bool CanCreate(string phone) => Check.For.IsValidPhoneFormat(GetDigits(phone));

        private static string GetDigits(string phone) => phone
                .ToDigits()
                    .TrimStart('1');
    }
}
