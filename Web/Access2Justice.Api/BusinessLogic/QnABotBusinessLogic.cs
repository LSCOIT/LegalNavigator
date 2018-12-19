using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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

        public async Task<dynamic> GetAnswersAsync(string question, bool isLuisCallRequired = true)
        {
            dynamic luisResponse = await luisProxy.GetIntents(question);
            IntentWithScore luisTopIntents = luisBusinessLogic.ParseLuisIntent(luisResponse);
            dynamic answerObject = null;
            string bestAnswer = "Sorry, not able to get you. please explain you problem in detail.";
            if (luisTopIntents != null && luisBusinessLogic.IsIntentAccurate(luisTopIntents) && isLuisCallRequired)
            {
                question = luisTopIntents.TopScoringIntent;
            }
            var requestContent = JsonConvert.SerializeObject(new { question });
            using (var request = new HttpRequestMessage())
            {
                // set request data.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(string.Format(CultureInfo.InvariantCulture, qnAMakerSettings.Endpoint.OriginalString, qnAMakerSettings.KnowledgeId));
                request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");                
                request.Headers.Add("Authorization", "EndpointKey " + qnAMakerSettings.AuthorizationKey);

                var response = await httpClientService.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string qnaResponse = response.Content.ReadAsStringAsync().Result;
                    var qnAMakers = JsonConvert.DeserializeObject<QnAMakerResult>(qnaResponse);
                    if (qnAMakers.Answers.LastOrDefault().Score > 0)
                    {
                        bestAnswer = qnAMakers.Answers.OrderBy(answer => answer.Score).LastOrDefault().Answer;
                    }
                }
                if (bestAnswer.Contains("{") && bestAnswer.Contains("}"))
                {
                    answerObject = JsonConvert.DeserializeObject(bestAnswer);
                }
                else
                {
                    answerObject = new JObject { { "description", bestAnswer } };
                }
                return answerObject.ToString();
            }
        }
    }
}
