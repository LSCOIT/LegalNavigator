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
        public async Task<IEnumerable<object>> UpsertServiceProviderDocumentAsync(dynamic serviceProviderJson, dynamic providerDetailJson, dynamic topicNameJson)
        {
            List<dynamic> results = new List<dynamic>();
            List<dynamic> resources = new List<dynamic>();
            var serviceProviderObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(serviceProviderJson);
            var providerDetailObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(providerDetailJson);
            string topicName = string.Empty;
            var topicObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(topicNameJson);
            foreach (var topic in topicObjects)
            {
                topicName = topic.topicName.ToString();
            }
            ServiceProvider serviceProvider = new ServiceProvider();
            foreach (var serviceProviderObject in serviceProviderObjects)
            {
                foreach (var site in serviceProviderObject.Sites)
                {                    
                    string siteId = site.ID;
                    string resourceType = Constants.ServiceProviderResourceType;
                    var topicTag = await GetServiceProviderTopicTagsAsync(topicName).ConfigureAwait(false);
                    serviceProvider = UpsertServiceProvider(site, topicTag);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(serviceProvider);
                    List<string> propertyNames = new List<string>() { Constants.SiteId, Constants.ResourceType };
                    List<string> values = new List<string>() { siteId, resourceType };
                    var serviceProviderDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values).ConfigureAwait(false);                    
                    if (serviceProviderDBData.Count == 0)
                    {
                        var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourcesCollectionId).ConfigureAwait(false);
                        resources.Add(result);
                    }
                    else
                    {
                        string id = serviceProviderDBData.id;
                        var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourcesCollectionId).ConfigureAwait(false);
                        resources.Add(result);
                    }
                }                
            }
            return resources;
        }

        /// <summary>
        /// upserts service provider
        /// </summary>
        public dynamic UpsertServiceProvider(dynamic site, dynamic topicTag)
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            Organization organizations = new Organization();
            List<Location> locations = new List<Location>();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            Availability availability = new Availability();
            AcceptanceCriteria acceptanceCriteria = new AcceptanceCriteria();
            OnboardingInfo onboardingInfo = new OnboardingInfo();
            SharedReferences sharedReferences = new SharedReferences();
            var address = GetServiceProviderAddress(site.Address);
            var phone = GetServiceProviderPhone(site.Phones);
            var topicTagData = topicTag;
            dynamic references = sharedReferences.GetReferences(site);
            locations = references[1];
            organizationReviewers = references[4];
            availability = references[6];
            acceptanceCriteria = references[7];
            onboardingInfo = references[8];     
            serviceProvider = new ServiceProvider()
            {
                SiteId = site.ID,
                Email = site.Email,
                Availability = availability,
                AcceptanceCriteria = acceptanceCriteria,
                OnboardingInfo = onboardingInfo,
                ResourceId = site.id == "" ? Guid.NewGuid() : site.id,
                Name = site.Name,
                ResourceCategory = site.resourceCategory,
                Description = site.description,
                ResourceType = Constants.ServiceProviderResourceType,
                Url = site.URL,
                TopicTags = topicTagData,
                OrganizationalUnit = site.organizationalUnit,
                Location = locations,
                CreatedBy = Constants.IntegrationAPI,
                ModifiedBy = Constants.IntegrationAPI,
                Address = address,
                Telephone = phone,
                Overview = site.overview,
                Specialties = site.specialties,
                EligibilityInformation = site.eligibilityInformation,
                Qualifications = site.qualifications,
                BusinessHours = site.businessHours,
                Reviewer = organizationReviewers
            };
            return serviceProvider;
        }

        /// <summary>
        /// Creates service provider address
        /// </summary>
        /// <param name="siteAddress"></param>
        /// <returns></returns>
        public dynamic GetServiceProviderAddress(dynamic siteAddress)
        {
            string serviceProviderAddresses = string.Empty;
            foreach(var address in siteAddress)
            {
                string fullAddress = FormAddress(address.Line1.ToString()) + FormAddress(address.Line2.ToString()) + FormAddress(address.Line3.ToString()) + FormAddress(address.City.ToString()) + FormAddress(address.State.ToString()) + FormAddress(address.Zip.ToString());
                fullAddress = fullAddress.Remove(fullAddress.Length - 2);
                serviceProviderAddresses = serviceProviderAddresses + fullAddress;
                serviceProviderAddresses = serviceProviderAddresses + " | ";                               
            }
            return serviceProviderAddresses.Remove(serviceProviderAddresses.Length - 3);
        }

        /// <summary>
        /// Forms full address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string FormAddress(string address)
        {
            string formedAddress = string.Empty;
            if (!string.IsNullOrEmpty(address))
            {
                formedAddress = address + ", ";
            }
            return formedAddress;
        }

        /// <summary>
        /// Retrieves phone numbers
        /// </summary>
        /// <param name="sitePhone"></param>
        /// <returns></returns>
        public static dynamic GetServiceProviderPhone(dynamic sitePhone)
        {
            string serviceProviderTelephones = string.Empty;
            dynamic telephone = null;
            foreach (var phone in sitePhone)
            {
                if (phone.Type == "st")
                {
                    telephone = phone.Phone;
                }
                serviceProviderTelephones = serviceProviderTelephones + telephone;
                serviceProviderTelephones = serviceProviderTelephones + " | ";
            }
            return serviceProviderTelephones.Remove(serviceProviderTelephones.Length - 3);
        }

        /// <summary>
        /// returns topic tags
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public async Task<dynamic> GetServiceProviderTopicTagsAsync(string topicName)
        {
            var topicDBData = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Name, topicName).ConfigureAwait(false);
            var id = topicDBData[0].id;
            List<TopicTag> topicTags = new List<TopicTag>();
            topicTags.Add(new TopicTag { TopicTags = id });
            return topicTags;
        }
    }          
}