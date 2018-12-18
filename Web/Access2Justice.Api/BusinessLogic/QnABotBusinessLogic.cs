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
        private readonly IHttpClientService httpClientService;

        public QnABotBusinessLogic(IHttpClientService httpClientService, IQnAMakerSettings qnAMakerSettings)
        {
            this.qnAMakerSettings = qnAMakerSettings;
            this.httpClientService = httpClientService;
        }

        public async Task<dynamic> GetAnswersAsync(string question)
        {
            string result = string.Empty;
            var requestContent = JsonConvert.SerializeObject(new { question });
            using (var content = new StringContent(requestContent, Encoding.UTF8, "application/json"))            
            {
                //httpClientService.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "<TODO: YOUR KEY>");
                Uri QnAUrl = new Uri(string.Format(CultureInfo.InvariantCulture, qnAMakerSettings.Endpoint.OriginalString, qnAMakerSettings.KnowledgeId));
                var response = await httpClientService.PostAsync(QnAUrl, content, qnAMakerSettings.AuthorizationKey);
                if (response.IsSuccessStatusCode) {
                    result = response.Content.ReadAsStringAsync().Result;
                    //var result = JsonConvert.DeserializeObject<QnAMakerResult>(json);
                    //var bestAnswer = result.Answers.OrderBy(answer => answer.Score).LastOrDefault();
                }
                return result;
            }
        }

    }
}
