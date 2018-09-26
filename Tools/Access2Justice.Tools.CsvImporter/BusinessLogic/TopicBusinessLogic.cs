using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Access2Justice.Tools.BusinessLogic
{
    public class TopicBusinessLogic: IDisposable
    {
        static HttpClient clientHttp = new HttpClient();
                
        public async static void GetTopics()
        {
            clientHttp.BaseAddress = new Uri("http://localhost:4200/");
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                InsertTopics obj = new InsertTopics();
                List<dynamic> topicsList = new List<dynamic>();
                var topics = obj.CreateJsonFromCSV();
                if (topics == null || topics.Count==0)
                {
                    throw new Exception("Please check Error log file to correct errors");
                }

                else
                {
                    foreach (var topicList in topics)
                    {
                        var serializedResult = JsonConvert.SerializeObject(topicList);
                        JObject jsonResult = (JObject)JsonConvert.DeserializeObject(serializedResult);
                        topicsList.Add(jsonResult);
                    }

                    foreach (var topicList in topicsList)
                    {
                        //var parentTopic = topicList.parentTopicId;
                        if (topicList.parentTopicId != null)
                        {                    
                            foreach (var topicData in topicsList)
                            {
                                for (int iterator=0; iterator < topicList.parentTopicId.Count; iterator++) {
                                    if (topicList.parentTopicId[iterator].id == topicData.name)
                                    {
                                        for (int locationIteratortopicList = 0; locationIteratortopicList < topicList.location.Count; locationIteratortopicList++)
                                        {
                                            for (int locationIteratortopicData = 0; locationIteratortopicData < topicData.location.Count; locationIteratortopicData++)
                                            {
                                                if (topicList.location[locationIteratortopicList].state == topicData.location[locationIteratortopicData].state)
                                                {
                                                    topicList.parentTopicId[iterator].id = topicData.id;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var serializedTopics = JsonConvert.SerializeObject(topicsList);
                    var result = JsonConvert.DeserializeObject(serializedTopics);
                    var response = await clientHttp.PostAsJsonAsync("api/upserttopicdocument", result).ConfigureAwait(false);
                    var json = response.Content.ReadAsStringAsync().Result;
                    var documentsCreated = JsonConvert.DeserializeObject(json);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode == true)
                    {
                        Console.WriteLine("Topics created successfully" + "\n" + documentsCreated);
                        Console.WriteLine("You may close the window now.");
                    }
                    else
                    {
                        throw new Exception("Please correct errors" + "\n" + response);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
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