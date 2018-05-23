﻿using Access2Justice.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.Shared.Interfaces;
using System.Globalization;
using Newtonsoft.Json;

namespace Access2Justice.Api.BusinessLogic
{
    public class WebSearchBusinessLogic : IWebSearchBusinessLogic
    {
        private IBingSettings _bingSettings;
        private IHttpClientService _httpClientService;

        public WebSearchBusinessLogic(IHttpClientService httpClientService, IBingSettings bingSettings)
        {
            _bingSettings = bingSettings;
            _httpClientService = httpClientService;
        }

        public async Task<dynamic> GetWebResources(string searchTerm)
        {
            var uri = string.Format(CultureInfo.InvariantCulture, _bingSettings.BingSearchUrl.OriginalString, searchTerm,_bingSettings.CustomConfigId);
            // Pull the data from Cognitive CustomSearch AI
            var httpResponseMessage = await _httpClientService.GetDataAsync(new Uri(uri), _bingSettings.SubscriptionKey);
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            //returns webPage.name, url, displayUrl,snipper,dateLastCrawled etc.           

            return responseContent;         
        }
    }
}
