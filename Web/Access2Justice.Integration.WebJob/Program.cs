//using Access2Justice.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Access2Justice.Integration.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        private static void Main()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            var config = new JobHostConfiguration
            {
                JobActivator = new JobActivator(services.BuildServiceProvider())
            };
            config.UseTimers();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }
            var host = new JobHost(config);
            host.Start();

            Console.WriteLine("[{0}] Job Host started!!!", DateTime.Now);
            Console.ReadLine();

            // The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();
        }

        private static IConfiguration Configuration { get; set; }

        private static void ConfigureServices(IServiceCollection services)
        {

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            

            services.AddSingleton(Configuration);
            services.AddTransient<Functions, Functions>();
            //services.AddTransient<IHttpClientService, HttpClientService>();
            services.AddLogging(builder => builder.AddConsole());
        }
    }
}
