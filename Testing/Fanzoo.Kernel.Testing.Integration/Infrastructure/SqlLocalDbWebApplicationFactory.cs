using Xunit;

namespace Fanzoo.Kernel.Testing.Integration
{

    [CollectionDefinition("LocalDb")]
    public class SqlLocalDbWebApplicationFactoryCollection : ICollectionFixture<SqlLocalDbWebApplicationFactory> { }

    public class SqlLocalDbWebApplicationFactory : SqlLocalDbWebApplicationFactory<Program>
    {
        public SqlLocalDbWebApplicationFactory() : base("kernel-test-instance", "kernel") { }
    }
}
