namespace Access2Justice.Repository
{
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;    
    using System.Linq;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class LUISHelper : ILUISHelper
    {
        private IOptions<App> _appSettings;
        private IHttpClientService _httpClientService;

        public LUISHelper(IOptions<App> appSettings, IHttpClientService httpClientService)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        }

        public async Task<IntentWithScore> GetLUISIntent(LUISInput luisInput)
        {
            return await Task.Run(() =>
             {
                 try
                 {

                     var uri = string.Format(_appSettings.Value.LuisUrl, luisInput.Sentence);

                     string result;
                     using (var client = new HttpClient())
                     {
                         using (var r = _httpClientService.GetAsync(uri).Result)
                         {
                             result = r.Content.ReadAsStringAsync().Result;
                         }
                     }

                     LUISIntent luisIntent = JsonConvert.DeserializeObject<LUISIntent>(result);
                     return new IntentWithScore
                     {
                         IsSuccessful = true,
                         TopScoringIntent = luisIntent?.TopScoringIntent?.intent,
                         Score = luisIntent?.TopScoringIntent?.score ?? 0,
                         TopThreeIntents = luisIntent?.Intents.Skip(1).Take(3).Select(x=>x.intent).ToArray()                         
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
