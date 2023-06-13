

namespace Fanzoo.Kernel
{
    public static class ReflectionExtensions
    {

        public static object? GetFieldValue(this object obj, string name)
        {
            Guard.Against.Null(obj, nameof(obj));

            var type = obj.GetType();

            var fieldInfo = GetFieldInfo(type, name) ?? throw new InvalidOperationException($"{name} not found.");

            return fieldInfo.GetValue(obj);
        }

        public static void SetFieldValue(this object obj, string name, object val)
        {
            Guard.Against.Null(obj, nameof(obj));

            var type = obj.GetType();

            var fieldInfo = GetFieldInfo(type, name) ?? throw new InvalidOperationException($"{name} not found.");

            fieldInfo.SetValue(obj, val);
        }

        private static FieldInfo? GetFieldInfo(Type type, string name)
        {
            var searchType = type; //make it nullable

            FieldInfo? fieldInfo;

            do
            {
                fieldInfo = searchType?.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                searchType = searchType?.BaseType;

            } while (fieldInfo is null && searchType is not null);

            return fieldInfo;
        }

        public static object? GetPropertyValue(this object obj, string name)
        {
            Guard.Against.Null(obj, nameof(obj));

            var type = obj.GetType();

            var propertyInfo = GetPropertyInfo(type, name) ?? throw new InvalidOperationException($"{name} not found.");

            return propertyInfo.GetValue(obj, null);
        }

        public static void SetPropertyValue(this object obj, string name, object val)
        {
            Guard.Against.Null(obj, nameof(obj));

            var type = obj.GetType();

            var propertyInfo = GetPropertyInfo(type, name) ?? throw new InvalidOperationException($"{name} not found.");

            propertyInfo.SetValue(obj, val, null);
        }

        private static PropertyInfo? GetPropertyInfo(Type type, string name)
        {
            var searchType = type; //make it nullable

            PropertyInfo? propertyInfo;

            do
            {
                propertyInfo = searchType?.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                searchType = searchType?.BaseType;

            } while (propertyInfo is null && searchType is not null);

            return propertyInfo;
        }
    }
}
