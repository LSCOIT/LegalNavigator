using Access2Justice.Shared;
using System;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using System.Globalization;

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

        public async Task<dynamic> SearchWebResourcesAsync(string searchTerm)
        {            
            var uri = string.Format(CultureInfo.InvariantCulture, bingSettings.BingSearchUrl.OriginalString, searchTerm,bingSettings.CustomConfigId);
            
            var httpResponseMessage = await httpClientService.GetDataAsync(new Uri(uri), bingSettings.SubscriptionKey);
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            
            return responseContent;
        }
    }
}