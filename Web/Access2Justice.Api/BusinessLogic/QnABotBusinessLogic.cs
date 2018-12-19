using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class QnABotBusinessLogic : IQnABotBusinessLogic
    {

        private readonly IQnAMakerSettings qnAMakerSettings;
        private readonly ILuisProxy luisProxy;
        private readonly IHttpClientService httpClientService;
        private readonly ILuisBusinessLogic luisBusinessLogic;

        public QnABotBusinessLogic(IHttpClientService httpClientService, ILuisProxy luisProxy,
                                   IQnAMakerSettings qnAMakerSettings, ILuisBusinessLogic luisBusinessLogic)
        {
            this.qnAMakerSettings = qnAMakerSettings;
            this.luisProxy = luisProxy;
            this.httpClientService = httpClientService;
            this.luisBusinessLogic = luisBusinessLogic;
        }

        public async Task<dynamic> GetAnswersAsync(string question)
        {
            dynamic luisResponse = await luisProxy.GetIntents(question);
            IntentWithScore luisTopIntents = luisBusinessLogic.ParseLuisIntent(luisResponse);
            string result = string.Empty;
            if (luisTopIntents != null && luisBusinessLogic.IsIntentAccurate(luisTopIntents))
            {
                question = luisTopIntents.TopScoringIntent;
            }
            var requestContent = JsonConvert.SerializeObject(new { question });
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(string.Format(CultureInfo.InvariantCulture, qnAMakerSettings.Endpoint.OriginalString, qnAMakerSettings.KnowledgeId));
                request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
                // set authorization
                request.Headers.Add("Authorization", "EndpointKey " + qnAMakerSettings.AuthorizationKey);

                var response = await httpClientService.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsStringAsync().Result;
                    //var result = JsonConvert.DeserializeObject<QnAMakerResult>(json);
                    //var bestAnswer = result.Answers.OrderBy(answer => answer.Score).LastOrDefault();
                }
                return result;
            }            
        }
    }
}
