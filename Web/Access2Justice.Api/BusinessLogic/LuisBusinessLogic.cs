using Access2Justice.CosmosDb.Interfaces;
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
        private readonly IBackendDatabaseService _backendDatabaseService;
        private readonly ILuisProxy _luisProxy;
        private readonly ICosmosDbSettings _cosmosDbSettings;
        private readonly ILuisSettings _luisSettings;

        public LuisBusinessLogic(IBackendDatabaseService backendDatabaseService, ILuisProxy luisProxy, 
            ICosmosDbSettings cosmosDbSettings, ILuisSettings luisSettings)
        {
            _backendDatabaseService = backendDatabaseService;
            _cosmosDbSettings = cosmosDbSettings;
            _luisSettings = luisSettings;
            _luisProxy = luisProxy;
        }

        public async Task<dynamic> GetInternalResources(string query)
        {
            var luisResponse = await _luisProxy.GetIntents(query);
            var intentWithScore = string.IsNullOrEmpty(luisResponse) ? null : ParseLuisIntent(luisResponse);

            IEnumerable<string> keywords = FilterLuisIntents(intentWithScore);
            string input = "";
            foreach (var keyword in keywords)
            {
                input = keyword; break;
            }
            var response = await GetTopicAsync(input);
            
            //todo: get resources

            return response; // todo: return resources instead
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

        public async Task<dynamic> GetTopicAsync(string keywords)
        {
            // we need to use a quey format to retrieve items because we are returning a dynmaic object.
            var qeury = string.Format("SELECT * FROM c WHERE CONTAINS(c.keywords, '{0}')", keywords.ToLower());
            var result = await _backendDatabaseService.QueryItemsAsync(_cosmosDbSettings.TopicCollectionId, qeury);

            return JsonConvert.SerializeObject(result);
        }
    }
}
