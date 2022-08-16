using {project}.Dtos.{area};
using {project}.Queries.{area};

namespace {project}.QueryHandlers.{area}
{
    public class {name}QueryHandler : QueryHandler<{name}Query, {returntype}>
    {
        public {name}QueryHandler(IConfiguration configuration, IDynamicMappingService mapper, IEmbeddedResourceReaderService embeddedResourceReaderService, IScriptEmbeddedResourceLocator embeddedResourceLocator) : base(configuration, mapper, embeddedResourceReaderService, embeddedResourceLocator) { }

        protected override async Task<QueryResult<{returntype}>> OnHandleAsync({name}Query query) => await QuerySingleFromScriptAsync<{returntype}>("{area}\\Scripts\\{name}.sql");
    }
}