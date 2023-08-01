using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Fanzoo.Kernel.Web.Mvc.ModelBinding
{
    public class FormFileModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;

            return modelType == typeof(IFormFile) || modelType == typeof(IFormFileCollection) || typeof(IEnumerable<IFormFile>).IsAssignableFrom(modelType)
                ? new BinderTypeModelBinder(typeof(FormFileModelBinder))
                : (IModelBinder?)null;
        }
    }
}
