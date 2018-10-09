using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public TopicsResourcesBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetResourcesAsync(dynamic topics)
        {
            var ids = new List<string>();
            foreach (var topic in topics)
            {
                string topicId = topic.id;
                ids.Add(topicId);
            }
            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, ids);
        }

        public async Task<dynamic> GetTopicsAsync(string keyword, Location location)
        {
            return await dbClient.FindItemsWhereContainsWithLocationAsync(dbSettings.TopicCollectionId, "keywords", keyword, location);
        }

        public async Task<dynamic> GetTopLevelTopicsAsync(Location location)
        {
            return await dbClient.FindItemsWhereWithLocationAsync(dbSettings.TopicCollectionId, Constants.ParentTopicId, "", location);
        }

        public async Task<dynamic> GetSubTopicsAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.TopicCollectionId, Constants.ParentTopicId, Constants.Id, topicInput.Id);
            }
            else
            {
                return await dbClient.FindItemsWhereArrayContainsAsyncWithLocation(dbSettings.TopicCollectionId, Constants.ParentTopicId, Constants.Id, topicInput.Id, topicInput.Location);
            }
        }

        public async Task<dynamic> GetResourceByIdAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.Id, topicInput.Id);
            }
            else
            {
                return await dbClient.FindItemsWhereWithLocationAsync(dbSettings.ResourceCollectionId, Constants.Id, topicInput.Id, topicInput.Location);
            }
        }

        public async Task<dynamic> GetResourceAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, topicInput.Id);
            }
            else
            {
                return await dbClient.FindItemsWhereArrayContainsAsyncWithLocation(dbSettings.ResourceCollectionId, Constants.TopicTags, Constants.Id, topicInput.Id, topicInput.Location);
            }
        }

        public async Task<dynamic> GetDocumentAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Id, topicInput.Id);
            }
            else
            {
                return await dbClient.FindItemsWhereWithLocationAsync(dbSettings.TopicCollectionId, Constants.Id, topicInput.Id, topicInput.Location);
            }
        }

        public async Task<dynamic> GetBreadcrumbDataAsync(string id)
        {
            List<dynamic> procedureParams = new List<dynamic>() { id };
            return await dbService.ExecuteStoredProcedureAsync(dbSettings.TopicCollectionId, Constants.BreadcrumbStoredProcedureName, procedureParams);
        }

        public async Task<dynamic> GetTopicDetailsAsync(string topicName)
        {
            var result = await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Name, topicName);
            return result;
        }

        public async Task<dynamic> GetResourceDetailAsync(string resourceName, string resourceType)
        {
            List<string> propertyNames = new List<string>() { Constants.Name, Constants.ResourceType };
            List<string> values = new List<string>() { resourceName, resourceType };
            var result = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
            return result;
        }

        public async Task<dynamic> GetPagedResourceAsync(ResourceFilter resourceFilter)
        {
            PagedResourceViewModel pagedResourceViewModel = new PagedResourceViewModel();
            if (resourceFilter.IsResourceCountRequired)
            {
                var groupedResourceType = await GetResourcesCountAsync(resourceFilter);
                pagedResourceViewModel.ResourceTypeFilter = JsonUtilities.DeserializeDynamicObject<dynamic>(groupedResourceType);
            }
            PagedResources pagedResources = await ApplyPaginationAsync(resourceFilter);
            dynamic serializedToken = pagedResources?.ContinuationToken ?? Constants.EmptyArray;
            pagedResourceViewModel.Resources = JsonUtilities.DeserializeDynamicObject<dynamic>(pagedResources?.Results);
            pagedResourceViewModel.ContinuationToken = JsonConvert.DeserializeObject(serializedToken);
            pagedResourceViewModel.TopicIds = JsonUtilities.DeserializeDynamicObject<dynamic>(pagedResources?.TopicIds);

            return JObject.FromObject(pagedResourceViewModel).ToString();
        }

        public async Task<dynamic> ApplyPaginationAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);
            return pagedResources;
        }

        public async Task<dynamic> GetResourcesCountAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, true);
            return ResourcesCount(pagedResources);
        }

        private dynamic ResourcesCount(PagedResources resources)
        {
            List<dynamic> allResources = new List<dynamic>
            {
                new
                {
                    ResourceName = "All",
                    ResourceCount = resources.Results.Count()
                }
            };
            var groupedResourceType = resources.Results.GroupBy(u => u.resourceType)
                  .OrderBy(group => group.Key)
                  .Select(n => new
                  {
                      ResourceName = n.Key,
                      ResourceCount = n.Count()
                  }).OrderBy(n => n.ResourceName);
            dynamic resourceList = allResources.Concat(groupedResourceType);
            return resourceList;
        }

        public dynamic GetReferences(dynamic resourceObject)
        {
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            List<Conditions> conditions = new List<Conditions>();
            List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
            List<QuickLinks> quickLinks = new List<QuickLinks>();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            List<dynamic> references = new List<dynamic>();
            foreach (JProperty field in resourceObject)
            {
                if (field.Name == "topicTags")
                {
                    topicTags = field.Value != null && field.Value.Count() > 0 ? GetTopicTags(field.Value) : null;
                }

                else if (field.Name == "location")
                {
                    locations = GetLocations(field.Value);
                }

                else if (field.Name == "conditions")
                {
                    conditions = field.Value != null && field.Value.Count() > 0 ? GetConditions(field.Value) : null;
                }

                else if (field.Name == "parentTopicId")
                {
                    parentTopicIds = field.Value != null && field.Value.Count() > 0 ? GetParentTopicIds(field.Value) : null;
                }

                else if (field.Name == "quickLinks")
                {
                    quickLinks = field.Value != null && field.Value.Count() > 0 ? GetQuickLinks(field.Value) : null;
                }

                else if (field.Name == "reviewer")
                {
                    organizationReviewers = field.Value != null && field.Value.Count() > 0 ? GetReviewer(field.Value) : null;
                }
            }

            references.Add(topicTags);
            references.Add(locations);
            references.Add(conditions);
            references.Add(parentTopicIds);
            references.Add(quickLinks);
            references.Add(organizationReviewers);
            return references;
        }

        public dynamic GetTopicTags(dynamic tagValues)
        {
            List<TopicTag> topicTags = new List<TopicTag>();
            foreach (var referenceTag in tagValues)
            {
                string id = string.Empty;
                foreach (JProperty tags in referenceTag)
                {
                    if (tags.Name == "id")
                    {
                        id = tags.Value.ToString();
                    }
                }
                topicTags.Add(new TopicTag { TopicTags = id });
            }
            return topicTags;
        }

        public dynamic GetLocations(dynamic locationValues)
        {
            List<Location> locations = new List<Location>();
            foreach (var loc in locationValues)
                {
                    string state = string.Empty, county = string.Empty, city = string.Empty, zipCode = string.Empty;
                    foreach (JProperty locs in loc)
                    {
                        if (locs.Name == "state")
                        {
                            state = locs.Value.ToString();
                        }
                        else if (locs.Name == "county")
                        {
                            county = locs.Value.ToString();
                        }
                        else if (locs.Name == "city")
                        {
                            city = locs.Value.ToString();
                        }
                        else if (locs.Name == "zipCode")
                        {
                            zipCode = locs.Value.ToString();
                        }
                    }
                    locations.Add(new Location { State = state, County = county, City = city, ZipCode = zipCode });
                }
            return locations;
        }

        public dynamic GetConditions(dynamic conditionsValues)
        {
            List<Conditions> conditions = new List<Conditions>();
            foreach (var conditon in conditionsValues)
                {
                    List<Condition> conditionData = new List<Condition>();
                    string title = string.Empty, description = string.Empty;
                    foreach (JProperty conditionJson in conditon)
                    {
                        if (conditionJson.Name == "condition")
                        {
                            var conditionDetails = conditionJson.Value;
                            foreach (JProperty conditionDetail in conditionDetails)
                            {
                                if (conditionDetail.Name == "title")
                                {
                                    title = conditionDetail.Value.ToString();
                                }
                                else if (conditionDetail.Name == "description")
                                {
                                    description = conditionDetail.Value.ToString();
                                }
                            }
                            conditionData.Add(new Condition { Title = title, ConditionDescription = description });
                        }
                    }
                    conditions.Add(new Conditions { ConditionDetail = conditionData });
                }
            return conditions;
        }

        public dynamic GetParentTopicIds(dynamic parentTopicIdValues)
        {
            List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
            foreach (var parentTopic in parentTopicIdValues)
            {
                string id = string.Empty;
                foreach (JProperty parentId in parentTopic)
                {
                    if (parentId.Name == "id")
                    {
                        id = parentId.Value.ToString();
                    }
                }
                parentTopicIds.Add(new ParentTopicId { ParentTopicIds = id });
            }
            return parentTopicIds;
        }

        public dynamic GetQuickLinks(dynamic quickLinksValues)
        {
            List<QuickLinks> quickLinks = new List<QuickLinks>();
            foreach (var quickLink in quickLinksValues)
            {
                string text = string.Empty;
                string url = string.Empty;
                foreach (JProperty quickLinkDetails in quickLink)
                {
                    if (quickLinkDetails.Name == "text")
                    {
                        text = quickLinkDetails.Value.ToString();
                    }

                    else if (quickLinkDetails.Name == "url")
                    {
                        url = quickLinkDetails.Value.ToString();
                    }
                }
                quickLinks.Add(new QuickLinks { Text = text, Urls = url });
            }
            return quickLinks;
        }

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

        public async Task<IEnumerable<object>> UpsertResourcesUploadAsync(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                dynamic resources = null;
                string json = r.ReadToEnd();
                var resourceObjects = JsonConvert.DeserializeObject<List<dynamic>>(json);
                resources = await UpsertResourceDocumentAsync(resourceObjects);
                return resources;
            }
        }

        public async Task<IEnumerable<object>> UpsertResourceDocumentAsync(dynamic resource)
        {
            List<dynamic> results = new List<dynamic>();
            List<dynamic> resources = new List<dynamic>();
            var resourceObjects = JsonUtilities.DeserializeDynamicObject<List<dynamic>>(resource);
            Form forms = new Form();
            ActionPlan actionPlans = new ActionPlan();
            Article articles = new Article();
            Video videos = new Video();
            Organization organizations = new Organization();
            EssentialReading essentialReadings = new EssentialReading();

            foreach (var resourceObject in resourceObjects)
            {
                string id = resourceObject.id;
                string resourceType = resourceObject.resourceType;
                if (resourceObject.resourceType == "Forms")
                {
                    forms = UpsertResourcesForms(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(forms);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
                    {
                        var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                    else
                    {
                        var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                }

                else if (resourceObject.resourceType == "Action Plans")
                {
                    actionPlans = UpsertResourcesActionPlans(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(actionPlans);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
                    {
                        var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                    else
                    {
                        var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                }

                else if (resourceObject.resourceType == "Articles")
                {
                    articles = UpsertResourcesArticles(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(articles);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
                    {
                        var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                    else
                    {
                        var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                }

                else if (resourceObject.resourceType == "Videos")
                {
                    videos = UpsertResourcesVideos(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(videos);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
                    {
                        var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                    else
                    {
                        var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                }

                else if (resourceObject.resourceType == "Organizations")
                {
                    organizations = UpsertResourcesOrganizations(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(organizations);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
                    {
                        var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                    else
                    {
                        var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                }

                else if (resourceObject.resourceType == "Related Links")
                {
                    essentialReadings = UpsertResourcesEssentialReadings(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(essentialReadings);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
                    {
                        var result = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                    else
                    {
                        var result = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourceCollectionId);
                        resources.Add(result);
                    }
                }

            }
            return resources;
        }

        public dynamic UpsertResourcesForms(dynamic resourceObject)
        {
            Form forms = new Form();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];

            forms = new Form()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                Icon = resourceObject.icon,
                Overview = resourceObject.overview,
                FullDescription = resourceObject.fullDescription,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            forms.Validate();
            return forms;
        }

        public dynamic UpsertResourcesActionPlans(dynamic resourceObject)
        {
            ActionPlan actionPlans = new ActionPlan();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            List<Conditions> conditions = new List<Conditions>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];
            conditions = references[2];

            actionPlans = new ActionPlan()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                Icon = resourceObject.icon,
                Conditions = conditions,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            actionPlans.Validate();
            return actionPlans;
        }

        public dynamic UpsertResourcesArticles(dynamic resourceObject)
        {
            Article articles = new Article();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];

            articles = new Article()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,
                Overview = resourceObject.overview,
                HeadLine1 = resourceObject.headline1,
                Content1 = resourceObject.content1,
                HeadLine2 = resourceObject.headline2,
                Content2 = resourceObject.content2
            };
            articles.Validate();
            return articles;
        }

        public dynamic UpsertResourcesVideos(dynamic resourceObject)
        {
            Video videos = new Video();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];

            videos = new Video()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,
                Overview = resourceObject.overview
            };
            videos.Validate();
            return videos;
        }

        public dynamic UpsertResourcesOrganizations(dynamic resourceObject)
        {
            Organization organizations = new Organization();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];
            organizationReviewers = references[5];            

            organizations = new Organization()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,                
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,                
                Address = resourceObject.address,
                Telephone = resourceObject.telephone,
                Overview = resourceObject.overview,
                EligibilityInformation = resourceObject.eligibilityInformation,
                Reviewer = organizationReviewers
            };
            organizations.Validate();
            return organizations;
        }

        public dynamic UpsertResourcesEssentialReadings(dynamic resourceObject)
        {
            EssentialReading essentialReadings = new EssentialReading();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];

            essentialReadings = new EssentialReading()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                ExternalUrls = resourceObject.externalUrl,
                Urls = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                Icon = resourceObject.icon,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            essentialReadings.Validate();
            return essentialReadings;
        }

        public async Task<IEnumerable<object>> UpsertTopicsUploadAsync(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                dynamic topics = null;
                string json = r.ReadToEnd();
                var topicObjects = JsonConvert.DeserializeObject<List<dynamic>>(json);
                topics = await UpsertTopicDocumentAsync(topicObjects);
                return topics;
            }
        }

        public async Task<IEnumerable<object>> UpsertTopicDocumentAsync(dynamic topic)
        {
            List<dynamic> results = new List<dynamic>();
            List<dynamic> topics = new List<dynamic>();
            var topicObjects = JsonUtilities.DeserializeDynamicObject<object>(topic);
            Topic topicdocuments = new Topic();            

            foreach (var topicObject in topicObjects)
            {
                string id = topicObject.id;
                topicdocuments = UpsertTopics(topicObject);
                var topicDocument = JsonUtilities.DeserializeDynamicObject<object>(topicdocuments);
                var topicDBData = await dbClient.FindItemsWhereAsync(dbSettings.TopicCollectionId, Constants.Id, id);

                if (topicDBData.Count == 0)
                {
                    var result = await dbService.CreateItemAsync(topicDocument, dbSettings.TopicCollectionId);
                    topics.Add(result);
                }
                else
                {
                    var result = await dbService.UpdateItemAsync(id, topicDocument, dbSettings.TopicCollectionId);
                    topics.Add(result);
                }                
            }
            return topics;
        }

        public dynamic UpsertTopics(dynamic topicObject)
        {
            Topic topics = new Topic();
            List<ParentTopicId> parentTopicIds = new List<ParentTopicId>();
            List<Location> locations = new List<Location>();
            List<QuickLinks> quickLinks = new List<QuickLinks>();
            dynamic references = GetReferences(topicObject);
            locations = references[1];
            parentTopicIds = references[3];
            quickLinks = references[4];

            topics = new Topic()
            {
                Id = topicObject.id == "" ? Guid.NewGuid() : topicObject.id,
                Name = topicObject.name,
                Overview = topicObject.overview,
                QuickLinks = quickLinks,
                ParentTopicId = parentTopicIds,
                ResourceType = topicObject.resourceType,
                Keywords = topicObject.keywords,
                OrganizationalUnit = topicObject.organizationalUnit,
                Location = locations,
                Icon = topicObject.icon,
                CreatedBy = topicObject.createdBy,
                ModifiedBy = topicObject.modifiedBy
            };
            topics.Validate();
            return topics;
        }

        public async Task<dynamic> GetPersonalizedResourcesAsync(ResourceFilter resourceFilter)
        {
            dynamic Topics = Array.Empty<string>();
            dynamic Resources = Array.Empty<string>();
            if (resourceFilter.TopicIds != null && resourceFilter.TopicIds.Count() > 0)
            {
                Topics = await dbClient.FindItemsWhereInClauseAsync(dbSettings.TopicCollectionId, "id", resourceFilter.TopicIds) ?? Array.Empty<string>();
            }
            if (resourceFilter.ResourceIds != null && resourceFilter.ResourceIds.Count() > 0)
            {
                Resources = await dbClient.FindItemsWhereInClauseAsync(dbSettings.ResourceCollectionId, "id", resourceFilter.ResourceIds) ?? Array.Empty<string>();
            }
            Topics = JsonConvert.SerializeObject(Topics);
            Resources = JsonConvert.SerializeObject(Resources);

            JObject personalizedResources = new JObject {
                { "topics", JsonConvert.DeserializeObject(Topics) },
                {"resources", JsonConvert.DeserializeObject(Resources) }
            };
            return personalizedResources.ToString();
        }

        public async Task<dynamic> GetOrganizationsAsync(Location location)
        {
            return await dbClient.FindItemsWhereContainsWithLocationAsync(dbSettings.ResourceCollectionId, Constants.ResourceType, Constants.Organization, location);
        }

        public async Task<dynamic> GetAllTopics()
        {
            return await dbClient.FindItemsAllAsync(dbSettings.TopicCollectionId);
        }
    }
}