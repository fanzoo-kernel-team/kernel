﻿namespace Fanzoo.Kernel.Defaults.Domain.Values
{
    public sealed class MaxStringValue(string value) : StringValue(value)
    {
        public static ValueResult<MaxStringValue, Error> Create(string value)
        {
            var isValid = Check.For.NotNull(value);

            return isValid ? new MaxStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator MaxStringValue(string value) => new(value);
    }

    public sealed class MaxRequiredStringValue(string value) : RequiredStringValue(value)
    {
        public static ValueResult<MaxRequiredStringValue, Error> Create(string value)
        {
            var isValid = Check.For.NotNullOrWhiteSpace(value);

            return isValid ? new MaxRequiredStringValue(value) : Errors.ValueObjects.StringValue.InvalidFormat;
        }

        public static implicit operator MaxRequiredStringValue(string value) => new(value);
    }
}
