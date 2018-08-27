using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
            var topics = obj.CreateJsonFromCSV();
            List<dynamic> topicsList = new List<dynamic>();

            foreach (var topicList in topics)
            {
                var serializedResult = JsonConvert.SerializeObject(topicList);
                JObject jsonResult = (JObject)JsonConvert.DeserializeObject(serializedResult);
                topicsList.Add(jsonResult);
            }
            var serializedTopics = JsonConvert.SerializeObject(topicsList);
            var result = JsonConvert.DeserializeObject(serializedTopics);
            var response = await clientHttp.PostAsJsonAsync("api/createtopicdocument", result).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode == true) {
                Console.WriteLine("Topics created successfully"+ "\n"+ result);
            }

            else
            {
                Console.WriteLine("Please correct errors" + "\n" + response);
            }
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