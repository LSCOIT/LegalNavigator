namespace Access2Justice.CognitiveServices
{
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;    
    using System.Linq;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Access2Justice.Shared;
    

    public class LuisHelper : ILuisHelper
    {
        private IOptions<App> appSettings;
        private IHttpClientService httpClientService;

        public LuisHelper(IOptions<App> appSettings, IHttpClientService httpClientService)
        {
            this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            this.httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        }

        public async Task<IntentWithScore> GetLuisIntent(LuisInput luisInput)
        {
            return await Task.Run(() =>
             {
                 try
                 {

                     var uri = string.Format(appSettings.Value.LuisUrl, luisInput.Sentence);

                     string result;
                     using (var response = httpClientService.GetAsync(uri).Result)
                     {
                         result = response.Content.ReadAsStringAsync().Result;
                     }

                     LuisIntent luisIntent = JsonConvert.DeserializeObject<LuisIntent>(result);
                     return new IntentWithScore
                     {
                         IsSuccessful = true,
                         TopScoringIntent = luisIntent?.TopScoringIntent?.Intent,
                         Score = luisIntent?.TopScoringIntent?.Score ?? 0,
                         TopThreeIntents = luisIntent?.Intents.Skip(1).Take(appSettings.Value.TopIntentsCount).Select(x => x.Intent).ToArray()
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

             });
        }
    }
}
