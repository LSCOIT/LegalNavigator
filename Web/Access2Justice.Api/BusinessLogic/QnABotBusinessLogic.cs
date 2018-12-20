using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class QnABotBusinessLogic : IQnABotBusinessLogic
    {

        private readonly IQnAMakerSettings qnAMakerSettings;
        private readonly ILuisProxy luisProxy;
        private readonly IHttpClientService httpClientService;
        private readonly ILuisBusinessLogic luisBusinessLogic;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;

        public QnABotBusinessLogic(IHttpClientService httpClientService, ILuisProxy luisProxy, ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic,
                                   IQnAMakerSettings qnAMakerSettings, ILuisBusinessLogic luisBusinessLogic)
        {
            this.qnAMakerSettings = qnAMakerSettings;
            this.luisProxy = luisProxy;
            this.httpClientService = httpClientService;
            this.luisBusinessLogic = luisBusinessLogic;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
        }

        public async Task<dynamic> GetAnswersAsync(string inputQuestion, bool isLuisCallRequired = true)
        {
            //@TODO Need to make changes as per AI's are trained..
            dynamic answerObject = null;
            string bestAnswer = "Sorry, not able to get you. Please explain your problem in detail.";
            string[] input = inputQuestion.Split("|");
            if (!isLuisCallRequired)
            {
                //dynamic luisResponse = await luisProxy.GetIntents(question);
                //IntentWithScore luisTopIntents = luisBusinessLogic.ParseLuisIntent(luisResponse);
                //if (luisTopIntents != null && luisBusinessLogic.IsIntentAccurate(luisTopIntents))
                //{
                //    question = luisTopIntents.TopScoringIntent;
                //}
            }
            string question = input[0];
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
                //if (bestAnswer.Contains("{") && bestAnswer.Contains("}"))
                if(bestAnswer.Contains("|"))
                {
                    string[] keyword = bestAnswer.Split("|");
                    CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    TextInfo textInfo = cultureInfo.TextInfo;
                    Location location = new Location { State = input[1] };
                    var topics = await topicsResourcesBusinessLogic.GetTopicsAsync(textInfo.ToTitleCase(keyword[1]), location);
                    if (topics != null && topics.Count > 0)
                    {
                        string topicId = topics[0].id;
                        answerObject = new JObject { { "description", bestAnswer }, { "topic", "topics/" + topicId } };
                    }
                    else
                    {
                        bestAnswer = "We couldn't find internal resources, please try our web search";
                        answerObject = new JObject { { "description", bestAnswer } };
                    }
                    //answerObject = JsonConvert.DeserializeObject(bestAnswer);
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
