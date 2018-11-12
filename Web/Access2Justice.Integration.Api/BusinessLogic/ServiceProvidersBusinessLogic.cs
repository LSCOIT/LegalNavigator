using Access2Justice.Shared.Interfaces;
using Access2Justice.Integration.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Shared;
using Access2Justice.Shared.Utilities;
using Access2Justice.Shared.Models.Integration;
using Access2Justice.Shared.Models;
//using Access2Justice.Api.BusinessLogic;

namespace Access2Justice.Integration.Api.BusinessLogic
{
    public class ServiceProvidersBusinessLogic : IServiceProvidersBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly ITopicsResourcesBusinessLogic topicsResources;
        public ServiceProvidersBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetServiceProviderDocumentAsync(string topicName)
        {

            var result = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, Constants.Name, topicName);
            return result;
        }

        public async Task<IEnumerable<object>> UpsertServiceProviderDocumentAsync(dynamic serviceProviders)
        {
            List<dynamic> results = new List<dynamic>();
            List<dynamic> resources = new List<dynamic>();
            var serviceProviderObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(serviceProviders);
            ServiceProvider serviceProvider = new ServiceProvider();

            foreach (var serviceProviderObject in serviceProviderObjects)
            {
                string id = serviceProviderObject.id;
                string resourceType = Constants.ServiceProviderResourceType;
                serviceProvider = UpsertServiceProvider(serviceProviderObject);
                var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(serviceProvider);
                List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                List<string> values = new List<string>() { id, resourceType };
                var serviceProviderDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                if (serviceProviderDBData.Count == 0)
                {
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourcesCollectionId);
                    resources.Add(result);
                }
                else
                {
                    var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourcesCollectionId);
                    resources.Add(result);
                }                
            }
            return resources;
        }

        public dynamic UpsertServiceProvider(dynamic ServiceProvider)
        {
            ServiceProvider serviceProvider = new ServiceProvider();

            Organization organizations = new Organization();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            //dynamic references = topicsResources.GetReferences(ServiceProvider);
            //topicTags = references[0];
            //locations = references[1];
            //organizationReviewers = references[4];

            serviceProvider = new ServiceProvider()
            {
                ResourceId = ServiceProvider.id == "" ? Guid.NewGuid() : ServiceProvider.id,
                Name = ServiceProvider.name,
                ResourceCategory = ServiceProvider.resourceCategory,
                Description = ServiceProvider.description,
                ResourceType = ServiceProvider.resourceType,
                Url = ServiceProvider.url,
                TopicTags = topicTags,
                OrganizationalUnit = ServiceProvider.organizationalUnit,
                Location = locations,
                CreatedBy = ServiceProvider.createdBy,
                ModifiedBy = ServiceProvider.modifiedBy,
                Address = ServiceProvider.address,
                Telephone = ServiceProvider.telephone,
                Overview = ServiceProvider.overview,
                Specialties = ServiceProvider.specialties,
                EligibilityInformation = ServiceProvider.eligibilityInformation,
                Qualifications = ServiceProvider.qualifications,
                BusinessHours = ServiceProvider.businessHours,
                Reviewer = organizationReviewers
            };
            //serviceProvider.Validate();
            return serviceProvider;
        }
    }
          
}