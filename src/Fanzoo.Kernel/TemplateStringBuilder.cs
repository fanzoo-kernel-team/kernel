namespace Fanzoo.Kernel
{
    public class TemplateStringBuilder(string template)
    {
        private const string _searchQualifier = "${{{0}}}";

        private readonly string _template = template;

        public TemplateStringBuilder(string template, Dictionary<string, object> templateValues) : this(template)
        {
            TemplateValues = templateValues;
        }

        public Dictionary<string, object> TemplateValues { get; private set; } = [];

        public override string ToString()
        {
            var s = new string(_template);

            foreach (var templateValue in TemplateValues)
            {
                var searchString = string.Format(_searchQualifier, templateValue.Key);

                s = s.Replace(searchString, templateValue.Value.ToString());
            }

            return s;
        }
    }
}
