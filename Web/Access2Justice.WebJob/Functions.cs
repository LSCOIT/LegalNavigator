using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace Access2Justice.WebJob
{
    public class Functions
    {
        private readonly ILogger<Functions> logger;

        public Functions(ILogger<Functions> logger)
        {
            this.logger = logger;
        }

        public void ProcessQueueMessage([TimerTrigger("* * * * *")]TimerInfo timerInfo)
        {
            logger.LogInformation("Dummy Web Job is Running.........");
            ProcessRTMData();
            logger.LogInformation(DateTime.Now.ToString());
        }

        public void ProcessRTMData()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                string session = string.Empty;
                string serviceProviderResponse = string.Empty;
                string sessionLink = @"https://www.referweb.net/pubres/api/GetSessionID/?ip={apikey:'61GV7G4Y'}";
                try
                {                    
                    logger.LogInformation(sessionLink);
                    logger.LogInformation("Before making API call");
                    var response = httpClient.GetAsync(sessionLink).Result;
                    logger.LogInformation("After making API call");

                    logger.LogInformation("Before converting response to string");
                    if (response != null)
                    {
                        session = response.Content.ReadAsStringAsync().Result;
                        logger.LogInformation(session);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
                if (session != null)
                {
                    dynamic sessionJson = JsonConvert.DeserializeObject(session);
                    string sessionId = sessionJson[0]["session_id"];
                    logger.LogInformation(sessionId);
                    string serviceProviderLink = "https://www.referweb.net/pubres/api/ServiceProviders/?ip={{apikey:'61GV7G4Y',st:'s',catid:'',sn:'Family Based Services',zip:'99606',county:'',sid:'{0}'}}"; ;
                    var spURL = string.Format(CultureInfo.InvariantCulture, serviceProviderLink, sessionId);
                    logger.LogInformation(spURL);
                    var spResponse = httpClient.GetAsync(spURL).Result;
                    logger.LogInformation("After SP API call");
                    if (spResponse != null)
                    {
                        logger.LogInformation("Entered SP response block");
                        serviceProviderResponse = spResponse.Content.ReadAsStringAsync().Result;
                        logger.LogInformation(serviceProviderResponse);
                        if (!string.IsNullOrEmpty(serviceProviderResponse))
                        {
                            logger.LogInformation("Entered SP response IF block");
                            string siteID = string.Empty;
                            string serviceGroupID = string.Empty;
                            string serviceSiteID = string.Empty;
                            List<string> servicedetailsUrls = new List<string>();
                            dynamic serializedSP = JsonConvert.DeserializeObject(serviceProviderResponse);
                            logger.LogInformation("Deserialized SP response");
                            string servicedetailLink = "https://www.referweb.net/pubres/api/ProviderDetail/?ip={{apikey:'61GV7G4Y',locid: '{0}',svid:'{1}',ssid:'{2}',sid:'{3}'}}";
                            foreach (var item in serializedSP)
                            {
                                siteID = item.Sites[0]["ID"];
                                serviceGroupID = item.Sites[0]["ServiceGroup"][0]["ID"];
                                serviceSiteID = item.Sites[0]["ServiceGroup"][0]["ServiceSites"][0]["ID"];
                                string servicedetailURL = string.Format(CultureInfo.InvariantCulture, servicedetailLink, siteID, serviceGroupID, serviceSiteID, sessionId);
                                logger.LogInformation("Formatted URL" + servicedetailURL);
                                servicedetailsUrls.Add(servicedetailURL);
                            }
                            List<dynamic> servicedetailResponse = new List<dynamic>();
                            dynamic spDetailResponse = null;
                            foreach (var servicedetailsUrl in servicedetailsUrls)
                            {
                                logger.LogInformation("Before API CALL" + servicedetailsUrl);
                                spDetailResponse = httpClient.GetAsync(servicedetailsUrl).Result;
                                string result = spDetailResponse.Content.ReadAsStringAsync().Result;
                                logger.LogInformation(result);
                                servicedetailResponse.Add(JsonConvert.DeserializeObject(result));
                            }
                            dynamic seralizedSD = JsonConvert.SerializeObject(servicedetailResponse);
                            JObject serviceDetails = new JObject
                            {
                                { "serviceProvider" , serializedSP },
                                { "serviceProviderDetails" , seralizedSD },
                                { "keyword","Family Based Services"}
                            };
                        }
                        //IF BLOCK ENDS HERE
                    }

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
