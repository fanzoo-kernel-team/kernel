using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Fanzoo.Kernel.Testing.Integration
{
    public abstract class SqlLocalDbWebApplicationFactory<TStartUp> : WebApplicationFactory<TStartUp>
        where TStartUp : class
    {
        private readonly string _instanceName;
        private readonly string _databaseName;

        private bool _disposed = false;

        protected SqlLocalDbWebApplicationFactory(string instanceName, string databaseName)
        {
            _instanceName = instanceName;
            _databaseName = databaseName;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            LocalDbHelper.StartUpInstance(_instanceName, _databaseName);

            LocalDbHelper.CreateDatabase(_instanceName, _databaseName);

            builder.UseEnvironment("Test");

            base.ConfigureWebHost(builder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                LocalDbHelper.CleanUpInstance(_instanceName);

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
