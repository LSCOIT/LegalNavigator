using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
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
        dynamic luisTopIntents = null;

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
            //var encodedSentence = HttpUtility.UrlEncode(luisInput.Sentence);
            //if (string.IsNullOrEmpty(luisInput.LuisTopScoringIntent))
            //{
            //    dynamic luisResponse = await luisProxy.GetIntents(encodedSentence);
            //    luisTopIntents = ParseLuisIntent(luisResponse);
            //}

            //if ((luisTopIntents != null && IsIntentAccurate(luisTopIntents)) || !string.IsNullOrEmpty(luisInput.LuisTopScoringIntent))
            if (true)
            {
                return await GetInternalResourcesAsync(
                    "Eviction",
                    luisInput.Location,
                    luisTopIntents != null && luisTopIntents.TopNIntents != null ? luisTopIntents.TopNIntents : null);
            }
            //return await GetWebResourcesAsync(encodedSentence);
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

            if (topicIds.Count == 0 || location == null)
            {
                return JObject.FromObject(new LuisViewModel
                {
                    TopIntent = keyword
                }).ToString();
            }

            ResourceFilter resourceFilter = new ResourceFilter { TopicIds = topicIds, PageNumber = 0, ResourceType = Constants.All, Location = location };
            var GetResourcesTask = topicsResourcesBusinessLogic.GetResourcesCountAsync(resourceFilter);
            var ApplyPaginationTask = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            //To get guided assistant id
            resourceFilter.ResourceType = Constants.GuidedAssistant;
            var GetGuidedAssistantId = topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);
            await Task.WhenAll(GetResourcesTask, ApplyPaginationTask, GetGuidedAssistantId);

            var groupedResourceType = GetResourcesTask.Result;
            PagedResources resources = ApplyPaginationTask.Result;
            PagedResources guidedAssistantResponse = GetGuidedAssistantId.Result;
            var guidedAssistantResult = JsonUtilities.DeserializeDynamicObject<Resource>(guidedAssistantResponse.Results.FirstOrDefault());
            
            return JObject.FromObject(new LuisViewModel
            {
                TopIntent = keyword,
                RelevantIntents = relevantIntents != null ? JsonUtilities.DeserializeDynamicObject<dynamic>(relevantIntents) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                Topics = JsonUtilities.DeserializeDynamicObject<dynamic>(topics),
                Resources = JsonUtilities.DeserializeDynamicObject<dynamic>(resources.Results),
                ContinuationToken = resources.ContinuationToken != null ? JsonConvert.DeserializeObject(resources.ContinuationToken) : JsonConvert.DeserializeObject(Constants.EmptyArray),
                TopicIds = JsonUtilities.DeserializeDynamicObject<dynamic>(topicIds),
                ResourceTypeFilter = JsonUtilities.DeserializeDynamicObject<dynamic>(groupedResourceType),
                GuidedAssistantId = guidedAssistantResult != null ? guidedAssistantResult.ExternalUrls : string.Empty
            }).ToString();
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