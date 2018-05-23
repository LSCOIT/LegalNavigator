using Access2Justice.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace Access2Justice.Api
{
    public class LuisProxy : ILuisProxy
    {
        private ILuisSettings _luisSettings;
        private IHttpClientService _httpClientService;

        public LuisProxy(IHttpClientService httpClientService, ILuisSettings luisSettings)
        {
            _luisSettings = luisSettings;
            _httpClientService = httpClientService;
        }

        public async Task<IntentWithScore> GetIntents(string query)
        {
            try
            {
                var uri = string.Format(CultureInfo.InvariantCulture, _luisSettings.Endpoint.OriginalString, query);

                string result = string.Empty;
                using (var response = await _httpClientService.GetAsync(uri))
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }
                LuisIntent luisIntent = JsonConvert.DeserializeObject<LuisIntent>(result);
                NumberFormatInfo provider = new NumberFormatInfo { PositiveSign = "pos " };
                return new IntentWithScore
                {
                    IsSuccessful = true,
                    TopScoringIntent = luisIntent?.TopScoringIntent?.Intent,
                    Score = luisIntent?.TopScoringIntent?.Score ?? 0,
                    TopNIntents = luisIntent?.Intents.Skip(1).Take(Convert.ToInt16(_luisSettings.TopIntentsCount, provider)).Select(x => x.Intent).ToList()
                };
            }
            catch (Exception e)
            {
                //TO DO : Need to implement exception logging..
                return new IntentWithScore
                {
                    IsSuccessful = false,
                    ErrorMessage = e.Message
                };

            }
        }

        public IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore)
        {
            string input = intentWithScore.TopScoringIntent;
            if (input?.ToUpperInvariant() == "NONE")
            {
                return null;
            }
            foreach (var item in intentWithScore.TopNIntents)
            {
                input += "," + item;
            }
            string[] spParams = { Constants.keywords, input };

            return spParams;
        }
    }
}