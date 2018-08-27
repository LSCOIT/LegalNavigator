using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Tools.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Access2Justice.Tools.BusinessLogic
{
    public class TopicBusinessLogic: IDisposable
    {
        static HttpClient clientHttp = new HttpClient();

        public async void GetTopics()
        {
            clientHttp.BaseAddress = new Uri("http://localhost:4200/");
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            InsertTopics obj = new InsertTopics();
            var content = obj.CreateJsonFromCSV();
            List<dynamic> topicsList = new List<dynamic>();
            Topics topics = new Topics
            {
                TopicsList = content.TopicsList
            };

            foreach (var topicList in topics.TopicsList)
            {
                var serializedResult = JsonConvert.SerializeObject(topicList);
                JObject jsonResult = (JObject)JsonConvert.DeserializeObject(serializedResult);
                topicsList.Add(jsonResult);
            }
            var serializedTopics = JsonConvert.SerializeObject(topicsList);
            var result = JsonConvert.DeserializeObject(serializedTopics);
            var response = await clientHttp.PostAsJsonAsync("api/createtopicdocument", result).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode == true) { };            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}