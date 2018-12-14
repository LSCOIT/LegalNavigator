using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Access2Justice.AdapterFunction
{
    public static class Function
    {
        private static IConfiguration Configuration { get; set; }

        [FunctionName("AdapterFunction")]
        public static void Run([TimerTrigger("* * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                // Todo:@Rakesh Need to check how to fetch the URL from configuration.
                //AdapterSettings adapterSettings = Function.GetAdapterConfiguration();
                AdapterSettings adapterSettings = new AdapterSettings { AdapterApiUrl = new Uri("https://a2jdevintegrationapi.azurewebsites.net/api/service-providers/load-partners-data/{0}") };
                if (adapterSettings != null)
                {
                    string topicName = "Child Advocacy Centers";
                    HttpClient httpClient = new HttpClient();
                    string serviceProviderUrl = string.Format(CultureInfo.InvariantCulture, adapterSettings.AdapterApiUrl.OriginalString, topicName);
                    var response = httpClient.GetAsync(serviceProviderUrl).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        log.LogInformation("service providers call with ok status code");                        
                    }
                    log.LogInformation(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Reads Adapter configuration from json file.
        /// </summary>
        /// <returns>Adapter configuration</returns>
        public static AdapterSettings GetAdapterConfiguration()
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                //AdapterSettings adminSettings = new AdapterSettings(Configuration.GetSection("AdapterSettings"));
                AdapterSettings adminSettings = new AdapterSettings();
                return adminSettings;
            }
            catch(Exception ex)
            {
                throw new Exception("Invalid Application configurations");
            }
        }
    }
}
