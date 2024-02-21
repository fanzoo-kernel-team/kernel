using System.Linq.Expressions;

namespace FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> StopOnEmpty<T, TProperty>(this IRuleBuilderInitial<T, TProperty> ruleBuilder) => ruleBuilder
                .Cascade(CascadeMode.Stop)
                    .NotEmpty();

        public static IRuleBuilderOptions<T, TProperty> MustWhenNotNull<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, Func<TProperty, bool> predicate)
        {
            var rule = ruleBuilder.Must(predicate);

            rule = rule.When((p, c) =>
            {
                var v = p?.GetPropertyValue(c.PropertyName);

                return typeof(TProperty) switch
                {
                    var type when type == typeof(string) => (v as string).IsNotNullOrWhitespace(),
                    var type when type == typeof(Guid) => v != null && !v.Equals(Guid.Empty),
                    var type when type == typeof(int) => v != null && !v.Equals(0),
                    var type when type == typeof(long) => v != null && !v.Equals(0),
                    var type when type == typeof(decimal) => v != null && !v.Equals(0),
                    var type when type == typeof(DateTime) => v != null && !v.Equals(DateTime.MinValue),
                    var type when type == typeof(DateTimeOffset) => v != null && !v.Equals(DateTimeOffset.MinValue),
                    var type when type == typeof(TimeSpan) => v != null && !v.Equals(TimeSpan.Zero),
                    _ => v != null,
                };
            });

            return rule;
        }

        public static IRuleBuilderOptions<T, DateTime> CurrentOrFutureDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder) => ruleBuilder.Must(p =>
        {
            var date = new DateTime(p.Year, p.Month, p.Day, 0, 0, 0, DateTimeKind.Utc);

            return date >= SystemDateTime.UtcNow.Date;
        });

        public static IRuleBuilderOptions<T, bool> MustBeTrue<T>(this IRuleBuilder<T, bool> ruleBuilder) => ruleBuilder.Must(p => p);

        public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this AbstractValidator<T> validator, Expression<Func<T, TProperty>> ruleFor, Func<TProperty, bool>? must = null, string? errorMessage = null)
        {
            var rule = validator
                .RuleFor(ruleFor)
                    .StopOnEmpty();

            if (must is not null)
            {
                rule = rule.Must(must);
            }

            if (errorMessage is not null)
            {
                rule = rule.WithMessage(errorMessage);
            }

            return rule;
        }
    }
}
