using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ConsoleHelperCore.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleHelperCore
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

                // configure services
                var services = new ServiceCollection()
                    .AddTransient<IAppService, AppService>()
                    .AddSingleton(config)
                    .AddSingleton<IDbConnection, SqlConnection>(options => new SqlConnection(config.GetConnectionString("DefaultConnection")));

                // create a service provider from the service collection
                var serviceProvider = services.BuildServiceProvider();

                // resolve the dependency graph
                var appService = serviceProvider.GetService<IAppService>();

                // run the application
                appService.RunAsync(new CancellationTokenSource().Token).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
