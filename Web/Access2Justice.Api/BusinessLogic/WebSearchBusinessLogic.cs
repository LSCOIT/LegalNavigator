using Access2Justice.Shared;
using System;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using System.Globalization;
using System.Net;

namespace Access2Justice.Api.BusinessLogic
{
    public class WebSearchBusinessLogic : IWebSearchBusinessLogic
    {
        private IBingSettings bingSettings;
        private IHttpClientService httpClientService;

        public WebSearchBusinessLogic(IHttpClientService httpClientService, IBingSettings bingSettings)
        {
            this.bingSettings = bingSettings;
            this.httpClientService = httpClientService;
        }

        public async Task<dynamic> SearchWebResourcesAsync(Uri uri)
        {
            var httpResponseMessage = await httpClientService.GetDataAsync(uri, bingSettings.SubscriptionKey);
           
            if (httpResponseMessage.Content.Headers.ContentLength == 0)
            {
                return System.Net.HttpStatusCode.NoContent;
            }
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }        
    }
}