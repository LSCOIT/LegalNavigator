using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.IntegrationAdapters
{
    public class ServiceProvidersOrchestrator : IServiceProvidersOrchestrator
    {
        private readonly IEnumerable<IServiceProviderAdapter> serviceProviderAdaptors;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly ICosmosDbSettings cosmosDbSettings;

        public ServiceProvidersOrchestrator(IEnumerable<IServiceProviderAdapter> serviceProviderAdaptors, IBackendDatabaseService backendDatabaseService, ICosmosDbSettings cosmosDbSettings)
        {
            this.serviceProviderAdaptors = serviceProviderAdaptors;
            this.backendDatabaseService = backendDatabaseService;
            this.cosmosDbSettings = cosmosDbSettings;
        }

        public async Task<bool> LoadServiceProviders(string topicName)
        {
            foreach (var adapter in serviceProviderAdaptors)
            {
                var serviceProviders = await adapter.GetServiceProviders(topicName);
                foreach (var provider in serviceProviders)
                {
                    await backendDatabaseService.CreateItemAsync(await adapter.GetServiceProviderDetails(provider), cosmosDbSettings.RolesCollectionId);
                }
            }

            return true;
        }
    }
}
