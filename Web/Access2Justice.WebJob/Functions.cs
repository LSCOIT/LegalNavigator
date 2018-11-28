using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Access2Justice.WebJob
{
    public class Functions
    {
        private readonly ILogger<Functions> logger;
        private readonly IHttpClientService httpClient;
        private readonly IServiceProviderAdapter serviceProviderAdapter;
        private readonly IA2JSettings a2JSettings;

        public Functions(ILogger<Functions> logger, IHttpClientService httpClient,
            IServiceProviderAdapter serviceProviderAdapter, IA2JSettings a2JSettings)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.serviceProviderAdapter = serviceProviderAdapter;
            this.a2JSettings = a2JSettings;
        }

        public void ProcessQueueMessage([TimerTrigger("* * * * *")]TimerInfo timerInfo)
        {
            logger.LogInformation("Web Job started Running");
            ProcessRTMData();
            logger.LogInformation("Web Job Completed");
        }

        public void ProcessRTMData()
        {
            try
            {
                var serviceProviders = serviceProviderAdapter.GetServiceProviders("Family").ConfigureAwait(false);
                var result = JsonConvert.SerializeObject(serviceProviders);
                if (!string.IsNullOrEmpty(result))
                {
                    var bufferSP = System.Text.Encoding.UTF8.GetBytes(result);
                    var byteContentSP = new ByteArrayContent(bufferSP);
                    byteContentSP.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var spResponse = httpClient.PostAsync(a2JSettings.ServiceProviderURL, byteContentSP).Result;
                    string spResult = spResponse.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(spResult))
                    {                        
                        logger.LogInformation(spResult);
                        logger.LogInformation("Transaction completed successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);                
            }
            
        }
    }
}
