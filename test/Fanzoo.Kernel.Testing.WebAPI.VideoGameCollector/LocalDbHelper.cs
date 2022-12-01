using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace Fanzoo.Kernel.Testing.WebAPI.VideoGameCollector
{
    public static class LocalDbHelper
    {
        public static void StartUpInstance(string instanceName, string databaseName)
        {
            //shut down if something bad happened last time
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c sqllocaldb stop \"{instanceName}\""
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();

            //nuke the old one
            startInfo.Arguments = $"/c sqllocaldb delete \"{instanceName}\"";
            process.Start();
            process.WaitForExit();

            //clean up any lingering files
            foreach (var file in Directory.GetFiles(Directory.GetCurrentDirectory(), $"{databaseName}_*"))
            {
                File.Delete(file);
            }

            //start a new one
            startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c sqllocaldb create \"{instanceName}\" -s"
            };

            process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();

        }

        public static void CreateDatabase(string instanceName, string databaseName)
        {
            //create the database
            using var connection = new SqlConnection(@$"server=(localdb)\{instanceName};Trusted_connection=yes;database=master;Integrated Security=true");

            var sql = @$"
                CREATE DATABASE [{databaseName}]
                    ON PRIMARY(Name={databaseName}_DATA, Filename=N'{Directory.GetCurrentDirectory()}\{databaseName}_DATA.mdf')
                    LOG ON(Name={databaseName}_LOG, Filename=N'{Directory.GetCurrentDirectory()}\{databaseName}_LOG.ldf')";

            connection.Open();

            var command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }

        public static void CleanUpInstance(string instanceName)
        {
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/c sqllocaldb stop \"{instanceName}\""
            };

            var process = new Process { StartInfo = startInfo };
            process.Start();
            process.WaitForExit();

            startInfo.Arguments = $"/c sqllocaldb delete \"{instanceName}\"";
            process.Start();
            process.WaitForExit();

        }
    }
}
