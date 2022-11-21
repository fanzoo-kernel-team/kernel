#pragma warning disable IDE0046 // Convert to conditional expression


namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class MoneyValue : ValueObject
    {
        public static readonly MoneyValue Zero = new(0, CurrencyValue.Empty);

        private MoneyValue() { } //ORM

        public MoneyValue(decimal amount, CurrencyValue currency)
        {

            Guard.Against.LessThanMinCurrency(amount, 0m, nameof(amount));
            Guard.Against.InvalidMinorUnits(amount, currency.MinorUnits, nameof(amount));

            Amount = amount;
            Currency = currency;

        }

        public static ValueResult<MoneyValue, Error> Create(decimal amount, CurrencyValue currency)
        {
            if (Check.For.LessThanMinValueValue(amount, 0).IsInvalid)
            {
                return Errors.ValueObjects.MoneyValue.GreaterThanOrEqualToZero;
            }

            if (decimal.Round(amount, currency.MinorUnits) != amount)
            {
                return Errors.ValueObjects.MoneyValue.InvalidNumberOfDecimalPlaces;
            }

            return new MoneyValue(amount, currency);
        }

        public decimal Amount { get; init; }

        public CurrencyValue Currency { get; init; } = default!;

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return Amount;
        }

        public static MoneyValue operator +(MoneyValue a, MoneyValue b)
        {
            if (a.Currency != b.Currency)
            {
                throw new KernelErrorException(Errors.ValueObjects.MoneyValue.CannotPerformArithmeticOperationOnDifferentCurrencies);
            }

            return new(a.Amount + b.Amount, a.Currency);
        }

        public static MoneyValue operator -(MoneyValue a, MoneyValue b)
        {
            if (a.Currency != b.Currency)
            {
                throw new KernelErrorException(Errors.ValueObjects.MoneyValue.CannotPerformArithmeticOperationOnDifferentCurrencies);
            }

            return new(a.Amount - b.Amount, a.Currency);
        }
    }
}
