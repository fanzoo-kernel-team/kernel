namespace Fanzoo.Kernel
{
    public class TemplateStringBuilder
    {
        private const string SEARCH_QUALIFIER = "${{{0}}}";

        private readonly string _template;

        public TemplateStringBuilder(string template)
        {
            _template = template;
            TemplateValues = new Dictionary<string, object>();
        }

        public TemplateStringBuilder(string template, Dictionary<string, object> templateValues) : this(template)
        {
            TemplateValues = templateValues;
        }

        public Dictionary<string, object> TemplateValues { get; private set; }

        public override string ToString()
        {
            var s = new string(_template);

            foreach (var templateValue in TemplateValues)
            {
                var searchString = string.Format(SEARCH_QUALIFIER, templateValue.Key);

                s = s.Replace(searchString, templateValue.Value.ToString());
            }

            return s;
        }
    }
}
