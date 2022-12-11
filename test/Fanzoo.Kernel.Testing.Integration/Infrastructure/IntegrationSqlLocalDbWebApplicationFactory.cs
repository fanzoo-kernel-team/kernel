using Xunit;

namespace Fanzoo.Kernel.Testing.Integration
{

    [CollectionDefinition("LocalDb", DisableParallelization = true)]
    public class IntegrationSqlLocalDbWebApplicationFactoryCollection : ICollectionFixture<IntegrationSqlLocalDbWebApplicationFactory> { }

    public class IntegrationSqlLocalDbWebApplicationFactory : SqlLocalDbWebApplicationFactory<Program>
    {
        public IntegrationSqlLocalDbWebApplicationFactory() : base("kernel-test-instance", "videogames") { }
    }
}
