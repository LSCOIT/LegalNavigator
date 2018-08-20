﻿using Access2Justice.Shared;
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
            var encodedSentence = HttpUtility.UrlEncode(luisInput.Sentence);
            dynamic luisResponse = await luisProxy.GetIntents(encodedSentence);
            dynamic luisTopIntents = ParseLuisIntent(luisResponse);
            string resourceType = Constants.All;

            if (IsIntentAccurate(luisTopIntents))
            {
                if (luisInput.IsFromCuratedExperience)
                {
                    resourceType = Constants.GuidedAssistant;
                }
                return await GetInternalResourcesAsync(luisTopIntents?.TopScoringIntent ?? luisInput.LuisTopScoringIntent, luisInput.Location, luisTopIntents.TopNIntents, resourceType);
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

        public async Task<dynamic> GetInternalResourcesAsync(string keyword, Location location, IEnumerable<string> relevantIntents, string resourceType)
        {
            string topic = string.Empty, resource = string.Empty;
            var topics = await topicsResourcesBusinessLogic.GetTopicsAsync(keyword, location);

            List<string> topicIds = new List<string>();
            foreach (var item in topics)
            {
                string topicId = item.id;
                topicIds.Add(topicId);
            }

            dynamic serializedTopics = "[]";
            dynamic serializedResources = "[]";
            dynamic serializedToken = "[]";
            dynamic serializedTopicIds = "[]";
            dynamic serializedGroupedResources = "[]";
            dynamic serializedRelevantIntents = "[]";
            if (topicIds.Count > 0)
            {
                ResourceFilter resourceFilter = new ResourceFilter { TopicIds = topicIds, PageNumber = 0, ResourceType = resourceType, Location = location };
                var GetResourcesTask = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
                var ApplyPaginationTask = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
                await Task.WhenAll(GetResourcesTask, ApplyPaginationTask);
                var groupedResourceType = GetResourcesTask.Result;
                PagedResources resources = ApplyPaginationTask.Result;
                serializedTopics = JsonConvert.SerializeObject(topics);
                serializedResources = JsonConvert.SerializeObject(resources.Results);
                serializedToken = resources.ContinuationToken ?? "[]";
                serializedTopicIds = JsonConvert.SerializeObject(topicIds);
                serializedGroupedResources = JsonConvert.SerializeObject(groupedResourceType);
                serializedRelevantIntents = JsonConvert.SerializeObject(relevantIntents);
            }

            JObject internalResources = new JObject {
                { "topIntent", keyword },
                { "relevantIntents", JsonConvert.DeserializeObject(serializedRelevantIntents)},
                { "topics", JsonConvert.DeserializeObject(serializedTopics) },
                { "resources", JsonConvert.DeserializeObject(serializedResources) },
                {"continuationToken", JsonConvert.DeserializeObject(serializedToken) },
                {"topicIds" , JsonConvert.DeserializeObject(serializedTopicIds)},
                { "resourceTypeFilter", JsonConvert.DeserializeObject(serializedGroupedResources) }
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