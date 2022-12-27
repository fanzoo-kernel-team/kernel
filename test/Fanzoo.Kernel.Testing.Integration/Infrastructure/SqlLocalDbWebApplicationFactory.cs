using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;

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
            StartUpInstance();

            CreateDatabase();

            builder.UseEnvironment("Test");

            base.ConfigureWebHost(builder);
        }

        private void StartUpInstance()
        {
            //shut down if something bad happened last time
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c sqllocaldb stop \"{_instanceName}\""
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();

            //nuke the old one
            startInfo.Arguments = $"/c sqllocaldb delete \"{_instanceName}\"";
            process.Start();
            process.WaitForExit();

            //clean up any lingering files
            foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(), $"{_databaseName}_*"))
            {
                File.Delete(file);
            }

            //start a new one
            startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c sqllocaldb create \"{_instanceName}\" -s"
            };

            process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();

        }

        private void CreateDatabase()
        {
            //create the database
            using var connection = new SqlConnection(@$"server=(localdb)\{_instanceName};Trusted_connection=yes;database=master;Integrated Security=true");

            var sql = @$"
                CREATE DATABASE [{_databaseName}]
                    ON PRIMARY(Name={_databaseName}_DATA, Filename=N'{Directory.GetCurrentDirectory()}\{_databaseName}_DATA.mdf')
                    LOG ON(Name={_databaseName}_LOG, Filename=N'{Directory.GetCurrentDirectory()}\{_databaseName}_LOG.ldf')";

            connection.Open();

            var command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }

        private void CleanUpInstance()
        {
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c sqllocaldb stop \"{_instanceName}\""
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = $"/c sqllocaldb delete \"{_instanceName}\"";
            process.Start();
            process.WaitForExit();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                CleanUpInstance();

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
