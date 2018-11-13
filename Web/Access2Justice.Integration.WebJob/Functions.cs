using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Access2Justice.Integration.WebJob
{
    public class Functions
    {
        private readonly ILogger logger;
       // private readonly IHttpClientService httpClientService;

        public Functions(ILogger logger)//, IHttpClientService httpClientService)
        {
            this.logger = logger;
            //this.httpClientService = httpClientService;
        }

        public void ProcessQueueMessage([TimerTrigger("* * * * *")]TimerInfo timerInfo)
        {
            logger.LogInformation(DateTime.Now.ToString());
        }

        //public void ProcessRTMData()
        //{
        //    try
        //    {
        //        dynamic result = null;
        //        string sessionLink = "https://www.referweb.net/pubres/api/GetSessionID/?ip=61GV7G4Y";
        //        try
        //        {
        //            logger.LogInformation("Currently i'm making API call");
        //            var response = httpClientService.GetAsync(new Uri(sessionLink)).Result;
        //            logger.LogInformation("Currently I got making API call");
        //            //using (var response = httpClientService.GetAsync(new Uri(sessionLink)).Result)
        //            //{
        //            result = response.Content.ReadAsStringAsync().Result;
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.LogError(ex, ex.Message);
        //        }
        //        if (result != null)
        //        {
        //            string info = Convert.ToString(result);
        //            logger.LogInformation(info);
        //            Console.WriteLine(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, ex.Message);
        //    }
        //}
    }
}
