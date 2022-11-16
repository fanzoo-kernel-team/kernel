#pragma warning disable IDE0046 // Convert to conditional expression

namespace Fanzoo.Kernel.Domain.Values
{
    public sealed class MoneyValue : ValueObject
    {
        public static readonly MoneyValue Zero = new(0, CurrencyValue.Empty);

        private MoneyValue() { } //ORM

        public MoneyValue(decimal amount, CurrencyValue currency)
        {
            Amount = amount;
            Currency = currency;
            Guard.Against.LessThanMinCurrency(amount, 0m, nameof(amount));
            Guard.Against.InvalidMinorUnits(amount, currency.MinorUnits, nameof(amount));

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
            //check that the currency is the same
            //if not, throw exception

            //throw new KernelErrorException(Errors.ValueObjects.MoneyValue.CannotPerformArithmeticOperationOnDifferentCurrencies);

            var result = MoneyValue.Create(100, CurrencyValue.USDollar);

            var money = result.Value;

            return Create(a.Amount + b.Amount, a.Currency).Value;
        }

        //TODO: operators

        /*
         
            var amount1 = MoneyValue.Create(100, USD);
            var amount2 = MoneyValue.Create(200, USD);

            var dollar = amount1.Amount + amount2.Amount;

            var amount3 = MoneyValue.Create(dollar, USD);

            var amount3 = amount1 + amount2;
         
        */
    }
}
