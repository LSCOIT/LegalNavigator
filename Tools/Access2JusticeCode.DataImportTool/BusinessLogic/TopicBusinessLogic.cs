using Access2Justice.DataImportTool.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Access2Justice.DataImportTool.BusinessLogic
{
    public class TopicBusinessLogic: IDisposable
    {
        static HttpClient clientHttp = new HttpClient();

        public async static Task GetTopics(string accessToken, string filePath)
        {
            clientHttp.BaseAddress = new Uri("http://localhost:4200/");
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                InsertTopics obj = new InsertTopics();
                List<dynamic> topicsList = new List<dynamic>();
                var topics = obj.CreateJsonFromCSV(filePath);
                if (topics == null || topics.Count == 0)
                {
                    MessageBox.Show("Please check Error log file to correct errors");
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
                        if (topicList.parentTopicId != null)
                        {
                            foreach (var topicData in topicsList)
                            {
                                for (int iterator = 0; iterator < topicList.parentTopicId.Count; iterator++)
                                {
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
                    StringContent content = new StringContent(serializedTopics, Encoding.UTF8, "application/json");
                    var response = await clientHttp.PostAsync("api/topics/upsert-topic-documents", content).ConfigureAwait(false);
                    var json = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode == true)
                    {
                        string fileName = $@"Topic{DateTime.Now.Ticks}.txt";
                        LogHelper.DataLogging(json, fileName);                        
                        MessageBox.Show("File got processed");
                    }
                    else
                    {
                        MessageBox.Show("Please correct errors" + "\n" + response);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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