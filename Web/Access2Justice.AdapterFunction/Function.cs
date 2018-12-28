using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Access2Justice.AdapterFunction
{
    public static class Function
    {
        [FunctionName("AdapterFunction")]
        public static void Run([TimerTrigger("* * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            try
            {
                Uri adapterApiUrl = new Uri(Environment.GetEnvironmentVariable("AdapterApiUrl", EnvironmentVariableTarget.Process));
                string topicName = Environment.GetEnvironmentVariable("TopicName", EnvironmentVariableTarget.Process);
                if (adapterApiUrl != null && !string.IsNullOrWhiteSpace(topicName))
                {
                    HttpClient httpClient = new HttpClient();
                    string serviceProviderUrl = string.Format(CultureInfo.InvariantCulture, adapterApiUrl.OriginalString, topicName);
                    var response = httpClient.GetAsync(serviceProviderUrl).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        log.LogInformation("service providers call with ok status code");
                    }
                    log.LogInformation(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex.StackTrace);
            }
        }
    }
}
