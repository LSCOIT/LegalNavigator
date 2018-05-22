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

        public async Task<string> GetIntents(string query)
        {
            try
            {
                var uri = string.Format(CultureInfo.InvariantCulture, _luisSettings.Endpoint.OriginalString, query);

                string result = string.Empty;
                using (var response = await _httpClientService.GetAsync(new Uri(uri)))
                {
                    result = response.Content.ReadAsStringAsync().Result;
                }

                return result;

            }
            catch (Exception e)
            {
                //TO DO : Need to implement exception logging..
                throw e;

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