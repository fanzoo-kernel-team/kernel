using System.Net;
using Fanzoo.Kernel.Web.Mvc.ModelBinding;
using FluentValidation;

// hat tip to Ben Foster on this: https://benfoster.io/blog/minimal-api-validation-endpoint-filters/

// TODO: move to kernel
namespace Fanzoo.Kernel.Web.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidateAttribute : Attribute { }

    public static class ValidationFilter
    {
        public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
        {
            var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices);

            if (validationDescriptors.Any())
            {
                return invocationContext => ValidateAsync(validationDescriptors, invocationContext, next);
            }

            // pass-thru
            return invocationContext => next(invocationContext);
        }

        private static async ValueTask<object?> ValidateAsync(IEnumerable<ValidationDescriptor> validationDescriptors, EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
        {
            foreach (var descriptor in validationDescriptors)
            {
                var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

                if (argument is not null && argument.GetType().IsGenericType && argument.GetType().GetGenericTypeDefinition() == typeof(FormData<>))
                {
                    argument = ((IFormData)argument).Model;
                }

                if (argument is not null)
                {
                    var validationResult = await descriptor.Validator.ValidateAsync(new ValidationContext<object>(argument));

                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary(), statusCode: (int)HttpStatusCode.UnprocessableEntity);
                    }
                }
            }

            return await next.Invoke(invocationContext);
        }

        private static IEnumerable<ValidationDescriptor> GetValidators(MethodInfo methodInfo, IServiceProvider serviceProvider)
        {
            var parameters = methodInfo.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var typeToValidate = parameters[i].ParameterType;

                // check for FormData wrapper
                if (typeToValidate.IsGenericType && typeToValidate.GetGenericTypeDefinition() == typeof(FormData<>))
                {
                    typeToValidate = typeToValidate.GetGenericArguments()[0];
                }

                if (parameter.GetCustomAttribute<ValidateAttribute>() is not null)
                {
                    var validatorType = typeof(IValidator<>).MakeGenericType(typeToValidate);

                    // Note that FluentValidation validators needs to be registered as singleton
                    var validator = serviceProvider.GetService(validatorType) as IValidator;

                    if (validator is not null)
                    {
                        yield return new ValidationDescriptor { ArgumentIndex = i, ArgumentType = typeToValidate, Validator = validator };
                    }
                }
            }
        }

        private sealed class ValidationDescriptor
        {
            public required int ArgumentIndex { get; init; }

            public required Type ArgumentType { get; init; }

            public required IValidator Validator { get; init; }
        }
    }
}
