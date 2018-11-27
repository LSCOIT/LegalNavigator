using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Access2Justice.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Access2Justice.WebJob
{
    public class Functions
    {
        private readonly ILogger<Functions> logger;
        private readonly IHttpClientService httpClient;

        public Functions(ILogger<Functions> logger, IHttpClientService httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;
        }

        public void ProcessQueueMessage([TimerTrigger("* * * * *")]TimerInfo timerInfo)
        {
            logger.LogInformation("Web Job started Running");
            ProcessRTMData();
            logger.LogInformation("Web Job Completed");
        }

        public void ProcessRTMData()
        {
            try
            {
                // #TODO : Will be removing this piece of code will be replacing this call with adapter method call.
                var buffer = System.Text.Encoding.UTF8.GetBytes("");
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                string postUrl = "https://access2justiceintegrationapi.azurewebsites.net/api/adapter?topicName=Family";
                var response = httpClient.PostAsync(new Uri(postUrl), byteContent).Result;
                string result = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(result))
                {
                    var bufferSP = System.Text.Encoding.UTF8.GetBytes(result);
                    var byteContentSP = new ByteArrayContent(bufferSP);
                    byteContentSP.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string spURL = "https://access2justiceintegrationapi.azurewebsites.net/api/service-providers";
                    var spResponse = httpClient.PostAsync(new Uri(spURL), byteContentSP).Result;
                    string spResult = spResponse.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(spResult))
                    {                        
                        logger.LogInformation(spResult);
                        logger.LogInformation("Transaction completed successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);                
            }
            
        }
    }
}
