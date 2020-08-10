using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Condition = Access2Justice.Shared.Models.Condition;
//using Microsoft.Azure.Documents;

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

        public async Task<Topic> GetTopic(string topicName, string state = null)
        {
            //topicName = topicName.Replace("’", "'");
            List<dynamic> topics = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Name, topicName);
            Topic topic;
            if (!topics.Any())
            {
                throw new Exception($"No topic found with this name: {topicName}");
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                topic = FilterByDeleteAndOrderByRanking<Topic>(topics).FirstOrDefault(x => x.Location[0].State == state);
            }
            else
            {
                topic = FilterByDeleteAndOrderByRanking<Topic>(topics).FirstOrDefault();
            }

            return GetOutboundTopic(topic);
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

            List<Topic> filteredTopics = FilterByDeleteAndOrderByRanking<Topic>(topics);

            return filteredTopics;
        }

        public async Task<dynamic> GetTopicsAsync(string stateCode)
        {
            var foundTopics = await dbClient.FindItemsWhereArrayContainsAsync(
                dbSettings.TopicsCollectionId, Constants.Location, Constants.StateCode, stateCode, true);

            var filteredTopics = FilterByDeleteAndOrderByRanking<Topic>(foundTopics);

            return GetOutboundTopicsCollection(filteredTopics);
        }

        public async Task<List<Resource>> GetResourcesWithLocationAsync(Location location)
        {
            var properLocation = GetProperLocation(location);
            var foundResources = await dbClient.FindResourcesWithLocationAsync(
                dbSettings.ResourcesCollectionId, properLocation);

            List<Resource> listResources = FilterByDeleteAndOrderByRanking<Resource>(foundResources);
            //listResources = listResources.OrderBy(x=>x.Ranking)
            if (!listResources.Any())
            {
                if (!IsValidLocation(location))
                {
                    return await GetResourcesWithLocationAsync(location);
                }
            }

            return listResources;
        }

        public async Task<List<dynamic>> GetSpecificCollectionItem(TopicInput topicInput)
        {
            List<dynamic> rawItems = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.ResourcesCollectionId, Constants.Id, topicInput.Id, topicInput.Location);

            if (!rawItems.Any())
            {
                if (!IsValidLocation(topicInput.Location))
                {
                    return await GetSpecificCollectionItem(topicInput);
                }
            }

            return rawItems;
        }

        public async Task<List<Topic>> GetItemsWithLocationAsync(TopicInput topicInput)
        {
            var rawItems = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.ResourcesCollectionId, Constants.Id, topicInput.Id, topicInput.Location);

            List<Topic> listItems = FilterByDeleteAndOrderByRanking<Topic>(rawItems);

            if (!listItems.Any())
            {
                if (!IsValidLocation(topicInput.Location))
                {
                    return await GetItemsWithLocationAsync(topicInput);
                }
            }

            return listItems;
        }


        public async Task<dynamic> FindAllResources(ResourceFilter resourceFilter)
        {

            var parameters = new Dictionary<string, object>();
            var query = dbClient.BuildQueryWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, parameters);
            List<dynamic> items = await dbService.QueryItemsAsync(dbSettings.ResourcesCollectionId, query, parameters);

            if (!items.Any() && resourceFilter.Location != null)
            {
                if (!IsValidLocation(resourceFilter.Location))
                {
                    return await FindAllResources(resourceFilter);
                }
            }

            return items;
        }

        public async Task<dynamic> FindAllResourcesForDownload(ResourceFilter resourceFilter)
        {

            var parameters = new Dictionary<string, object>();
            var query = dbClient.BuildQueryWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, parameters);
            List<dynamic> items = await dbService.QueryItemsAsync(dbSettings.ResourcesCollectionId, query, parameters);

            if (!items.Any() && resourceFilter.Location != null)
            {
                if (!IsValidLocation(resourceFilter.Location))
                {
                    return await FindAllResources(resourceFilter);
                }
            }

            var listResources = FilterByDeleteAndOrderByRanking<Resource>(items);

            var tt = listResources.OrderBy(x => x.Name).ToList();

            return GetOutboundTopicsCollection(tt);
        }

        private async Task GetParentSubTopic(List<string> topicIds, List<Topic> resultTopics, string parentTopicId)
        {
            List<string> intermediateTopicIds = new List<string>();
            List<dynamic> topics = await dbClient.FindItemsWhereInClauseAsync(dbSettings.TopicsCollectionId, Constants.Id, topicIds);
            var topicsStronglyTyped = topics.Select(x => CastDynamicTo(x)).Cast<Topic>();
            foreach (var topic in topicsStronglyTyped)
            {
                if (topic.ParentTopicId == null)
                {
                    continue;
                }
                var listParentTopicIds = topic.ParentTopicId.Select(x => (string)x.ParentTopicIds).ToList();
                if (listParentTopicIds.Contains(parentTopicId))
                {
                    resultTopics.Add(topic);
                }
                else
                {
                    intermediateTopicIds.AddRange(listParentTopicIds);
                }
            }

            if (intermediateTopicIds.Any())
            {
                await GetParentSubTopic(intermediateTopicIds, resultTopics, parentTopicId);
            }
        }

        private async Task GetParentTopics(List<string> topicIds, List<dynamic> parentTopics)
        {
            List<string> intermediateTopicIds = new List<string>();
            List<dynamic> topics = await dbClient.FindItemsWhereInClauseAsync(dbSettings.TopicsCollectionId, Constants.Id, topicIds);
            foreach (var topic in topics)
            {
                if (topic.parentTopicId == null)
                {
                    if (!parentTopics.Any(x => x.id == topic.id))
                    {
                        parentTopics.Add(topic);
                    }
                }
                else
                {
                    var dynamicResponse = JsonConvert.SerializeObject(topic);
                    var topicResult = (JObject)JsonConvert.DeserializeObject(dynamicResponse);
                    if (topicResult != null)
                    {
                        var parentTopicIds = topicResult["parentTopicId"].ToList();
                        foreach (var topicId in parentTopicIds)
                        {
                            var id = topicId["id"].ToString();
                            intermediateTopicIds.Add(id);
                        }
                    }
                }
            }
            if (intermediateTopicIds.Any())
            {
                await GetParentTopics(intermediateTopicIds, parentTopics);
            }
        }

        public async Task<dynamic> GetTopLevelTopicsAsync(Location location)
        {
            var resources = await GetResourcesWithLocationAsync(location);

            List<dynamic> parentTopics = new List<dynamic>();
            List<string> topicIds = resources.SelectMany(x => x.TopicTags).Select(x => (string)x.TopicTags).Distinct().ToList();
            if (topicIds.Any())
            {
                await GetParentTopics(topicIds, parentTopics);
            }

            var filteredTopics = FilterByDeleteAndOrderByRanking<Topic>(parentTopics.ToList());

            filteredTopics = filteredTopics.Where(x => x.Location.Any(y => y.State == location.State)).ToList();
            return GetOutboundTopicsCollection(filteredTopics);
        }

        private Topic CastDynamicTo(dynamic obj)
        {
            var dynamicResponse = JsonConvert.SerializeObject(obj);
            var topicResult = (JObject)JsonConvert.DeserializeObject(dynamicResponse);
            var topicResult2 = topicResult.ToObject<Topic>();
            return topicResult2;
        }
        private async Task<List<Topic>> GetSubtopicsByParentTopic(List<string> topicIds, dynamic parentTopicId)
        {
            var resultTopics = new List<Topic>();
            List<dynamic> topics = await dbClient.FindItemsWhereInClauseAsync(dbSettings.TopicsCollectionId, Constants.Id, topicIds);
            IEnumerable<Topic> topicsStronglyTyped = topics.Select(x => CastDynamicTo(x)).Cast<Topic>();

            await GetParentSubTopic(topicIds, resultTopics, parentTopicId);

            List<Topic> resources = await GetResourcesByTopicIdsAsync(new List<string> { parentTopicId });
            if (resources.Any())
            {
                return new List<Topic>();
            }

            return resultTopics;
        }

        public async Task<dynamic> GetSubTopicsAsync(TopicInput topicInput)
        {
            List<Topic> result;
            List<dynamic> rawResult;

            if (topicInput.IsShared)
            {
                rawResult = await dbClient.FindItemsWhereArrayContainsAsync(
                    dbSettings.TopicsCollectionId, Constants.ParentTopicId, Constants.Id, topicInput.Id);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(topicInput.Location.County) && string.IsNullOrWhiteSpace(topicInput.Location.City)
                    && string.IsNullOrWhiteSpace(topicInput.Location.ZipCode))
                {
                    rawResult = await GetSubtopicsFromLocation(topicInput);
                }
                else
                {
                    var resources = await GetResourcesWithLocationAsync(topicInput.Location);
                    var topicIdsFromResources = resources.SelectMany(x => x.TopicTags).Select(x => (string)x.TopicTags).Distinct().ToList();
                    var res = await GetSubtopicsByParentTopic(topicIdsFromResources, topicInput.Id);
                    return GetOutboundTopicsCollection(res);
                }
            }


            result = FilterByDeleteAndOrderByRanking<Topic>(rawResult);

            List<string> topicIds = result.Select(x => (string)x.Id).ToList();

            if (topicIds.Any())
            {
                List<Topic> topicsToRemove = new List<Topic>();
                List<Topic> topicsWithoutResources = new List<Topic>();
                List<dynamic> resources = await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourcesCollectionId, Constants.TopicTags, Constants.Id, topicIds);

                var resourcesStronglyTyped = FilterByDeleteAndOrderByRanking<Resource>(resources);
                foreach (var item in result)
                {
                    if (!resourcesStronglyTyped.Any(x => x.TopicTags.Any(y => (string)y.TopicTags == (string)item.Id)))
                    {
                        item.Display = "No";
                        topicsWithoutResources.Add(item);
                    }
                }

                foreach (var item in topicsWithoutResources)
                {
                    var parentTopic = await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.TopicsCollectionId, Constants.ParentTopicId, Constants.Id, item.Id);
                    if (parentTopic.Count == 0)
                    {
                        topicsToRemove.Add(item);
                    }
                }

                result = result.Except(topicsToRemove).ToList();
            }

            return GetOutboundTopicsCollection(result);
        }

        private async Task<dynamic> GetSubtopicsFromLocation(TopicInput topicInput)
        {
            List<dynamic> rawResult = await dbClient.FindItemsWhereArrayContainsAsyncWithLocation(
                    dbSettings.TopicsCollectionId, Constants.ParentTopicId, Constants.Id, topicInput.Id, topicInput.Location);

            if (!rawResult.Any())
            {
                if (!IsValidLocation(topicInput))
                {
                    return await GetSubtopicsFromLocation(topicInput);
                }
            }

            return rawResult;
        }

        private async Task<dynamic> GetTopicsByLocation(TopicInput topicInput)
        {
            var location = GetProperLocation(topicInput);
            List<dynamic> topics = await dbClient.FindItemsWhereWithLocationAsync(
                dbSettings.TopicsCollectionId, Constants.Id, topicInput.Id, location);

            if (!topics.Any())
            {
                if (!IsValidLocation(topicInput))
                {
                    return await GetTopicsByLocation(topicInput);
                }
            }

            return topics;
        }


        private Location GetProperLocation(TopicInput topicInput)
        {
            Location location;

            if (!string.IsNullOrWhiteSpace(topicInput.Location.ZipCode))
            {
                location = new Location
                {
                    ZipCode = topicInput.Location.ZipCode
                };
            }
            else if (!string.IsNullOrWhiteSpace(topicInput.Location.City))
            {
                location = new Location
                {
                    City = topicInput.Location.City
                };
            }
            else if (!string.IsNullOrWhiteSpace(topicInput.Location.County))
            {
                if (topicInput.Location.County.Contains("County"))
                {
                    topicInput.Location.County = topicInput.Location.County.Replace("County", string.Empty).Trim();
                }
                location = new Location
                {
                    County = topicInput.Location.County
                };
            }
            else
            {
                location = new Location
                {
                    State = topicInput.Location.State
                };
            }

            return location;
        }

        private Location GetProperLocation(Location inputLocation)
        {
            Location location;

            if (!string.IsNullOrWhiteSpace(inputLocation.ZipCode))
            {
                location = new Location
                {
                    ZipCode = inputLocation.ZipCode
                };
            }
            else if (!string.IsNullOrWhiteSpace(inputLocation.City))
            {
                location = new Location
                {
                    City = inputLocation.City
                };
            }
            else if (!string.IsNullOrWhiteSpace(inputLocation.County))
            {
                if (inputLocation.County.Contains("County"))
                {
                    inputLocation.County = inputLocation.County.Replace("County", string.Empty).Trim();
                }
                location = new Location
                {
                    County = inputLocation.County
                };
            }
            else
            {
                location = new Location
                {
                    State = inputLocation.State
                };
            }

            return location;
        }

        private Location GetProperLocation(ResourceFilter resFilter)
        {
            Location location;

            if (!string.IsNullOrWhiteSpace(resFilter.Location.ZipCode))
            {
                location = new Location
                {
                    ZipCode = resFilter.Location.ZipCode
                };
            }
            else if (!string.IsNullOrWhiteSpace(resFilter.Location.City))
            {
                location = new Location
                {
                    City = resFilter.Location.City
                };
            }

            else if (!string.IsNullOrWhiteSpace(resFilter.Location.County))
            {
                if (resFilter.Location.County.Contains("County"))
                {
                    resFilter.Location.County = resFilter.Location.County.Replace("County", string.Empty).Trim();
                }
                location = new Location
                {
                    County = resFilter.Location.County
                };
            }
            else
            {
                location = new Location
                {
                    State = resFilter.Location.State
                };
            }

            return location;
        }

        private async Task<dynamic> GetResourcesFromLocation(TopicInput topicInput)
        {
            // Workaround
            var location = GetProperLocation(topicInput);

            List<dynamic> rawItems = await dbClient.FindItemsWhereArrayContainsAsyncWithLocation(dbSettings.ResourcesCollectionId,
                Constants.TopicTags, Constants.Id, topicInput.Id, location);

            if (!rawItems.Any())
            {
                if (!IsValidLocation(topicInput))
                {
                    return await GetResourcesFromLocation(topicInput);
                }
            }

            return rawItems;
        }

        private bool IsValidLocation(TopicInput topicInput)
        {
            if (!string.IsNullOrWhiteSpace(topicInput.Location.ZipCode))
            {
                topicInput.Location.ZipCode = null;
            }
            else if (!string.IsNullOrWhiteSpace(topicInput.Location.City))
            {
                topicInput.Location.City = null;
            }
            else if (!string.IsNullOrWhiteSpace(topicInput.Location.County))
            {
                topicInput.Location.County = null;
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool IsValidLocation(Location location)
        {
            if (!string.IsNullOrWhiteSpace(location.ZipCode))
            {
                location.ZipCode = null;
            }
            else if (!string.IsNullOrWhiteSpace(location.City))
            {
                location.City = null;
            }
            else if (!string.IsNullOrWhiteSpace(location.County))
            {
                location.County = null;
            }
            else
            {
                return true;
            }

            return false;
        }

        public async Task<dynamic> GetResourceByIdAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                var rawItems = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, Constants.Id, topicInput.Id);
                return rawItems;
            }
            else
            {
                var rawItems = await GetSpecificCollectionItem(topicInput);
                return rawItems;
            }
        }

        public async Task<dynamic> GetResourceAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                var rawItems = await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourcesCollectionId, Constants.TopicTags, Constants.Id, topicInput.Id);
                var items = FilterByDeleteAndOrderByRanking<Topic>(rawItems);
                return items;
            }
            else
            {
                List<dynamic> rawItems = await GetResourcesFromLocation(topicInput);

                var items = FilterByDeleteAndOrderByRanking<dynamic>(rawItems, topicInput.Name);
                return items;
            }
        }

        public async Task<dynamic> GetResourceForCuratedExperienceAsync(TopicInput topicInput)
        {
            if (topicInput.IsShared)
            {
                var rawItems = await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourcesCollectionId, Constants.TopicTags, Constants.Id, topicInput.Id);
                var items = FilterByDeleteAndOrderByRanking<Topic>(rawItems);
                return items;
            }
            else
            {
                List<dynamic> rawItems = await GetResourcesFromLocation(topicInput);
                var items = FilterByDeleteAndOrderByRanking<Topic>(rawItems, topicInput.Name);
                return items;
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

            var rawTopic = await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourcesCollectionId,
                                            Constants.TopicTags, Constants.Id, ids);
            var realTopics = FilterByDeleteAndOrderByRanking<Topic>(rawTopic);
            return realTopics;
        }

        public async Task<List<Topic>> GetResourcesByTopicIdsAsync(List<string> topicIds)
        {
            var rawTopic = await dbClient.FindItemsWhereArrayContainsAsync(dbSettings.ResourcesCollectionId,
                                            Constants.TopicTags, Constants.Id, topicIds);
            List<Topic> resources = FilterByDeleteAndOrderByRanking<Topic>(rawTopic);
            return resources;
        }

        public async Task<dynamic> GetDocumentAsync(TopicInput topicInput)
        {
            dynamic topics;
            if (topicInput.IsShared)
            {

                topics = await dbClient.FindItemsWhereAsync(
                    dbSettings.TopicsCollectionId, Constants.Id, topicInput.Id);
            }
            else
            {

                topics = await GetTopicsByLocation(topicInput);
            }

            var filteredTopicsCollection = FilterByDeleteAndOrderByRanking<Topic>(topics);
            var topicsCollection = GetOutboundTopicsCollection(filteredTopicsCollection);
            var processedTopics = await fillCuratedExperienceId(topicInput.Location, topicsCollection);
            if (processedTopics != null && processedTopics.Count != 0)
            {
                topics = processedTopics;
            }

            return topics;
        }

        private async Task<List<dynamic>> fillCuratedExperienceId(Location location, IEnumerable<dynamic> topicsCollection)
        {
            var realTopics = (topicsCollection ?? throw new InvalidOperationException()).Where(x => x is Topic).ToList();
            realTopics = realTopics
                .Select(x =>
                    (dynamic)new TopicView(JsonUtilities.DeserializeDynamicObject<Topic>(x))
                ).ToList();

            if (realTopics.Count != 0)
            {
                var curatedExperienceId = await findGuidedAssistantId(realTopics.Select(x => (string)x.Id).ToList(), location);
                foreach (var topic in realTopics)
                {
                    topic.CuratedExperienceId = curatedExperienceId;
                }
            }
            return realTopics;
        }

        private async Task<Guid> findGuidedAssistantId(IEnumerable<string> topics, Location location = null)
        {
            var filter = new ResourceFilter
            {
                TopicIds = topics,
                PageNumber = 0,
                ResourceType = Constants.GuidedAssistant,
                Location = location
            };

            List<dynamic> result = await FindAllResources(filter);
            if (result != null && result.Count != 0)
            {
                GuidedAssistant guidedAssistant =
                    JsonUtilities.DeserializeDynamicObject<GuidedAssistant>(result.FirstOrDefault());
                return Guid.Parse(guidedAssistant.CuratedExperienceId);
            }
            return Guid.Empty;
        }

        public async Task<dynamic> GetBreadcrumbDataAsync(string id)
        {
            List<dynamic> topics = await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Id, id);

            var filteredTopic = FilterByDeleteAndOrderByRanking<Topic>(topics);
            if (!filteredTopic.Any())
            {
                throw new Exception($"No topic found with this id: {id}");
            }

            var topic = filteredTopic.FirstOrDefault();

            List<dynamic> procedureParams = new List<dynamic>() { id };
            return await dbService.ExecuteStoredProcedureAsync(
                dbSettings.TopicsCollectionId,
                Constants.BreadcrumbStoredProcedureName,
                topic.OrganizationalUnit,
                procedureParams);
        }

        public async Task<dynamic> GetTopicDetailsAsync(string topicName)
        {
            var topics = await dbClient.FindItemsWhereAsync(
                dbSettings.TopicsCollectionId, Constants.Name, topicName);

            var filteredTopics = FilterByDeleteAndOrderByRanking<Topic>(topics);

            return GetOutboundTopicsCollection(filteredTopics);
        }

        public async Task<dynamic> GetResourceDetailAsync(string resourceName, string resourceType)
        {
            List<string> propertyNames = new List<string>() { Constants.Name, Constants.ResourceType };
            List<string> values = new List<string>() { resourceName, resourceType };
            var rawResult = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);

            var filteredResources = FilterByDeleteAndOrderByRanking<Resource>(rawResult);

            return filteredResources;
        }

        private async Task<PagedResources> GetPagedResourceWithLocation(ResourceFilter resourceFilter)
        {
            var location = GetProperLocation(resourceFilter);

            PagedResources pagedResources = await ApplyPaginationLocationAsync(resourceFilter, location);
            if (!pagedResources.Results.Any())
            {
                if (!IsValidLocation(resourceFilter.Location))
                {
                    return await GetPagedResourceWithLocation(resourceFilter);
                }
            }

            return pagedResources;
        }

        private async Task<dynamic> GetGroupedResourceType(ResourceFilter resourceFilter)
        {
            var location = GetProperLocation(resourceFilter);

            List<dynamic> groupedResourceType = await GetResourcesCountLocationAsync(resourceFilter, location);

            if (groupedResourceType.Any(x => x.ResourceName == "All" && x.ResourceCount == 0))
            {
                if (!IsValidLocation(resourceFilter.Location))
                {
                    return await GetGroupedResourceType(resourceFilter);
                }
            }

            return groupedResourceType;
        }

        public async Task<dynamic> GetPagedResourceAsync(ResourceFilter resourceFilter)
        {
            var curatedLocation = new Location
            {
                City = resourceFilter.Location.City,
                County = resourceFilter.Location.County,
                State = resourceFilter.Location.State,
                ZipCode = resourceFilter.Location.ZipCode,
            };

            PagedResourceViewModel pagedResourceViewModel = new PagedResourceViewModel();
            if (resourceFilter.IsResourceCountRequired)
            {
                var groupedResourceType = await GetGroupedResourceType(resourceFilter);
                pagedResourceViewModel.ResourceTypeFilter = JsonUtilities.DeserializeDynamicObject<dynamic>(groupedResourceType);
            }
            dynamic searchFilter = new JObject();
            searchFilter.OrderByField = resourceFilter.OrderByField;
            searchFilter.OrderBy = resourceFilter.OrderBy;

            //Fix duplication
            resourceFilter.TopicIds = resourceFilter.TopicIds.Distinct();

            PagedResources pagedResources = await GetPagedResourceWithLocation(resourceFilter);
            pagedResources.Results = pagedResources.Results.Where(x => x.display != "No").ToList();
            dynamic serializedToken = pagedResources?.ContinuationToken ?? Constants.EmptyArray;
            pagedResourceViewModel.Resources = JsonUtilities.DeserializeDynamicObject<dynamic>(pagedResources?.Results);
            pagedResourceViewModel.ContinuationToken = JsonConvert.DeserializeObject(serializedToken);
            pagedResourceViewModel.TopicIds = JsonUtilities.DeserializeDynamicObject<dynamic>(pagedResources?.TopicIds);
            pagedResourceViewModel.SearchFilter = searchFilter;
            pagedResourceViewModel.CuratedExperienceId = await findGuidedAssistantId(
                JsonUtilities.DeserializeDynamicObject<List<string>>(pagedResourceViewModel.TopicIds), curatedLocation);

            return JObject.FromObject(pagedResourceViewModel).ToString();
        }

        public async Task<dynamic> ApplyPaginationAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter);
            return pagedResources;
        }

        public async Task<dynamic> ApplyPaginationLocationAsync(ResourceFilter resourceFilter, Location location)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseLocationAsync("topicTags", "id", "resourceType", resourceFilter, location);
            return pagedResources;
        }

        public async Task<dynamic> GetResourcesCountAsync(ResourceFilter resourceFilter)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseAsync("topicTags", "id", "resourceType", resourceFilter, true);
            return ResourcesCount(pagedResources);
        }

        public async Task<dynamic> GetResourcesCountLocationAsync(ResourceFilter resourceFilter, Location location)
        {
            PagedResources pagedResources = await dbClient.FindItemsWhereArrayContainsWithAndClauseLocationAsync("topicTags", "id", "resourceType", resourceFilter, location, true);
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
            List<dynamic> resourceList = allResources.Concat(groupedResourceType).ToList();
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
                    var result = await UpdateOrCreateResource(resourceDocument, dbSettings.ResourcesCollectionId,
                        id, resourceType, forms.Name, forms.OrganizationalUnit);

                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Action Plans")
                {
                    actionPlans = UpsertResourcesActionPlans(resourceObject);

                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(actionPlans);

                    var result = await UpdateOrCreateResource(resourceDocument, dbSettings.ResourcesCollectionId,
                        id, resourceType, actionPlans.Name, actionPlans.OrganizationalUnit);

                    resources.Add(result);

                }

                else if (resourceObject.resourceType == "Articles")
                {
                    articles = UpsertResourcesArticles(resourceObject);

                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(articles);

                    var result = await UpdateOrCreateResource(resourceDocument, dbSettings.ResourcesCollectionId,
                        id, resourceType, articles.Name, articles.OrganizationalUnit);

                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Videos")
                {
                    videos = UpsertResourcesVideos(resourceObject);

                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(videos);

                    var result = await UpdateOrCreateResource(resourceDocument, dbSettings.ResourcesCollectionId,
                        id, resourceType, videos.Name, videos.OrganizationalUnit);

                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Organizations")
                {
                    organizations = UpsertResourcesOrganizations(resourceObject);

                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(organizations);
                    var result = await UpdateOrCreateResource(resourceDocument, dbSettings.ResourcesCollectionId,
                        id, resourceType, organizations.Name, organizations.OrganizationalUnit);

                    resources.Add(result);
                }

                else if (resourceObject.resourceType == "Related Readings" || resourceObject.resourceType == "Additional Readings")
                {
                    resourceObject.resourceType = "Related Readings";
                    additionalReadings = UpsertResourcesAdditionalReadings(resourceObject);

                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(additionalReadings);
                    var result = await UpdateOrCreateResource(resourceDocument, dbSettings.ResourcesCollectionId,
                        id, resourceType, additionalReadings.Name, additionalReadings.OrganizationalUnit);

                    resources.Add(result);

                }
                else if (resourceObject.resourceType == "Related Links")
                {
                    relatedLinks = UpsertResourcesRelatedLinks(resourceObject);
                    var resourceDocument = JsonUtilities.DeserializeDynamicObject<object>(relatedLinks);
                    var result = await UpdateOrCreateResource(resourceDocument, dbSettings.ResourcesCollectionId,
                        id, resourceType, relatedLinks.Name, relatedLinks.OrganizationalUnit);

                    resources.Add(result);
                }
            }
            return resources;
        }

        private async Task<Microsoft.Azure.Documents.Document> UpdateOrCreateResource(object resourceDocument, string settings,
            string id, string resourceType, string resourceName, string resourceOrganizationUnit)
        {
            Microsoft.Azure.Documents.Document document;
            if (resourceType == "Additional Readings")
            {
                resourceType = "Related Readings";
            }
            List<string> propertyNames = new List<string>() { Constants.Id, Constants.ResourceType };
            List<string> values = new List<string>() { id, resourceType };
            var resourceDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNames, values);
            if (resourceDBData.Count == 0)
            {
                List<string> propertyNamesAdditional = new List<string>() { Constants.Name, Constants.OrganizationalUnit, Constants.ResourceType };
                List<string> valuesAdditional = new List<string>() { resourceName, resourceOrganizationUnit, resourceType };
                var resourceDBDataAdditional = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, propertyNamesAdditional, valuesAdditional);

                if (resourceDBDataAdditional.Count == 0)
                {
                    document = await dbService.CreateItemAsync(resourceDocument, dbSettings.ResourcesCollectionId);
                }
                else
                {
                    var resourceData = resourceDBDataAdditional[0];
                    var serializedObj = JsonConvert.SerializeObject(resourceDocument);
                    dynamic dynamicObj = JsonConvert.DeserializeObject(serializedObj);
                    dynamicObj.id = resourceData.id;
                    var resDocument = JsonUtilities.DeserializeDynamicObject<object>(dynamicObj);

                    document = await dbService.UpdateItemAsync(resourceData.id, resDocument, dbSettings.ResourcesCollectionId);
                }
            }
            else
            {
                document = await dbService.UpdateItemAsync(id, resourceDocument, dbSettings.ResourcesCollectionId);
            }

            return document;
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
                ModifiedBy = resourceObject.modifiedBy,
                NsmiCode = resourceObject.nsmiCode,
                Ranking = resourceObject.ranking,
                XlsFileName = resourceObject.xlsFileName
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
                ModifiedBy = resourceObject.modifiedBy,
                NsmiCode = resourceObject.nsmiCode,
                Ranking = resourceObject.ranking
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
                Contents = articleContents,
                NsmiCode = resourceObject.nsmiCode,
                Ranking = resourceObject.ranking,
                XlsFileName = resourceObject.xlsFileName
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
                Overview = resourceObject.overview,
                NsmiCode = resourceObject.nsmiCode,
                Ranking = resourceObject.ranking,
                XlsFileName = resourceObject.xlsFileName
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
                Reviewer = organizationReviewers,
                NsmiCode = resourceObject.nsmiCode,
                Display = resourceObject.display,
                Ranking = resourceObject.ranking,
                XlsFileName = resourceObject.xlsFileName
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
                ModifiedBy = resourceObject.modifiedBy,
                NsmiCode = resourceObject.nsmiCode,
                Ranking = resourceObject.ranking,
                XlsFileName = resourceObject.xlsFileName
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
                ModifiedBy = resourceObject.modifiedBy,
                NsmiCode = resourceObject.nsmiCode,
                Ranking = resourceObject.ranking,
                XlsFileName = resourceObject.xlsFileName
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

            try
            {
                foreach (var topicObject in topicObjects)
                {
                    string id = topicObject.id;
                    Microsoft.Azure.Documents.Document document;
                    topicdocuments = UpsertTopics(topicObject);
                    var topicDocument = JsonUtilities.DeserializeDynamicObject<object>(topicdocuments);
                    var topicDBData =
                        await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId, Constants.Id, id);

                    if (topicDBData.Count == 0)
                    {
                        var topicDBDataAdditional =
                            await dbClient.FindItemsWhereAsync(dbSettings.TopicsCollectionId,
                            new List<string> { Constants.Name, Constants.OrganizationalUnit },
                            new List<string> { topicdocuments.Name, topicdocuments.OrganizationalUnit });

                        if (topicDBDataAdditional.Count == 0)
                        {
                            document = await dbService.CreateItemAsync(topicDocument, dbSettings.TopicsCollectionId);
                        }
                        else
                        {
                            var topicData = topicDBDataAdditional[0];
                            var serializedObj = JsonConvert.SerializeObject(topicDocument);
                            dynamic dynamicObj = JsonConvert.DeserializeObject(serializedObj);
                            dynamicObj.id = topicData.id;
                            var resDocument = JsonUtilities.DeserializeDynamicObject<object>(dynamicObj);

                            document = await dbService.UpdateItemAsync(topicData.id, resDocument, dbSettings.TopicsCollectionId);
                        }
                    }
                    else
                    {
                        document = await dbService.UpdateItemAsync(id, topicDocument, dbSettings.TopicsCollectionId);
                    }

                    topics.Add(document);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                Ranking = topicObject.ranking,
                Icon = GetRelativeStaticResourceStoragePath((string)topicObject.icon),
                CreatedBy = topicObject.createdBy,
                ModifiedBy = topicObject.modifiedBy,
                NsmiCode = topicObject.nsmiCode
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

        public async Task<List<Topic>> GetTopics(IEnumerable<string> topicIds)
        {
            return JsonUtilities.DeserializeDynamicObject<List<Topic>>(await dbClient.FindItemsWhereInClauseAsync(dbSettings.TopicsCollectionId, Constants.Id, topicIds));
        }

        public List<string> GetChild2TopicsAsync(string parentGuid)
        {
            var result = dbClient.FindAllChildTopicsAsync(parentGuid).Result;
            var resources = JsonUtilities.DeserializeDynamicObject<List<Resource>>(result) as List<Resource>;
            return resources?.Select(s => s.Name).ToList();
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
                var listOfTopics = topics as List<Topic>;
                if (listOfTopics != null)
                {
                    if (listOfTopics.Any())
                    {
                        var preparedTopics = new List<dynamic>();

                        foreach (var topic in listOfTopics)
                        {
                            preparedTopics.Add(GetOutboundTopic(topic));
                        }

                        topics = preparedTopics;
                    }
                }
                else
                {
                    var listOfDynamic = topics as List<dynamic>;

                    if (listOfDynamic != null &&
                        listOfDynamic.Any())
                    {
                        var preparedTopics = new List<dynamic>();

                        foreach (var rawTopic in listOfDynamic)
                        {
                            preparedTopics.Add(
                                GetOutboundTopic(
                                    JsonUtilities.DeserializeDynamicObject<Topic>(rawTopic)));
                        }

                        topics = preparedTopics;
                    }
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

        // Filters for DELETE and Ranking

        // Catching  calls from tests
        private List<T> FilterByDeleteAndOrderByRanking<T>(JArray rawEntities) where T : class
        {
            var entities = JsonConvert.DeserializeObject<List<T>>(rawEntities.ToString());

            Func<T, bool> isEmpty = (T resource) =>
            {
                var result = false;
                if (typeof(T) == typeof(Topic))
                {
                    result = ((dynamic)resource).Id == null;
                }
                else if (typeof(T) == typeof(Resource))
                {
                    result = ((dynamic)resource).ResourceId == null;
                }

                return result;
            };

            if (entities.Count(w => isEmpty(w)) == entities.Count)
            {
                return new List<T>();
            }
            else
            {
                var dynamicentities = JsonConvert.DeserializeObject<List<dynamic>>(rawEntities.ToString());

                return FilterByDeleteAndOrderByRanking<T>(dynamicentities);
            }
        }

        private List<T> FilterByDeleteAndOrderByRanking<T>(List<dynamic> rawEntities, string topicName = "") where T : class
        {
            if (rawEntities == null)
            {
                return new List<T>();
            }

            Func<T, bool> notDeletedPredicate = (T resource) => !string.Equals(((dynamic)resource).Display, "No", StringComparison.Ordinal);
            Func<T, int> orderByRankingPredicate = (T resource) => ((dynamic)resource).Ranking;
            var realEntities = new List<T>();
            foreach (var entity in rawEntities)
            {
                dynamic rankingArray = null;
                try
                {
                    rankingArray = entity.ranking;
                    var test = (int)rankingArray;
                    continue;
                }
                catch (Exception e)
                {
                    if (rankingArray is null)
                    {
                        continue;
                    }
                }
                foreach (var item in rankingArray)
                {
                    if (item.Name == topicName)
                    {
                        entity.ranking = item.Value;
                    }
                }

            }
            realEntities = rawEntities.Select(s => JsonConvert.DeserializeObject<T>(s.ToString()) as T).ToList();
            var entities = realEntities.Where(notDeletedPredicate).OrderBy(x => orderByRankingPredicate).ToList();

            return entities;
        }

        private T CastDynamicToObj<T>(dynamic obj)
        {
            var dynamicResponse = JsonConvert.SerializeObject(obj);
            var topicResult = (JObject)JsonConvert.DeserializeObject(dynamicResponse);
            var topicResult2 = topicResult.ToObject<T>();
            return topicResult2;
        }

        public async Task<IEnumerable<string>> GetActiveCuratedExperienceIds(Location location)
        {
            List<dynamic> foundResources = new List<dynamic>();
            foreach (var searchLocation in LocationUtilities.GetSearchLocations(location))
            {
                foundResources = await dbClient.FindResourcesWithCuratedExperiences(
                dbSettings.ResourcesCollectionId, searchLocation);

                if (foundResources.Count > 0)
                {
                    break;
                }
            }

            return foundResources.Select(x => CastDynamicToObj<GuidedAssistant>(x)).Cast<GuidedAssistant>().Select(x => x.CuratedExperienceId);
        }

        public async Task<IEnumerable<CuratedExperience>> GetCuratedExperiences(List<string> ids)
        {
            List<dynamic> curatedExperiences = await dbClient.FindCuratedExperiences(dbSettings.CuratedExperiencesCollectionId, ids);

            return curatedExperiences.Select(x => CastDynamicToObj<CuratedExperience>(x)).Cast<CuratedExperience>();
        }

        public async Task DeleteResources(Dictionary<Guid, string> resources)
        {
            foreach (var item in resources)
            {
                List<dynamic> resource = await dbClient.FindItemsWhereAsync(dbSettings.ResourcesCollectionId, Constants.Id, item.Key.ToString());
                if (resource.Count > 0)
                {
                    var orgUnit = resource[0].organizationalUnit;
                    await dbClient.DeleteResource(dbSettings.ResourcesCollectionId, item.Key.ToString(), orgUnit.ToString());
                }
            }
        }
    }
}