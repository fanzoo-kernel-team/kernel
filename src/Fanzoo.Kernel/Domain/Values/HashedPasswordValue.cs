﻿namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class HashedPasswordValue : StringValue
    {
        private HashedPasswordValue() { } //ORM

        public HashedPasswordValue(string value) : base(value)
        {
            Guard.Against.InvalidBase64String(value, nameof(value));
        }

        public static Result<HashedPasswordValue, Error> Create(string hashedPassword) =>
            Check.For.Base64String(hashedPassword).IsValid
                ? new HashedPasswordValue(hashedPassword)
                : Errors.ValueObjects.HashedPasswordValue.InvalidFormat;
    }
}