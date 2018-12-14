using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.IntegrationAdapters
{
    public class ServiceProvidersOrchestrator : IServiceProvidersOrchestrator
    {
        private readonly IEnumerable<IServiceProviderAdapter> serviceProviderAdaptors;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IHttpClientService httpClientService;

        public ServiceProvidersOrchestrator(IEnumerable<IServiceProviderAdapter> serviceProviderAdaptors, IBackendDatabaseService backendDatabaseService,
            ICosmosDbSettings cosmosDbSettings, IHttpClientService httpClientService)
        {
            this.serviceProviderAdaptors = serviceProviderAdaptors;
            this.backendDatabaseService = backendDatabaseService;
            this.cosmosDbSettings = cosmosDbSettings;
            this.httpClientService = httpClientService;
        }

         // Todo:@Rakesh feel free to change the signature List<string> topicNames
        public async Task<bool> LoadServiceProviders(string topicName)
        {
            foreach (var adapter in serviceProviderAdaptors)
            {
                 // Todo:@Rakesh do whatever the webjob used to do!
                var serviceProviders = await adapter.GetServiceProviders(topicName);
                foreach (var provider in serviceProviders)
                {
                    var providerIds = adapter.GetServiceProviders("Family").Result;
                    foreach (var providerId in providerIds)
                    {
                        var serviceProvider = adapter.GetServiceProviderDetails(providerId);
                        var spSerialized = JsonConvert.SerializeObject(serviceProvider);
                        var bufferSP = System.Text.Encoding.UTF8.GetBytes(spSerialized);
                        var byteContentSP = new ByteArrayContent(bufferSP);
                        byteContentSP.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        //var spResponse = httpClientService.PostAsync(@"https://a2jdevintegrationapi.azurewebsites.net/api/service-providers", byteContentSP).Result;
                        //string spResult = spResponse.Content.ReadAsStringAsync().Result;
                    }
                }
            }

            return true;
        }
    }
}
