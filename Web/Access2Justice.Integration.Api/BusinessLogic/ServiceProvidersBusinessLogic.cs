using Access2Justice.Shared.Interfaces;
using Access2Justice.Integration.Api.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared;
using Access2Justice.Shared.Utilities;
using Access2Justice.Shared.Models.Integration;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;

namespace Access2Justice.Integration.Api.BusinessLogic
{
    /// <summary>
    /// Business logic for service provider
    /// </summary>
    public class ServiceProvidersBusinessLogic : IServiceProvidersBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        public ServiceProvidersBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        /// <summary>
        /// returns service provider based on id
        /// </summary>
        public async Task<dynamic> GetServiceProviderDocumentAsync(string id)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, Constants.Id, id).ConfigureAwait(false);
        }

        /// <summary>
        /// upserts service provider
        /// </summary>
        public async Task<IEnumerable<Document>> UpsertServiceProviderDocumentAsync(List<ServiceProvider> serviceProvider, string topic)
        {
            var resources = new List<Document>();
            var serviceProviderObjects = JsonUtilities.DeserializeDynamicObject<List<ServiceProvider>>(serviceProvider);
            foreach (var serviceProviderObject in serviceProviderObjects)
            {
                var serviceProviderLocation = new Location { State = serviceProviderObject.Location[0].State };
                if (!string.IsNullOrEmpty(topic))
                {
                    var topicDBData = await dbClient.FindItemsWhereContainsWithLocationAsync(dbSettings.TopicsCollectionId, Constants.Name, topic, serviceProviderLocation).ConfigureAwait(false);
                    serviceProviderObject.TopicTags = topicDBData.Count > 0 ? GetServiceProviderTopicTags(topicDBData[0].id) : null;
                }
                List<string> propertyNames = new List<string>() { Constants.ExternalId, Constants.ResourceType };
                List<string> values = new List<string>() { serviceProviderObject.ExternalId, serviceProviderObject.ResourceType };
                var serviceProviderDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values).ConfigureAwait(false);             
                if (serviceProviderDBData.Count == 0)
                {
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(serviceProviderObject);
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourcesCollectionId).ConfigureAwait(false);
                    resources.Add(result);
                }
                else
                {
                    serviceProviderObject.ResourceId = serviceProviderDBData[0].id.ToString();
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(serviceProviderObject);
                    var result = await dbService.UpdateItemAsync(serviceProviderObject.ResourceId, resourceDocument, dbSettings.ResourcesCollectionId).ConfigureAwait(false);
                    resources.Add(result);
                }
            }
            return resources;
        }
        
        /// <summary>
        /// returns topic tags
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetServiceProviderTopicTags(dynamic id)
        {
            List<TopicTag> topicTags = new List<TopicTag>
            {
                new TopicTag { TopicTags = id }
            };
            return topicTags;
        }
    }          
}