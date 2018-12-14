using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.Integration;
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
        private readonly IA2JSettings a2jSettings;

        public ServiceProvidersOrchestrator(IEnumerable<IServiceProviderAdapter> serviceProviderAdaptors, IBackendDatabaseService backendDatabaseService,
            ICosmosDbSettings cosmosDbSettings, IHttpClientService httpClientService,IA2JSettings a2jSettings)
        {
            this.serviceProviderAdaptors = serviceProviderAdaptors;
            this.backendDatabaseService = backendDatabaseService;
            this.cosmosDbSettings = cosmosDbSettings;
            this.httpClientService = httpClientService;
            this.a2jSettings = a2jSettings;
        }

        public async Task<bool> LoadServiceProviders(string topicName)
        {
            foreach (var adapter in serviceProviderAdaptors)
            {
                var providerIds = await adapter.GetServiceProviders(topicName);
                foreach (var providerId in providerIds)
                {
                    var serviceProvider = await adapter.GetServiceProviderDetails(providerId);
                    List<ServiceProvider> serviceProviders = new List<ServiceProvider> { serviceProvider };
                    var spSerialized = JsonConvert.SerializeObject(serviceProviders);
                    var bufferSP = System.Text.Encoding.UTF8.GetBytes(spSerialized);
                    var byteContentSP = new ByteArrayContent(bufferSP);
                    byteContentSP.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var spResponse = httpClientService.PostAsync(a2jSettings.ServiceProviderURL, byteContentSP).Result;
                    string spResult = spResponse.Content.ReadAsStringAsync().Result;
                }
            }
            return true;
        }
    }
}
