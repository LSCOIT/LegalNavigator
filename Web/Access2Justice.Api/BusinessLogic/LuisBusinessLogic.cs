using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Luis;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Access2Justice.Api
{
    public class LuisBusinessLogic : ILuisBusinessLogic
    {
        private readonly ILuisProxy luisProxy;
        private readonly ILuisSettings luisSettings;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;
        private readonly IBingSettings bingSettings;

        public LuisBusinessLogic(ILuisProxy luisProxy, ILuisSettings luisSettings, ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic, IWebSearchBusinessLogic webSearchBusinessLogic, IBingSettings bingSettings)
        {
            this.luisSettings = luisSettings;
            this.luisProxy = luisProxy;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
            this.webSearchBusinessLogic = webSearchBusinessLogic;
            this.bingSettings = bingSettings;
        }

        public async Task<dynamic> GetResourceBasedOnThresholdAsync(LuisInput luisInput)
        {
            if(!string.IsNullOrEmpty(luisInput.LuisTopScoringIntent))
            {
                return await GetInternalResourcesAsync(luisInput.LuisTopScoringIntent, luisInput.Location, null);
            }
            var encodedSentence = HttpUtility.UrlEncode(luisInput.Sentence);
            dynamic luisResponse = await luisProxy.GetIntents(encodedSentence);
            dynamic luisTopIntents = ParseLuisIntent(luisResponse);

            if (IsIntentAccurate(luisTopIntents))
            {
                return await GetInternalResourcesAsync(luisTopIntents?.TopScoringIntent ?? luisInput.LuisTopScoringIntent, luisInput.Location, luisTopIntents.TopNIntents);
            }
            return await GetWebResourcesAsync(encodedSentence);
        }

        public IntentWithScore ParseLuisIntent(string LuisResponse)
        {
            LuisIntent luisIntent = JsonConvert.DeserializeObject<LuisIntent>(LuisResponse);

            return new IntentWithScore
            {
                IsSuccessful = true,
                TopScoringIntent = luisIntent?.TopScoringIntent?.Intent,
                Score = luisIntent?.TopScoringIntent?.Score ?? 0,
                TopNIntents = luisIntent?.Intents.Skip(1).Take(luisSettings.TopIntentsCount).Select(x => x.Intent).ToList()
            };
        }

        public bool IsIntentAccurate(IntentWithScore intentWithScore)
        {
            return intentWithScore.Score >= luisSettings.IntentAccuracyThreshold && intentWithScore.TopScoringIntent.ToUpperInvariant() != "NONE";
        }

        public async Task<dynamic> GetInternalResourcesAsync(string keyword, Location location, IEnumerable<string> relevantIntents)
        {
            string topic = string.Empty, resource = string.Empty;
            var topics = await topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);

            List<string> topicIds = new List<string>();
            foreach (var item in topics)
            {
                string topicId = item.id;
                topicIds.Add(topicId);
            }

            dynamic serializedTopics = Constants.EmptyArray;
            dynamic serializedResources = Constants.EmptyArray;
            dynamic serializedToken = Constants.EmptyArray;
            dynamic serializedTopicIds = Constants.EmptyArray;
            dynamic serializedGroupedResources = Constants.EmptyArray;
            dynamic serializedRelevantIntents = Constants.EmptyArray;
            string guidedAssistantId = string.Empty;
            if (topicIds.Count > 0)
            {
                ResourceFilter resourceFilter = new ResourceFilter { TopicIds = topicIds, PageNumber = 0, ResourceType = Constants.All, Location = location };
                var GetResourcesTask = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
                var ApplyPaginationTask = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
                resourceFilter.ResourceType = Constants.GuidedAssistant;
                var GetGuidedAssistantId = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
                await Task.WhenAll(GetResourcesTask, ApplyPaginationTask, GetGuidedAssistantId);
                var groupedResourceType = GetResourcesTask.Result;
                PagedResources guidedAssistantResponse = GetGuidedAssistantId.Result;
                PagedResources resources = ApplyPaginationTask.Result;
                serializedTopics = JsonConvert.SerializeObject(topics);
                serializedResources = JsonConvert.SerializeObject(resources.Results);
                serializedToken = resources.ContinuationToken ?? Constants.EmptyArray;
                serializedTopicIds = JsonConvert.SerializeObject(topicIds);
                serializedGroupedResources = JsonConvert.SerializeObject(groupedResourceType);
                serializedRelevantIntents = relevantIntents != null ? JsonConvert.SerializeObject(relevantIntents) : Constants.EmptyArray;
                var guidedAssistantResult = JsonConvert.DeserializeObject<Resource>(
                    JsonConvert.SerializeObject(guidedAssistantResponse.Results.FirstOrDefault()));
                if (guidedAssistantResult != null)
                {
                    guidedAssistantId = guidedAssistantResult.ExternalUrls;
                }
            }

            JObject internalResources = new JObject {
                { "topIntent", keyword },
                { "relevantIntents", JsonConvert.DeserializeObject(serializedRelevantIntents)},
                { "topics", JsonConvert.DeserializeObject(serializedTopics) },
                { "resources", JsonConvert.DeserializeObject(serializedResources) },
                {"continuationToken", JsonConvert.DeserializeObject(serializedToken) },
                {"topicIds" , JsonConvert.DeserializeObject(serializedTopicIds)},
                { "resourceTypeFilter", JsonConvert.DeserializeObject(serializedGroupedResources) },
                { "guidedAssistantId", guidedAssistantId }
            };
            return internalResources.ToString();
        }

        public async Task<dynamic> GetWebResourcesAsync(string searchTerm)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, bingSettings.BingSearchUrl.OriginalString, searchTerm, bingSettings.CustomConfigId, bingSettings.PageResultsCount, bingSettings.PageOffsetValue);
            var response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri));
            JObject webResources = new JObject
            {
                { "webResources" , JsonConvert.DeserializeObject(response) }
            };
            return webResources.ToString();
        }
    }
}