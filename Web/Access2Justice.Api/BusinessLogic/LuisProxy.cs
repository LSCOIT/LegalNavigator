namespace Access2Justice.Api
{
    using System;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Linq;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Access2Justice.Shared;
    using System.Globalization;

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
            return await Task.Run(() =>
             {
                 try
                 {
                     var uri = string.Format(CultureInfo.InvariantCulture,appSettings.LuisUrl.OriginalString,luisInput.Sentence);

                     string result;
                     using (var response = httpClientService.GetAsync(new Uri(uri)).Result)
                     {
                         result = response.Content.ReadAsStringAsync().Result;
                     }

                     LuisIntent luisIntent = JsonConvert.DeserializeObject<LuisIntent>(result);
                     NumberFormatInfo provider = new NumberFormatInfo();
                     provider.PositiveSign = "pos ";
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

             }).ConfigureAwait(false);
        }
    }
}
