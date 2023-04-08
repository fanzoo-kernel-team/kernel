#pragma warning disable IDE0046 // Convert to conditional expression
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()


namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class MoneyValue : ValueObject
    {
        public static readonly MoneyValue Zero = new(0, CurrencyValue.Empty);

        private MoneyValue() { } //ORM

        public MoneyValue(decimal amount, CurrencyValue currency)
        {

            Guard.Against.LessThan(amount, 0m, nameof(amount));
            Guard.Against.InvalidMinorUnits(amount, currency.MinorUnits, nameof(amount));

            Amount = amount;
            Currency = currency;

        }

        public static ValueResult<MoneyValue, Error> Create(decimal amount, CurrencyValue currency) => CanCreate(amount, currency)
               ? new MoneyValue(amount, currency)
               : Errors.ValueObjects.MoneyValue.InvalidFormat;

        public decimal Amount { get; init; }

        public CurrencyValue Currency { get; init; } = default!;

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return Amount;
            yield return Currency;
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

        public static bool operator ==(MoneyValue a, decimal amount) => a.Amount == amount;

        public static bool operator !=(MoneyValue a, decimal amount) => a.Amount != amount;

        public static bool CanCreate(decimal amount, CurrencyValue currency) => Check.For.IsValidMoneyFormat(amount, currency);

    }
}
