using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Integration.Interfaces;
using Access2Justice.Integration.Models;
using Access2Justice.Integration.Partners.Rtm;

namespace Access2Justice.WebJob
{
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

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
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
            services.AddSingleton<IHttpClientService, HttpClientService>();
            services.AddSingleton<IServiceProviderAdapter, RtmServiceProviderAdapter>();
            services.AddTransient<Functions, Functions>();
            services.AddLogging(builder => builder.AddConsole());

            IA2JSettings a2JSettings = new A2JSettings(Configuration.GetSection("A2JSettings"));
            services.AddSingleton(a2JSettings);
        }
    }
}
