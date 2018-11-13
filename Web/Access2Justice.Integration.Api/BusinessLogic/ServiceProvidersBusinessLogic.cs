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
                var serviceProviderDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values).ConfigureAwait(false);
                if (serviceProviderDBData.Count == 0)
                {
                    var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourcesCollectionId).ConfigureAwait(false);
                    resources.Add(result);
                }
                else
                {
                    var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourcesCollectionId).ConfigureAwait(false);
                    resources.Add(result);
                }                
            }
            return resources;
        }

        /// <summary>
        /// upserts service provider
        /// </summary>
        public dynamic UpsertServiceProvider(dynamic ServiceProvider)
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            Organization organizations = new Organization();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            Availability availability = new Availability();
            AcceptanceCriteria acceptanceCriteria = new AcceptanceCriteria();
            OnboardingInfo onboardingInfo = new OnboardingInfo();
            SharedReferences sharedReferences = new SharedReferences();
            dynamic references = sharedReferences.GetReferences(ServiceProvider);
            topicTags = references[0];
            locations = references[1];
            organizationReviewers = references[4];
            availability = references[6];
            acceptanceCriteria = references[7];
            onboardingInfo = references[8];

            serviceProvider = new ServiceProvider()
            {
                Availability = availability,
                AcceptanceCriteria = acceptanceCriteria,
                OnboardingInfo = onboardingInfo,
                ResourceId = ServiceProvider.id == "" ? Guid.NewGuid() : ServiceProvider.id,
                Name = ServiceProvider.name,
                ResourceCategory = ServiceProvider.resourceCategory,
                Description = ServiceProvider.description,
                ResourceType = Constants.ServiceProviderResourceType,
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

        public dynamic UpsertServiceProviders(dynamic ServiceProvider)
        {
            ServiceProvider serviceProvider = new ServiceProvider();

            Organization organizations = new Organization();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            Availability availability = new Availability();
            AcceptanceCriteria acceptanceCriteria = new AcceptanceCriteria();
            OnboardingInfo onboardingInfo = new OnboardingInfo(); 
            SharedReferences sharedReferences = new SharedReferences();
            dynamic references = sharedReferences.GetReferences(ServiceProvider);
            topicTags = references[0];
            locations = references[1];
            organizationReviewers = references[4];
            availability = references[6];
            acceptanceCriteria = references[7];
            onboardingInfo = references[8];

            serviceProvider = new ServiceProvider()
            {
                Availability = availability,
                AcceptanceCriteria = acceptanceCriteria,
                OnboardingInfo = onboardingInfo,
                ResourceId = ServiceProvider.id == "" ? Guid.NewGuid() : ServiceProvider.id,
                Name = ServiceProvider.name,
                ResourceCategory = ServiceProvider.resourceCategory,
                Description = ServiceProvider.description,
                ResourceType = Constants.ServiceProviderResourceType,
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