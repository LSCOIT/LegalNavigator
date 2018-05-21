namespace Access2Justice.Api
{
    using Access2Justice.Shared;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public class LuisProxy : ILuisProxy
    {
        private IApp appSettings;
        private IHttpClientService httpClientService;

        public LuisProxy(IHttpClientService httpClientService, IApp appSettings)
        {
            this.appSettings = appSettings;
            this.httpClientService = httpClientService;
        }

        public async Task<IntentWithScore> GetLuisIntent(LuisInput luisInput)
        {
            try
            {
                var uri = string.Format(CultureInfo.InvariantCulture, appSettings.LuisUrl.OriginalString, luisInput.Sentence);                

                string result = string.Empty;
                using (var response = await httpClientService.GetAsync(new Uri(uri)))
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
                    TopNIntents = luisIntent?.Intents.Skip(1).Take(Convert.ToInt16(appSettings.TopIntentsCount, provider)).Select(x => x.Intent).ToList()
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