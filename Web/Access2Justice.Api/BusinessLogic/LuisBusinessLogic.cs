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

        public async Task<dynamic> GetResourceBasedOnThresholdAsync(string query)
        {
            var luisResponse = await luisProxy.GetIntents(query);

            var intentWithScore = ParseLuisIntent(luisResponse);

            int threshold = ApplyThreshold(intentWithScore);

            switch (threshold)
            {
                case (int)LuisAccuracyThreshold.High:
                    return await GetInternalResourcesAsync(intentWithScore.TopScoringIntent);
                case (int)LuisAccuracyThreshold.Medium:
                    JObject luisObject = new JObject { { "luisResponse", luisResponse } };
                    return luisObject.ToString();
                default:
                    return await GetWebResourcesAsync(query);
            }
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

        public int ApplyThreshold(IntentWithScore intentWithScore)
        {
            if (intentWithScore.Score >= luisSettings.UpperThreshold && intentWithScore.TopScoringIntent.ToUpperInvariant() != "NONE")
            {
                return (int)LuisAccuracyThreshold.High;
            }
            else if (intentWithScore.Score <= luisSettings.LowerThreshold || intentWithScore.TopScoringIntent.ToUpperInvariant() == "NONE")
            {
                return (int)LuisAccuracyThreshold.Low;
            }
            else
            {
                return (int)LuisAccuracyThreshold.Medium;
            }
        }

        public async Task<dynamic> GetInternalResourcesAsync(string keyword)
        {
            string topic = string.Empty, resource = string.Empty;
            var topics = await topicsResourcesBusinessLogic.GetTopicsAsync(keyword);

            List<string> topicIds = new List<string>();
            foreach (var item in topics)
            {
                topicIds.Add(item.id);
            }

            dynamic serializedTopics = "[]";
            dynamic serializedResources = "[]";
            dynamic serializedToken = "[]";            
            if (topicIds.Count > 0)
            {
                ResourceFilter resourceFilter = new ResourceFilter { TopicIds = topicIds, PageNumber = 0, ResourceType = "ALL" };
                PagedResources resources = await topicsResourcesBusinessLogic.ApplyPaginationAsync(resourceFilter);

                serializedTopics = JsonConvert.SerializeObject(topics);
                serializedResources = JsonConvert.SerializeObject(resources.Results);
                serializedToken = JsonConvert.SerializeObject(resources.ContinuationToken);
            }

            JObject internalResources = new JObject {
                { "topics", JsonConvert.DeserializeObject(serializedTopics) },
                { "resources", JsonConvert.DeserializeObject(serializedResources) },
                {"continuationToken", JsonConvert.DeserializeObject(serializedToken) },                
                { "topIntent", keyword }
            };

            return internalResources.ToString();
        }

        public async Task<dynamic> GetWebResourcesAsync(string searchTerm)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, bingSettings.BingSearchUrl.OriginalString, searchTerm, bingSettings.CustomConfigId, bingSettings.DefaultCount, bingSettings.DefaultOffset);

            var response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri));

            JObject webResources = new JObject
            {
                { "webResources" , JsonConvert.DeserializeObject(response) }
            };

            return webResources.ToString();
        }
        
    }
}
