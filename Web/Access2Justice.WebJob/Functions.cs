using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
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
                string res = string.Empty;
                string sessionLink = "https://www.referweb.net/pubres/api/GetSessionID/?ip=61GV7G4Y";
                try
                {
                    HttpClient httpClient = new HttpClient();
                    logger.LogInformation("Before making API call");
                    var response = httpClient.GetAsync(sessionLink).Result;
                    logger.LogInformation("After making API call");

                    logger.LogInformation("Before converting response to string");
                    if (response != null)
                    {
                        res = response.Content.ReadAsStringAsync().Result;
                        logger.LogInformation(res);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
                if (res != null)
                {
                    logger.LogInformation("Entered response not null block");
                    string info = Convert.ToString(res);
                    logger.LogInformation(info);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
