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
        public async Task<IEnumerable<object>> UpsertServiceProviderDocumentAsync(dynamic serviceProviderJson, dynamic providerDetailJson, dynamic topic)
        {
            List<dynamic> results = new List<dynamic>();
            List<dynamic> resources = new List<dynamic>();
            var serviceProviderObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(serviceProviderJson);
            var providerDetailObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(providerDetailJson);
            string topicName = string.Empty;
            topicName = topic.ToString();
            ServiceProvider serviceProvider = new ServiceProvider();            
            foreach (var serviceProviderObject in serviceProviderObjects)
            {
                foreach (var site in serviceProviderObject.Sites)
                {
                    string siteId = site.ID.ToString();
                    string resourceType = Constants.ServiceProviderResourceType;
                    List<string> propertyNames = new List<string>() { Constants.SiteId, Constants.ResourceType };
                    List<string> values = new List<string>() { siteId, resourceType };
                    var description = GetServiceProviderDescription(providerDetailObjects, siteId);
                    var topicDBData = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Name, topicName).ConfigureAwait(false);
                    var topicTag = GetServiceProviderTopicTags(topicDBData[0].id);
                    var serviceProviderDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values).ConfigureAwait(false);
                    string id = serviceProviderDBData.Count > 0 ? serviceProviderDBData[0].id.ToString() : string.Empty;
                    serviceProvider = UpsertServiceProvider(site, id, topicTag, description);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(serviceProvider);
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
            }
            return resources;
        }

        /// <summary>
        /// upserts service provider
        /// </summary>
        public dynamic UpsertServiceProvider(dynamic site, string id, dynamic topicTag, string description)
        {
            ServiceProvider serviceProvider = new ServiceProvider();
            Organization organizations = new Organization();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            Availability availability = new Availability();
            AcceptanceCriteria acceptanceCriteria = new AcceptanceCriteria();
            OnboardingInfo onboardingInfo = new OnboardingInfo();
            SharedReferences sharedReferences = new SharedReferences();
            var address = GetServiceProviderAddress(site.Address);
            var phone = GetServiceProviderPhone(site.Phones);
            var locations = GetServiceProviderLocation(site.Address);
            var organizationUnit = GetServiceProviderOrgUnit(site.Address);
            dynamic references = sharedReferences.GetReferences(site);
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
                ResourceId = string.IsNullOrEmpty(id)||string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id,
                Name = site.Name,
                ResourceCategory = site.resourceCategory,
                Description = description,
                ResourceType = Constants.ServiceProviderResourceType,
                Url = site.URL,
                TopicTags = topicTag,
                OrganizationalUnit = organizationUnit,
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
        /// return location
        /// </summary>
        /// <param name="siteAddress"></param>
        /// <returns></returns>
        public dynamic GetServiceProviderLocation(dynamic siteAddress)
        {
            string city, county, state, zip = string.Empty;
            List<Location> locations = new List<Location>();
            foreach (var address in siteAddress)
            {
                city = address.City.ToString();
                county = address.County.ToString();
                state = address.State.ToString();
                zip = address.Zip.ToString();
                Location location = new Location { State = state, County = county, City = city, ZipCode = zip };
                locations.Add(location);
            }
            return locations;
        }

        /// <summary>
        /// return organizational unit
        /// </summary>
        /// <param name="siteAddress"></param>
        /// <returns></returns>
        public static dynamic GetServiceProviderOrgUnit(dynamic siteAddress)
        {
            string serviceProviderOrgUnit = string.Empty;
            string orgUnit = string.Empty;
            foreach (var address in siteAddress)
            {
                orgUnit = address.State.ToString();
                serviceProviderOrgUnit = serviceProviderOrgUnit + orgUnit;
                serviceProviderOrgUnit = serviceProviderOrgUnit + " | ";
            }
            return serviceProviderOrgUnit.Remove(serviceProviderOrgUnit.Length - 3);
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

        /// <summary>
        /// return description
        /// </summary>
        /// <param name="providerDetailObjects"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static string GetServiceProviderDescription(dynamic providerDetailObjects, string siteId)
        {
            string description = string.Empty;
            foreach (var provider in providerDetailObjects)
            {
                if (siteId == provider.SiteKey.ToString())
                {
                    foreach (var detailText in provider.DetailText)
                    {
                        if (detailText.Label.ToString() == "SERVICE DESCRIPTION")
                        {
                            description = detailText.Text.ToString();
                            break;
                        }
                    }
                    break;
                }
            }
            return description;
        }
    }          
}