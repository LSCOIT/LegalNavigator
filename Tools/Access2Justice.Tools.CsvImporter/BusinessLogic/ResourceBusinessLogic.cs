using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Access2Justice.Tools.BusinessLogic
{
    class ResourceBusinessLogic : IDisposable
    {
        static HttpClient clientHttp = new HttpClient();

        public async static void GetResources()
        {
            clientHttp.BaseAddress = new Uri("http://localhost:4200/");
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                InsertResources obj = new InsertResources();
                List<dynamic> resourcesList = new List<dynamic>();
                var resources = obj.CreateJsonFromCSV();
                if (resources == null || resources.Count == 0)
                {
                    throw new Exception("Please check Error log file to correct errors");
                }

                else
                {
                    foreach (var resourceList in resources)
                    {
                        var serializedResult = JsonConvert.SerializeObject(resourceList);
                        JObject jsonResult = (JObject)JsonConvert.DeserializeObject(serializedResult);
                        resourcesList.Add(jsonResult);
                    }

                    foreach (var resourceList in resourcesList)
                    {
                        if (resourceList.topicTags != null)
                        {
                            for (int iterator = 0; iterator < resourceList.topicTags.Count; iterator++)
                            {
                                string name = resourceList.topicTags[iterator].id;
                                string location = resourceList.location[0].state;
                                var topicTag = await clientHttp.GetAsync("api/topics/gettopicdetails/" + name).ConfigureAwait(false);
                                var topicResult = topicTag.Content.ReadAsStringAsync().Result;
                                dynamic topicTagResult = JsonConvert.DeserializeObject(topicResult);
                                var topicTagId = topicTagResult[iterator].id;
                                resourceList.topicTags[iterator].id = topicTagId;
                            }
                        }
                    }

                    var serializedResources = JsonConvert.SerializeObject(resourcesList);
                    var result = JsonConvert.DeserializeObject(serializedResources);
                    var response = await clientHttp.PostAsJsonAsync("api/upsertresourcedocument", result).ConfigureAwait(false);
                    var json = response.Content.ReadAsStringAsync().Result;
                    var documentsCreated = JsonConvert.DeserializeObject(json);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode == true)
                    {
                        Console.WriteLine("Resources created successfully" + "\n" + documentsCreated);
                        Console.WriteLine("You may close the window now.");
                    }
                    else
                    {
                        throw new Exception("Please correct errors" + "\n" + response);
                    }
                }          
            }
            catch (Exception ex)
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