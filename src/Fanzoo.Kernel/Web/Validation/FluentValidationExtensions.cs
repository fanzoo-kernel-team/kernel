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

            switch (typeof(TProperty))
            {
                case Type type when type == typeof(string):
                    rule.When(p => (p as string).IsNotNullOrWhitespace());
                    break;

                case Type type when type == typeof(Guid):
                    rule.When(p => p != null && !p.Equals(Guid.Empty));
                    break;

                case Type type when type == typeof(int):
                    rule.When(p => p != null && !p.Equals(0));
                    break;

                case Type type when type == typeof(long):
                    rule.When(p => p != null && !p.Equals(0));
                    break;

                case Type type when type == typeof(decimal):
                    rule.When(p => p != null && !p.Equals(0));
                    break;

                case Type type when type == typeof(DateTime):
                    rule.When(p => p != null && !p.Equals(DateTime.MinValue));
                    break;

                case Type type when type == typeof(DateTimeOffset):
                    rule.When(p => p != null && !p.Equals(DateTimeOffset.MinValue));
                    break;

                case Type type when type == typeof(TimeSpan):
                    rule.When(p => p != null && !p.Equals(TimeSpan.Zero));
                    break;

                default:
                    rule.When(p => p != null);
                    break;
            }

            return rule;
        }
    }
}
