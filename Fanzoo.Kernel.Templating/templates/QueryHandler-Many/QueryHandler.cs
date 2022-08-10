using {project}.Dtos.{area};
using {project}.Queries.{area};

namespace {project}.QueryHandlers.{area}
{
    public class {name}QueryHandler : QueryHandler<{name}Query, IEnumerable<{returntype}>>
    {
        public {name}QueryHandler(IConfiguration configuration, IDynamicMappingService mapper, IEmbeddedResourceReaderService embeddedResourceReaderService, IScriptEmbeddedResourceLocator embeddedResourceLocator) : base(configuration, mapper, embeddedResourceReaderService, embeddedResourceLocator) { }

        protected override async Task<QueryResult<IEnumerable<{returntype}>>> OnHandleAsync({name}Query query) => await QueryFromScriptAsync<{returntype}>("{area}\\Scripts\\{name}.sql");
    }
}