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
using Newtonsoft.Json.Linq;
using System.Globalization;

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
        /// deletes service provider based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<dynamic> DeleteServiceProviderDocumentAsync(string id)
        {
            var response = await dbService.DeleteItemAsync(id, dbSettings.ResourcesCollectionId).ConfigureAwait(false);
            string message = string.Empty;
            if(response.ToString()== "NoContent")
            {
                message = "Record deleted successfully";
            }
            else
            {
                message = response.ToString();
            }
            return message;
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
            //var address = GetServiceProviderAddress(site.Address);
            //var phone = GetServiceProviderPhone(site.Phones);
            //var locations = GetServiceProviderLocation(site.Address);
            //var organizationUnit = GetServiceProviderOrgUnit(site.Address);
            dynamic references = GetServiceProviderReferences(site);
            organizationReviewers = references[0];
            availability = references[1];
            acceptanceCriteria = references[2];
            onboardingInfo = references[3];
            var address = references[4];
            var phone = references[5];
            var locations = references[6];
            var organizationUnit = references[7];
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
        /// returns service provider address
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
        /// returns formatted address
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
        /// returns organizational unit
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
        /// returns phone numbers
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

        /// <summary>
        /// returns reviewer details
        /// </summary>
        /// <param name="reviewerValues"></param>
        /// <returns></returns>
        public dynamic GetReviewer(dynamic reviewerValues)
        {
            List<OrganizationReviewer> organizationReviewer = new List<OrganizationReviewer>();            
            foreach (var reviewerDetails in reviewerValues)
            {
                string reviewerFullName = string.Empty, reviewerTitle = string.Empty, reviewText = string.Empty, reviewerImage = string.Empty;
                foreach (JProperty reviewer in reviewerDetails)
                {
                    if (reviewer.Name == "reviewerFullName")
                    {
                        reviewerFullName = reviewer.Value.ToString();
                    }
                    else if (reviewer.Name == "reviewerTitle")
                    {
                        reviewerTitle = reviewer.Value.ToString();
                    }
                    else if (reviewer.Name == "reviewText")
                    {
                        reviewText = reviewer.Value.ToString();
                    }
                    else if (reviewer.Name == "reviewerImage")
                    {
                        reviewerImage = reviewer.Value.ToString();
                    }
                }
                organizationReviewer.Add(new OrganizationReviewer { ReviewerFullName = reviewerFullName, ReviewerTitle = reviewerTitle, ReviewText = reviewText, ReviewerImage = reviewerImage });
            }
            return organizationReviewer;
        }

        /// <summary>
        /// returns availability details
        /// </summary>
        /// <param name="availabilityValues"></param>
        /// <returns></returns>
        public dynamic GetAvailability(dynamic availabilityValues)
        {
            Availability availability = new Availability();
            IEnumerable<Schedule> regularBusinessHours = null;
            IEnumerable<Schedule> holidayBusinessHours = null;
            var regularBusinessHoursData = availabilityValues.regularBusinessHours;
            regularBusinessHours = regularBusinessHoursData != null && regularBusinessHoursData.Count > 0 ? GetBusinessHours(regularBusinessHoursData) : null;
            holidayBusinessHours = availabilityValues.holidayBusinessHours != null && availabilityValues.holidayBusinessHours.Count > 0 ? GetBusinessHours(availabilityValues.holidayBusinessHours):null;
            TimeSpan waitTime = TimeSpan.Parse(availabilityValues.waitTime.ToString(), CultureInfo.InvariantCulture);
            availability = new Availability { RegularBusinessHours = regularBusinessHours, HolidayBusinessHours = holidayBusinessHours, WaitTime = waitTime };
            return availability;
        }

        /// <summary>
        /// returns open time and close time schedule
        /// </summary>
        /// <param name="businessHours"></param>
        /// <returns></returns>
        public dynamic GetBusinessHours(dynamic businessHours)
        {
            List<Schedule> schedules = new List<Schedule>();
            Schedule schedule = new Schedule();
            foreach (var businessHour in businessHours)
            {
                TimeSpan openTime = TimeSpan.Parse(businessHour.opensAt.ToString(), CultureInfo.InvariantCulture);
                TimeSpan closeTime = TimeSpan.Parse(businessHour.opensAt.ToString(), CultureInfo.InvariantCulture);
                schedule = (new Schedule { Day = businessHour.day, OpensAt = openTime, ClosesAt = closeTime });
                schedules.Add(schedule);
            }
            return schedules;
        }

        /// <summary>
        /// retruns acceptance criteria
        /// </summary>
        /// <param name="acceptanceCriteriaValues"></param>
        /// <returns></returns>
        public dynamic GetAcceptanceCriteria(dynamic acceptanceCriteriaValues)
        {
            AcceptanceCriteria acceptanceCriteria = new AcceptanceCriteria();
            string description = string.Empty;
            dynamic evaluatedRequirements = null;
            description = acceptanceCriteriaValues.description.ToString();
            evaluatedRequirements = acceptanceCriteriaValues.evaluatedRequirements != null && acceptanceCriteriaValues.evaluatedRequirements.Count > 0 ? GetEvaluatedRequirements(acceptanceCriteriaValues.evaluatedRequirements) : null;
            acceptanceCriteria = new AcceptanceCriteria { Description = description, EvaluatedRequirements = evaluatedRequirements };
            return acceptanceCriteria;
        }

        /// <summary>
        /// returns evaluated requirements
        /// </summary>
        /// <param name="evaluatedRequirements"></param>
        /// <returns></returns>
        public dynamic GetEvaluatedRequirements(dynamic evaluatedRequirements)
        {
            List<Expression> expressions = new List<Expression>();
            Expression expression = new Expression();
            Shared.Models.Integration.Condition condition = new Shared.Models.Integration.Condition();
            foreach (var evaluatedRequirement in evaluatedRequirements)
            {
                var displayLabel = evaluatedRequirement.condition.displayLabel;
                var data = evaluatedRequirement.condition.data;
                ConditionDataType dataType = evaluatedRequirement.condition.dataType;
                Operator operatorData = evaluatedRequirement.operatorName;
                string variableData = evaluatedRequirement.variable.ToString();
                condition = new Shared.Models.Integration.Condition { DisplayLabel = displayLabel, Data = data, DataType = dataType };
                expression = (new Expression { Condition = condition, Operator = operatorData, Variable = variableData });
                expressions.Add(expression);
            }
            return expressions;
        }

        /// <summary>
        /// returns onborading info
        /// </summary>
        /// <param name="onboardingInfoValues"></param>
        /// <returns></returns>
        public dynamic GetOnboardingInfo(dynamic onboardingInfoValues)
        {
            OnboardingInfo onboardingInfo = new OnboardingInfo();
            Shared.Models.Integration.Field field = new Shared.Models.Integration.Field();
            List<Shared.Models.Integration.Field> fields = new List<Shared.Models.Integration.Field>();
            var onboardingInfoData = onboardingInfoValues.userFields != null && onboardingInfoValues.userFields.Count > 0 ? onboardingInfoValues.userFields : null;
            foreach (var onboardingInfoValue in onboardingInfoData)
            {
                var name = onboardingInfoValue.name;
                var value = onboardingInfoValue.value;
                field = new Shared.Models.Integration.Field { Name = name, Value = value };
                fields.Add(field);
            }
            onboardingInfo = new OnboardingInfo { UserFields = fields };
            return onboardingInfo;
        }

        /// <summary>
        /// returns service provider references
        /// </summary>
        /// <param name="resourceObject"></param>
        /// <returns></returns>
        public dynamic GetServiceProviderReferences(dynamic resourceObject)
        {           
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            Availability availability = new Availability();
            AcceptanceCriteria acceptanceCriteria = new AcceptanceCriteria();
            OnboardingInfo onboardingInfo = new OnboardingInfo();
            List<dynamic> references = new List<dynamic>();
            var address = GetServiceProviderAddress(resourceObject.Address);
            var phone = GetServiceProviderPhone(resourceObject.Phones);
            var locations = GetServiceProviderLocation(resourceObject.Address);
            var organizationUnit = GetServiceProviderOrgUnit(resourceObject.Address);
            organizationReviewers = resourceObject.reviewer != null && resourceObject.reviewer.Count > 0 ? GetReviewer(resourceObject.reviewer) : null;
            availability = resourceObject.availability != null ? GetAvailability(resourceObject.availability) : null;
            acceptanceCriteria = resourceObject.acceptanceCriteria != null ? GetAcceptanceCriteria(resourceObject.acceptanceCriteria) : null;
            onboardingInfo = resourceObject.onboardingInfo != null ? GetOnboardingInfo(resourceObject.onboardingInfo) : null;
            references.Add(organizationReviewers);
            references.Add(availability);
            references.Add(acceptanceCriteria);
            references.Add(onboardingInfo);
            references.Add(address);
            references.Add(phone);
            references.Add(locations);
            references.Add(organizationUnit);
            return references;
        }
    }          
}