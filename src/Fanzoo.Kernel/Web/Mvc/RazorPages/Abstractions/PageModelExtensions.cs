using FluentValidation;

namespace Microsoft.AspNetCore.Mvc.RazorPages
{
    public class PageValidationResult(bool isValid)
    {
        public bool IsValid { get; } = isValid;

        public bool IsNotValid => !IsValid;
    }

    public static class PageModelExtentions
    {
        public static PageValidationResult GetValidation<TValidator, TPageModel>(this TPageModel pageModel, params object?[]? args)
            where TValidator : AbstractValidator<TPageModel>
            where TPageModel : notnull, PageModel
        {
            if (Activator.CreateInstance(typeof(TValidator), args) is not TValidator validator)
            {
                throw new InvalidOperationException("Validator not found.");
            }

            var result = validator.Validate(pageModel);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    pageModel.ModelState.ClearValidationState(error.PropertyName);

                    pageModel.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            return new(result.IsValid);
        }

        public static PageValidationResult GetValidation<TValidator, TInstance>(this PageModel pageModel, TInstance instance, string? name = null, params object?[]? args)
            where TValidator : AbstractValidator<TInstance>
            where TInstance : notnull
        {
            if (Activator.CreateInstance(typeof(TValidator), args) is not TValidator validator)
            {
                throw new InvalidOperationException("Validator not found.");
            }

            var result = validator.Validate(instance);

            if (!result.IsValid)
            {
                name ??= instance.GetType().Name;

                foreach (var error in result.Errors)
                {
                    var property = $"{name}.{error.PropertyName}";

                    pageModel.ModelState.ClearValidationState(property);

                    pageModel.ModelState.AddModelError(property, error.ErrorMessage);
                }
            }

            return new(result.IsValid);
        }

        public static bool Validate<TValidator, TPageModel>(this TPageModel pageModel, params object?[]? args)
            where TValidator : AbstractValidator<TPageModel>
            where TPageModel : notnull, PageModel =>
                GetValidation<TValidator, TPageModel>(pageModel, args).IsValid;

        public static bool Validate<TValidator, TInstance>(this PageModel pageModel, TInstance instance, string? name = null, params object?[]? args)
            where TValidator : AbstractValidator<TInstance>
            where TInstance : notnull =>
                GetValidation<TValidator, TInstance>(pageModel, instance, name, args).IsValid;

        public static async Task<PageValidationResult> GetValidationAsync<TValidator, TPageModel>(this TPageModel pageModel, params object?[]? args)
            where TValidator : AbstractValidator<TPageModel>
            where TPageModel : notnull, PageModel
        {
            if (Activator.CreateInstance(typeof(TValidator), args) is not TValidator validator)
            {
                throw new InvalidOperationException("Validator not found.");
            }

            var result = await validator.ValidateAsync(pageModel);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    pageModel.ModelState.ClearValidationState(error.PropertyName);

                    pageModel.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            return new(result.IsValid);
        }

        public static async Task<PageValidationResult> GetValidationAsync<TValidator, TInstance>(this PageModel pageModel, TInstance instance, string? name = null, params object?[]? args)
            where TValidator : AbstractValidator<TInstance>
            where TInstance : notnull
        {
            if (Activator.CreateInstance(typeof(TValidator), args) is not TValidator validator)
            {
                throw new InvalidOperationException("Validator not found.");
            }

            var result = await validator.ValidateAsync(instance);

            if (!result.IsValid)
            {
                name ??= instance.GetType().Name;

                foreach (var error in result.Errors)
                {
                    var property = $"{name}.{error.PropertyName}";

                    pageModel.ModelState.ClearValidationState(property);

                    pageModel.ModelState.AddModelError(property, error.ErrorMessage);
                }
            }

            return new(result.IsValid);
        }

        public static async Task<bool> ValidateAsync<TValidator, TPageModel>(this TPageModel pageModel, params object?[]? args)
            where TValidator : AbstractValidator<TPageModel>
            where TPageModel : notnull, PageModel =>
                (await GetValidationAsync<TValidator, TPageModel>(pageModel, args)).IsValid;

        public static async Task<bool> ValidateAsync<TValidator, TInstance>(this PageModel pageModel, TInstance instance, string? name = null, params object?[]? args)
            where TValidator : AbstractValidator<TInstance>
            where TInstance : notnull =>
                (await GetValidationAsync<TValidator, TInstance>(pageModel, instance, name, args)).IsValid;
    }
}
