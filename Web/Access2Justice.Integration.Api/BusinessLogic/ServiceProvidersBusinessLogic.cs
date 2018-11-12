using Access2Justice.Shared.Interfaces;
using Access2Justice.Integration.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Shared;

namespace Access2Justice.Integration.Api.BusinessLogic
{
    public class ServiceProvidersBusinessLogic : IServiceProvidersBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public ServiceProvidersBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetServiceProviderDocumentAsync(string topicName)
        {
            var result = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Name, topicName);
            return result;
        }
    }
          
}