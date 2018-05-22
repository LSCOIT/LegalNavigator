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

            // todo: we need to work on this logic, it is not good the way it is:
            IEnumerable<string> keywords = FilterLuisIntents(intentWithScore);
            string input = "";
            foreach (var keyword in keywords)
            {
                input = keyword; break;
            }



            var topics = await _topicsResourcesBusinessLogic.GetTopicAsync(input);
            var resources = topics; // todo: implement and use the await _topicsResourcesBusinessLogic.GetResources(pass topics id);

            return resources;
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

        public IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore)
        {

            NumberFormatInfo provider = new NumberFormatInfo { NumberDecimalDigits = 2 };
            decimal upperThershold = Convert.ToDecimal(_luisSettings.UpperThreshold, provider);
            //decimal lowerThershold = Convert.ToDecimal(_luisSettings.LowerThreshold, provider);

            List<string> keywords = new List<string>();
            if (intentWithScore.Score >= upperThershold)
            {
                keywords.Add(intentWithScore.TopScoringIntent);
            }
            //else if (intentWithScore.Score <= lowerThershold) {
            else
            {
                string input = intentWithScore.TopScoringIntent;
                foreach (var item in intentWithScore.TopNIntents)
                {
                    input += "," + item;
                }

                keywords.Add(input);
            }
            return keywords;
        }
    }
}
