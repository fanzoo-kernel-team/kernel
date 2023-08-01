using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace Fanzoo.Kernel.Web.Mvc.ModelBinding
{
    public interface IFormData
    {
        public object Model { get; }
    }

    public sealed class FormData<TModel> : IFormData
    {
        public FormData(TModel model)
        {
            Model = model;
        }

        public TModel Model { get; }

        object IFormData.Model => Model!;

        public static async ValueTask<FormData<TModel>> BindAsync(HttpContext httpContext)
        {
            var serviceProvider = httpContext.RequestServices;
            var factory = serviceProvider.GetRequiredService<IModelBinderFactory>();
            var metadataProvider = serviceProvider.GetRequiredService<IModelMetadataProvider>();

            var metadata = metadataProvider.GetMetadataForType(typeof(TModel));

            var modelBinder = factory.CreateBinder(new()
            {
                Metadata = metadata
            });

            var context = new DefaultModelBindingContext
            {
                ModelMetadata = metadata,
                ModelName = string.Empty,
                ValueProvider = new FormValueProvider(
                    BindingSource.Form,
                    await httpContext.Request.ReadFormAsync(),
                    CultureInfo.InvariantCulture
                ),
                ActionContext = new ActionContext(
                    httpContext,
                    new RouteData(),
                    new ActionDescriptor()),
                ModelState = new ModelStateDictionary()
            };

            await modelBinder.BindModelAsync(context);

            return new FormData<TModel>((TModel)(context.Result.Model ?? default!));
        }

        public static implicit operator TModel(FormData<TModel> formData) => formData.Model;
    }
}
