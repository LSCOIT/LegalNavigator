using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api
{
    public class LuisBusinessLogic : ILuisBusinessLogic
    {
        private readonly ILuisProxy _luisProxy;
        private readonly ILuisSettings _luisSettings;
        private readonly ITopicsResourcesBusinessLogic _topicsResourcesBusinessLogic;

        public LuisBusinessLogic(ILuisProxy luisProxy, ILuisSettings luisSettings, ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic)
        {
            _luisSettings = luisSettings;
            _luisProxy = luisProxy;
            _topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
        }

        public async Task<dynamic> GetInternalResources(string query)
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
            
            string keywords = FilterLuisIntents(intentWithScore);
            List<dynamic> internalResources = new List<dynamic>();
            var topics = await _topicsResourcesBusinessLogic.GetTopicAsync(keywords);

            string topicIds = string.Empty;
            foreach (var topic in topics)
            {
                topicIds += "'" + topic.id + "',";
            }
            internalResources.Add(JsonConvert.SerializeObject(topics));
            if (!string.IsNullOrEmpty(topicIds))
            {
                topicIds = topicIds.Remove(topicIds.Length - 1);
                var resources = await _topicsResourcesBusinessLogic.GetResources(topicIds);
                internalResources.Add(JsonConvert.SerializeObject(resources));
            }

            return internalResources;
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

        public string FilterLuisIntents(IntentWithScore intentWithScore)
        {

            NumberFormatInfo provider = new NumberFormatInfo { NumberDecimalDigits = 2 };
            decimal upperThershold = Convert.ToDecimal(_luisSettings.UpperThreshold, provider);
            decimal lowerThershold = Convert.ToDecimal(_luisSettings.LowerThreshold, provider);

            string keywords = string.Empty;
            if (intentWithScore.Score >= upperThershold)
            {
                keywords = intentWithScore.TopScoringIntent;
            }
            else if (intentWithScore.Score <= lowerThershold)
            {
                // TO DO : Need to check this scenario and implement logic.
            }
            else
            {
                keywords = intentWithScore.TopScoringIntent;
                foreach (var item in intentWithScore.TopNIntents)
                {
                    keywords += "," + item;
                }
            }
            return keywords;
        }
    }
}
