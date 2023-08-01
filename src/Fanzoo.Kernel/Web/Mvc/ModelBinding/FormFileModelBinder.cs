using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Fanzoo.Kernel.Web.Mvc.ModelBinding
{
    public class FormFileModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // this is largely copied from the built-in model binder, but is more optimistic because we can regulate deviations in-house

            var modelName = bindingContext.IsTopLevelObject
                ? bindingContext.BinderModelName ?? bindingContext.FieldName
                : bindingContext.ModelName;

            var files = new List<IFormFile>();

            var request = bindingContext.HttpContext.Request;

            if (request.HasFormContentType)
            {
                var form = await request.ReadFormAsync();

                foreach (var file in form.Files)
                {
                    if (file.Length == 0 && string.IsNullOrEmpty(file.FileName))
                    {
                        continue;
                    }

                    if (file.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase))
                    {
                        files.Add(file);
                    }
                }
            }

            if (files.Count == 0)
            {
                return;
            }

            object? value = bindingContext.ModelType switch
            {
                Type t when t == typeof(IFormFile) => files[0],
                Type t when t == typeof(IFormFileCollection) => new FileCollection(files),
                _ => files,
            };

            bindingContext.ModelState.SetModelValue(
                modelName,
                rawValue: null,
                attemptedValue: null);

            bindingContext.Result = ModelBindingResult.Success(value);
        }
    }

    internal sealed class FileCollection : ReadOnlyCollection<IFormFile>, IFormFileCollection
    {
        public FileCollection(List<IFormFile> list) : base(list) { }

        public IFormFile? this[string name] => GetFile(name);

        public IFormFile? GetFile(string name)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var file = Items[i];

                if (string.Equals(name, file.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return file;
                }
            }

            return null;
        }

        public IReadOnlyList<IFormFile> GetFiles(string name)
        {
            var files = new List<IFormFile>();

            for (var i = 0; i < Items.Count; i++)
            {
                var file = Items[i];
                if (string.Equals(name, file.Name, StringComparison.OrdinalIgnoreCase))
                {
                    files.Add(file);
                }
            }

            return files;
        }
    }
}
