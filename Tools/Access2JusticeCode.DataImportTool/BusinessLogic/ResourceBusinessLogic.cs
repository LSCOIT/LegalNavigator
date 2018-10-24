using Access2JusticeCode.DataImportTool.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Access2Justice.Tools.BusinessLogic
{
    class ResourceBusinessLogic : IDisposable
    {
        static HttpClient clientHttp = new HttpClient();

        public async static Task GetResources(string accessToken,string filePath)
        {
            clientHttp.BaseAddress = new Uri("http://localhost:4200/");
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            clientHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {
                InsertResources obj = new InsertResources();
                List<dynamic> resourcesList = new List<dynamic>();
                var resources = obj.CreateJsonFromCSV(filePath);
                if (resources == null || resources.Count == 0)
                {
                    MessageBox.Show("Please check Error log file to correct errors");
                }
                else
                {
                    foreach (var resourceList in resources)
                    {
                        var serializedResult = JsonConvert.SerializeObject(resourceList);
                        JObject jsonResult = (JObject)JsonConvert.DeserializeObject(serializedResult);
                        resourcesList.Add(jsonResult);
                    }

                    var topicTag = await clientHttp.GetAsync("api/topics/get-all-topics").ConfigureAwait(false);
                    var topicResult = topicTag.Content.ReadAsStringAsync().Result;
                    dynamic topicTagResult = JsonConvert.DeserializeObject(topicResult);
                    foreach (var resourceList in resourcesList)
                    {
                        if (resourceList.topicTags != null)
                        {
                            for (int iterator = 0; iterator < resourceList.topicTags.Count; iterator++)
                            {
                                string name = resourceList.topicTags[iterator].id;
                                string state = resourceList.location[0].state;
                                if (topicTagResult.Count > 0)
                                {
                                    foreach (var topic in topicTagResult)
                                    {
                                        dynamic topicName = null;
                                        dynamic topicTagId = null;
                                        dynamic locationValue = null;
                                        foreach (JProperty field in topic)
                                        {
                                            if (field.Name == "name")
                                            {
                                                topicName = field.Value.ToString();
                                            }

                                            if (field.Name == "id")
                                            {
                                                topicTagId = field.Value.ToString();
                                            }

                                            if (name == topicName)
                                            {
                                                if (field.Name == "location")
                                                {
                                                    locationValue = field.Value.ToString();
                                                    var location = JsonConvert.DeserializeObject(locationValue);
                                                    foreach (var loc in location)
                                                    {
                                                        foreach (JProperty locationTopic in loc)
                                                        {
                                                            if (locationTopic.Name == "state")
                                                            {
                                                                if (state == locationTopic.Value.ToString())
                                                                {
                                                                    resourceList.topicTags[iterator].id = topicTagId;
                                                                    break;
                                                                }
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var serializedResources = JsonConvert.SerializeObject(resourcesList);
                    StringContent content = new StringContent(serializedResources, Encoding.UTF8, "application/json");
                    var response = await clientHttp.PostAsync("api/upsert-resource-document", content).ConfigureAwait(false);
                    var json = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode == true)
                    {
                        string fileName = $@"Resource{DateTime.Now.Ticks}.txt";
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