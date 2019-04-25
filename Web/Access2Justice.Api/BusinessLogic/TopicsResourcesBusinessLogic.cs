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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class TopicsResourcesBusinessLogic : ITopicsResourcesBusinessLogic
    {
        private readonly IStorageSettings _storageSettings;
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;

        private static readonly Regex IconLocationAbsoluteStoragePathRegEx =
            new Regex(@"^https:\/\/[a-zA-Z0-9\-\.]+\.blob\.core\.windows\.net\/static-resource\/([\w- .\/?%&=])+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public TopicsResourcesBusinessLogic(
            IStorageSettings storageSettings,
            IDynamicQueries dynamicQueries,
            ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService)
        {
            _storageSettings = storageSettings;
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<Topic> GetTopic(string topicName)
        {
            List<dynamic> topics = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Name, topicName);

            if (!topics.Any())
            {
                throw new Exception($"No topic found with this name: {topicName}");
            }

            return GetOutboundTopic(
                JsonUtilities.DeserializeDynamicObject<Topic>(topics.FirstOrDefault()));
        }

        public async Task<dynamic> GetTopicsAsync(string keyword, Location location)
        {
            dynamic topics = new List<dynamic>();

            var searchKeyword = string.IsNullOrWhiteSpace(keyword) ?
                keyword :
                keyword.ToLower();

            foreach (var searchLocation in LocationUtilities.GetSearchLocations(location))
            {
                topics = await dbClient.
                    FindItemsWhereContainsWithLocationAsync(
                        dbSettings.TopicsCollectionId,
                        "keywords",
                        searchKeyword,
                        searchLocation,
                        ignoreCase: true);

                if (topics.Count > 0)
                {
                    break;
                }
            }

            return GetOutboundTopicsCollection(topics);
        }

        public async Task<dynamic> GetTopicsAsync(string stateCode)
        {
            return GetOutboundTopicsCollection(
                await dbClient.FindItemsWhereArrayContainsAsync(
                    dbSettings.TopicsCollectionId, Constants.Location, Constants.StateCode, stateCode, true));
        }

        public async Task<dynamic> GetTopLevelTopicsAsync(Location location)
        {
            return GetOutboundTopicsCollection(
                await dbClient.FindItemsWhereWithLocationAsync(
                    dbSettings.TopicsCollectionId, Constants.ParentTopicId, "", location));
        }

        public async Task<dynamic> GetSubTopicsAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return GetOutboundTopicsCollection(
                    await dbClient.FindItemsWhereArrayContainsAsync(
                        dbSettings.TopicsCollectionId, Constants.ParentTopicId, Constants.Id, topicInput.Id));
            }
            else
            {
                return GetOutboundTopicsCollection(
                    await dbClient.FindItemsWhereArrayContainsAsyncWithLocation(
                        dbSettings.TopicsCollectionId, Constants.ParentTopicId, Constants.Id, topicInput.Id, topicInput.Location));
            }
        }

        public async Task<dynamic> GetResourceByIdAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, Constants.Id, topicInput.Id);
            }
            else
            {
                return await dbClient.FindItemsWhereWithLocationAsync(dbSettings.ResourcesCollectionId, Constants.Id, topicInput.Id, topicInput.Location);
            }
        }

        public async Task<dynamic> GetResourceAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourcesCollectionId, Constants.TopicTags, Constants.Id, topicInput.Id);
            }
            else
            {
                return await dbClient.FindItemsWhereArrayContainsAsyncWithLocation(dbSettings.ResourcesCollectionId, Constants.TopicTags, Constants.Id, topicInput.Id, topicInput.Location);
            }
        }

        public async Task<dynamic> GetResourcesAsync(dynamic topics)
        {
            var ids = new List<string>();
            foreach (var topic in topics)
            {
                string topicId = topic.id;
                ids.Add(topicId);
            }
            return await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourcesCollectionId, Constants.TopicTags, Constants.Id, ids);
        }

        public async Task<dynamic> GetDocumentAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                return GetOutboundTopicsCollection(
                    await dbClient.FindItemsWhereAsync(
                        dbSettings.TopicsCollectionId, Constants.Id, topicInput.Id));
            }
            else
            {
                return GetOutboundTopicsCollection(
                    await dbClient.FindItemsWhereWithLocationAsync(
                        dbSettings.TopicsCollectionId, Constants.Id, topicInput.Id, topicInput.Location));
            }
        }

        public async Task<dynamic> GetBreadcrumbDataAsync(string id)
        {
            List<dynamic> topics = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Id, id);

            if (!topics.Any())
            {
                throw new Exception($"No topic found with this id: {id}");
            }

            var topic = JsonUtilities.DeserializeDynamicObject<Topic>(topics.First());

            List<dynamic> procedureParams = new List<dynamic>() { id };
            return await dbService.ExecuteStoredProcedureAsync(
                dbSettings.TopicsCollectionId,
                Constants.BreadcrumbStoredProcedureName,
                topic.OrganizationalUnit,
                procedureParams);
        }

        public async Task<dynamic> GetTopicDetailsAsync(string topicName)
        {
            return GetOutboundTopicsCollection(
                await dbClient.FindItemsWhereAsync(
                    dbSettings.TopicsCollectionId, Constants.Name, topicName));
        }

        public async Task<dynamic> GetResourceDetailAsync(string resourceName, string resourceType)
        {
            List<string> propertyNames = new List<string>() { Constants.Name, Constants.ResourceType };
            List<string> values = new List<string>() { resourceName, resourceType };
            var result = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
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
            dynamic searchFilter = new JObject();
            searchFilter.OrderByField = resourceFilter.OrderByField;
            searchFilter.OrderBy = resourceFilter.OrderBy;
            PagedResources pagedResources = await ApplyPaginationAsync(resourceFilter);
            dynamic serializedToken = pagedResources?.ContinuationToken ?? Constants.EmptyArray;
            pagedResourceViewModel.Resources = JsonUtilities.DeserializeDynamicObject<dynamic>(pagedResources?.Results);
            pagedResourceViewModel.ContinuationToken = JsonConvert.DeserializeObject(serializedToken);
            pagedResourceViewModel.TopicIds = JsonUtilities.DeserializeDynamicObject<dynamic>(pagedResources?.TopicIds);
            pagedResourceViewModel.SearchFilter = searchFilter;
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
            List<OrganizationReviewer> organizationReviewers = new List<OrganizationReviewer>();
            List<ArticleContent> articleContents = new List<ArticleContent>();
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

                else if (field.Name == "reviewer")
                {
                    organizationReviewers = field.Value != null && field.Value.Count() > 0 ? GetReviewer(field.Value) : null;
                }

                else if (field.Name == "contents")
                {
                    articleContents = field.Value != null && field.Value.Count() > 0 ? GetContents(field.Value) : null;
                }
            }

            references.Add(topicTags);
            references.Add(locations);
            references.Add(conditions);
            references.Add(parentTopicIds);
            references.Add(organizationReviewers);
            references.Add(articleContents);
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

        public dynamic GetContents(dynamic contentValues)
        {
            List<ArticleContent> articleContents = new List<ArticleContent>();
            foreach (var contentDetails in contentValues)
            {
                string headline = string.Empty, content = string.Empty;
                foreach (JProperty contentData in contentDetails)
                {
                    if (contentData.Name == "headline")
                    {
                        headline = contentData.Value.ToString();
                    }
                    else if (contentData.Name == "content")
                    {
                        content = contentData.Value.ToString();
                    }
                }
                articleContents.Add(new ArticleContent { Headline = headline, Content = content });
            }
            return articleContents;
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
            AdditionalReading additionalReadings = new AdditionalReading();
            RelatedLink relatedLinks = new RelatedLink();

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
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
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

                else if (resourceObject.resourceType == "Action Plans")
                {
                    actionPlans = UpsertResourcesActionPlans(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(actionPlans);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
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

                else if (resourceObject.resourceType == "Articles")
                {
                    articles = UpsertResourcesArticles(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(articles);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
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

                else if (resourceObject.resourceType == "Videos")
                {
                    videos = UpsertResourcesVideos(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(videos);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
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

                else if (resourceObject.resourceType == "Organizations")
                {
                    organizations = UpsertResourcesOrganizations(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(organizations);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
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

                else if (resourceObject.resourceType == "Additional Readings")
                {
                    additionalReadings = UpsertResourcesAdditionalReadings(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(additionalReadings);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
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

                else if (resourceObject.resourceType == "Related Links")
                {
                    relatedLinks = UpsertResourcesRelatedLinks(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(relatedLinks);
                    List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
                    List<string> values = new List<string>() { id, resourceType };
                    var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
                    if (resourceDBData.Count == 0)
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
                Url = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
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
                Url = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
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
            List<ArticleContent> articleContents = new List<ArticleContent>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];
            articleContents = references[5];

            articles = new Article()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                Url = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,
                Overview = resourceObject.overview,
                Contents = articleContents
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
                Url = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
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
            organizationReviewers = references[4];

            organizations = new Organization()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                Url = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy,
                Address = resourceObject.address,
                Telephone = resourceObject.telephone,
                Overview = resourceObject.overview,
                Specialties = resourceObject.specialties,
                EligibilityInformation = resourceObject.eligibilityInformation,
                Qualifications = resourceObject.qualifications,
                BusinessHours = resourceObject.businessHours,
                Reviewer = organizationReviewers
            };
            organizations.Validate();
            return organizations;
        }

        public dynamic UpsertResourcesAdditionalReadings(dynamic resourceObject)
        {
            AdditionalReading additionalReadings = new AdditionalReading();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];

            additionalReadings = new AdditionalReading()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                Url = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            additionalReadings.Validate();
            return additionalReadings;
        }

        public dynamic UpsertResourcesRelatedLinks(dynamic resourceObject)
        {
            RelatedLink relatedLink = new RelatedLink();
            List<TopicTag> topicTags = new List<TopicTag>();
            List<Location> locations = new List<Location>();
            dynamic references = GetReferences(resourceObject);
            topicTags = references[0];
            locations = references[1];

            relatedLink = new RelatedLink()
            {
                ResourceId = resourceObject.id == "" ? Guid.NewGuid() : resourceObject.id,
                Name = resourceObject.name,
                ResourceCategory = resourceObject.resourceCategory,
                Description = resourceObject.description,
                ResourceType = resourceObject.resourceType,
                Url = resourceObject.url,
                TopicTags = topicTags,
                OrganizationalUnit = resourceObject.organizationalUnit,
                Location = locations,
                CreatedBy = resourceObject.createdBy,
                ModifiedBy = resourceObject.modifiedBy
            };
            relatedLink.Validate();
            return relatedLink;
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
            //TODO: fix icon location when saving new item
            List<dynamic> results = new List<dynamic>();
            List<dynamic> topics = new List<dynamic>();
            var topicObjects = JsonUtilities.DeserializeDynamicObject<object>(topic);
            Topic topicdocuments = new Topic();

            foreach (var topicObject in topicObjects)
            {
                string id = topicObject.id;
                topicdocuments = UpsertTopics(topicObject);
                var topicDocument = JsonUtilities.DeserializeDynamicObject<object>(topicdocuments);
                var topicDBData = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Id, id);

                if (topicDBData.Count == 0)
                {
                    var result = await dbService.CreateItemAsync(topicDocument, dbSettings.TopicsCollectionId);
                    topics.Add(result);
                }
                else
                {
                    var result = await dbService.UpdateItemAsync(id, topicDocument, dbSettings.TopicsCollectionId);
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
            dynamic references = GetReferences(topicObject);
            locations = references[1];
            parentTopicIds = references[3];

            topics = new Topic()
            {
                Id = topicObject.id == "" ? Guid.NewGuid() : topicObject.id,
                Name = topicObject.name,
                Overview = topicObject.overview,
                ParentTopicId = parentTopicIds,
                ResourceType = topicObject.resourceType,
                Keywords = topicObject.keywords,
                OrganizationalUnit = topicObject.organizationalUnit,
                Location = locations,
                Icon = GetRelativeStaticResourceStoragePath((string)topicObject.icon),
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
                Topics = GetOutboundTopicsCollection(
                    await dbClient.FindItemsWhereInClauseAsync(
                        dbSettings.TopicsCollectionId, "id", resourceFilter.TopicIds) ?? Array.Empty<string>());
            }

            if (resourceFilter.ResourceIds != null && resourceFilter.ResourceIds.Count() > 0)
            {
                Resources = await dbClient.FindItemsWhereInClauseAsync(dbSettings.ResourcesCollectionId, "id", resourceFilter.ResourceIds);

                var resourcesList = Resources as List<dynamic>;

                if (resourcesList == null)
                {
                    resourcesList = new List<dynamic>();
                    Resources = resourcesList;
                }

                if (resourcesList.Count != resourceFilter.ResourceIds.Count())
                {
                    var sharedPlansList = await dbClient.
                        FindItemsWhereInClauseAsync(dbSettings.ActionPlansCollectionId, "id", resourceFilter.ResourceIds) as List<dynamic>;

                    if (sharedPlansList != null &&
                        sharedPlansList.Count > 0)
                    {
                        resourcesList.AddRange(sharedPlansList);
                    }
                }

                if (Resources == null)
                {
                    Resources = Array.Empty<string>();
                }
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
            return await dbClient.FindItemsWhereContainsWithLocationAsync(dbSettings.ResourcesCollectionId, Constants.ResourceType, Constants.Organization, location);
        }

        public async Task<dynamic> GetAllTopics()
        {
            return GetOutboundTopicsCollection(
                await dbClient.FindItemsAllAsync(dbSettings.TopicsCollectionId));
        }

        public async Task<dynamic> GetTopicDetailsAsync(IntentInput intentInput)
        {
            return GetOutboundTopicsCollection(
                await dbClient.FindItemsWhereInClauseAsync(
                    dbSettings.TopicsCollectionId, Constants.Name, intentInput.Intents, intentInput.Location));
        }

        private dynamic GetOutboundTopicsCollection(dynamic topics)
        {
            if (topics != null)
            {
                var listOfDynamic = topics as List<dynamic>;

                if (listOfDynamic != null &&
                    listOfDynamic.Any())
                {
                    var preparedTopics = new List<dynamic>();

                    foreach(var rawTopic in listOfDynamic)
                    {
                        preparedTopics.Add(
                            GetOutboundTopic(
                                JsonUtilities.DeserializeDynamicObject<Topic>(rawTopic)));
                    }

                    topics = preparedTopics;
                }
            }

            return topics;
        }

        private Topic GetOutboundTopic(Topic topic)
        {
            if (topic != null)
            {
                topic.Icon = GetAbsoluteStaticResourceStoragePath(topic.Icon);
            }

            return topic;
        }

        public string GetAbsoluteStaticResourceStoragePath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (path.IndexOf('|') > 0)
                {
                    var locations = path.Split('|', StringSplitOptions.RemoveEmptyEntries);

                    return string.Join(
                        " | ",
                        locations.
                            Where(l => !string.IsNullOrEmpty(l)).
                            Select(l => GetAbsoluteStaticResourceStoragePath(l.Trim())));
                }
                else if (path.StartsWith("/static-resource", StringComparison.InvariantCultureIgnoreCase))
                {
                    return _storageSettings.StaticResourcesRootUrl + path;
                }
            }

            return path;
        }

        public string GetRelativeStaticResourceStoragePath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (path.IndexOf('|') > 0)
                {
                    return string.Join(
                        " | ",
                        path.
                            Split('|', StringSplitOptions.RemoveEmptyEntries).
                            Where(l => !string.IsNullOrWhiteSpace(l)).
                            Select(l => GetRelativeStaticResourceStoragePath(l.Trim())));
                }
                else
                {
                    if (IconLocationAbsoluteStoragePathRegEx.IsMatch(path))
                    {
                        return path.Substring(
                            path.IndexOf(
                                "/static-resource",
                                StringComparison.InvariantCultureIgnoreCase));
                    }
                }
            }

            return path;
        }
    }
}