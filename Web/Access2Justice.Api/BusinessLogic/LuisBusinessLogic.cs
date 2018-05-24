using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Access2Justice.Shared.Utilities;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Api
{
    public class LuisBusinessLogic : ILuisBusinessLogic
    {
        private readonly ILuisProxy _luisProxy;
        private readonly ILuisSettings _luisSettings;
        private readonly ITopicsResourcesBusinessLogic _topicsResourcesBusinessLogic;
        private readonly IWebSearchBusinessLogic _webSearchBusinessLogic;

        public LuisBusinessLogic(ILuisProxy luisProxy, ILuisSettings luisSettings, ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic, IWebSearchBusinessLogic webSearchBusinessLogic)
        {
            _luisSettings = luisSettings;
            _luisProxy = luisProxy;
            _topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
            _webSearchBusinessLogic = webSearchBusinessLogic;
        }

        public async Task<dynamic> GetResourceBasedOnThresholdAsync(string query)
        {
            var luisResponse = await _luisProxy.GetIntents(query);

            var intentWithScore = new IntentWithScore();            
            if (luisResponse == null)
            {
                return "No intents found";
            }
            else
            {
                intentWithScore = ParseLuisIntent(luisResponse);
            }

            int threshold = ApplyThreshold(intentWithScore);

            switch (threshold)
            {
                case (int)Threshold.Upper:
                    return await GetInternalResourcesAsync(intentWithScore.TopScoringIntent);                    
                case (int)Threshold.Medium:
                    JObject luisObject = new JObject { { "luisResponse", luisResponse } };
                    return luisObject.ToString();
                default:
                    return await GetWebResourcesAsync(query);
            }
        }

        public IntentWithScore ParseLuisIntent(string LuisResponse)
        {
            LuisIntent luisIntent = JsonConvert.DeserializeObject<LuisIntent>(LuisResponse);
            NumberFormatInfo provider = new NumberFormatInfo { PositiveSign = "pos " };
            return new IntentWithScore
            {
                IsSuccessful = true,
                TopScoringIntent = luisIntent?.TopScoringIntent?.Intent,
                Score = luisIntent?.TopScoringIntent?.Score ?? 0,
                TopNIntents = luisIntent?.Intents.Skip(1).Take(Convert.ToInt16(_luisSettings.TopIntentsCount, provider)).Select(x => x.Intent).ToList()
            };
        }

        public int ApplyThreshold(IntentWithScore intentWithScore)
        {

            NumberFormatInfo provider = new NumberFormatInfo { NumberDecimalDigits = 2 };
            decimal upperThershold = Convert.ToDecimal(_luisSettings.UpperThreshold, provider);
            decimal lowerThershold = Convert.ToDecimal(_luisSettings.LowerThreshold, provider);

            if (intentWithScore.Score >= upperThershold && intentWithScore.TopScoringIntent.ToUpperInvariant() != "NONE")
            {
               return (int)Threshold.Upper;
            }
            else if (intentWithScore.Score <= lowerThershold && intentWithScore.TopScoringIntent.ToUpperInvariant() != "NONE")
            {
                return (int)Threshold.Lower;
            }
            else
            {
                return (int)Threshold.Medium;
            }
            
        }

        public async Task<dynamic> GetInternalResourcesAsync(string keywords)
        {
            string topic, resource = string.Empty;
            var topics = await _topicsResourcesBusinessLogic.GetTopicAsync(keywords);
           
            string topicIds = string.Empty;
            foreach (var item in topics)
            {
                topicIds += "  ARRAY_CONTAINS(c.topicTags, { 'id' : '" + item.id + "'}) OR";
            }

            topic = JsonFormatter.SanitizeJson(JsonConvert.SerializeObject(topics), "_");
            
            if (!string.IsNullOrEmpty(topicIds))
            {
                topicIds = topicIds.Remove(topicIds.Length - 2);
                var resources = await _topicsResourcesBusinessLogic.GetResourcesAsync(topicIds);
                resource = JsonFormatter.SanitizeJson(JsonConvert.SerializeObject(resources), "_");
            }

            JObject internalResources = new JObject {
                { "topics", topic },
                { "resources", resource }
            };
            return internalResources.ToString();
        }

        public async Task<dynamic> GetWebResourcesAsync(string query)
        {
            var response = await _webSearchBusinessLogic.SearchWebResourcesAsync(query);
            List<string> props = new List<string> { "webPages", "value" };
            string filteredJson = JsonFormatter.FilterJson(response, props);
            
            JObject webResources = new JObject
            {
                { "webResources" , filteredJson }
            };
            return webResources.ToString();
        }

        public enum Threshold : int
        {
            Lower = 0,
            Medium = 1,
            Upper = 2
        }

    }
}
