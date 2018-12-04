using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Access2Justice.Integration;
using Access2Justice.Integration.Interfaces;
using Access2Justice.Integration.Models;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Access2Justice.WebJob
{
    public class Functions
    {
        private readonly ILogger<Functions> logger;
        private readonly IHttpClientService httpClient;
        private readonly AdapterFactory adapterFactory;
        private readonly IA2JSettings a2JSettings;
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;

        public Functions(ILogger<Functions> logger, IHttpClientService httpClient,
            AdapterFactory adapterFactory, IA2JSettings a2JSettings,
            IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.adapterFactory = adapterFactory;
            this.a2JSettings = a2JSettings;
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
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
                var adapterData = GetAllServiceProviders().Result;
                var adapterSettings = JsonUtilities.DeserializeDynamicObject<List<AdapterSettings>>(adapterData);                
                foreach (var adapterSetting in adapterSettings)
                {
                    var serviceProviders = adapterFactory.CallServiceProvider(adapterSetting).Result;
                    if (serviceProviders?.Count > 0)
                    {
                        var spSerialized = JsonConvert.SerializeObject(serviceProviders);
                        var bufferSP = System.Text.Encoding.UTF8.GetBytes(spSerialized);
                        var byteContentSP = new ByteArrayContent(bufferSP);
                        byteContentSP.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var spResponse = httpClient.PostAsync(a2JSettings.ServiceProviderURL, byteContentSP).Result;
                        string spResult = spResponse.Content.ReadAsStringAsync().Result;
                        if (spResponse.IsSuccessStatusCode)
                        {
                            logger.LogInformation(spResult);
                            logger.LogInformation("Transaction completed successfully");
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);                
            }
            
        }

        public async Task<dynamic> GetAllServiceProviders()
        {
            return await dbClient.FindItemsAllAsync(dbSettings.ServiceProviderCollectionId).ConfigureAwait(false);            
        }
    }
}
