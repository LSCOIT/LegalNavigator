using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Adapters
{
    public class ServiceProviderAdapter : IServiceProviderAdapter
    {
        private readonly IRtmSettings rtmSettings;
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;

        public ServiceProviderAdapter(IRtmSettings rtmSettings, IWebSearchBusinessLogic webSearchBusinessLogic)
        {
            this.rtmSettings = rtmSettings;
            this.webSearchBusinessLogic = webSearchBusinessLogic;
        }

        public async Task<IEnumerable<ServiceProvider>> GetServiceProviders(string organizationalUnit, Topic topic)
        {
            //https://www.referweb.net/pubres/api/ServiceProviders/?ip={ apikey: 'DEEZ3KKT', st: 'c',  catid: '364', sn: '', zip: '99504', county: '', sid: '1ZWQL0mwagl6hB9yCyGV'   }
            string rtmApi = "ServiceProviders";
            string rtmSessionId = await GetSessionId().ConfigureAwait(false);
            string otherParams = "st=c,catid=364,sn='',zip=99504,county=''"; // hardcoded for demo as per above url
            var uri = string.Format(CultureInfo.InvariantCulture, rtmSettings.RtmApiUrl.OriginalString, rtmApi, organizationalUnit, topic, rtmSettings.RtmApiKey, otherParams, rtmSessionId);
            dynamic response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri)).ConfigureAwait(false);
            return response;
        }

        public async Task<ServiceProvider> GetServiceProviderDetails(string id)
        {
            // https://www.referweb.net/pubres/api/ProviderDetail/?ip={ apikey: 'DEEZ3KKT', locid: '677',  svid: '188', ssid: '45608' , sid: '1ZWQL0mwagl6hB9yCyGV'  } 
            string rtmApi = "ProviderDetail";
            string rtmSessionId = await GetSessionId().ConfigureAwait(false);
            string otherParams = "locid=677,svid=188,ssid=45608"; // hardcoded for demo as per above url
            var uri = string.Format(CultureInfo.InvariantCulture, rtmSettings.RtmApiUrl.OriginalString, rtmApi, rtmSettings.RtmApiKey, otherParams, rtmSessionId);
            dynamic response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri)).ConfigureAwait(false);
            return response;
        }

        public async Task<string> GetSessionId()
        {
            string rtmApi = "GetSessionID";
            var uri = string.Format(CultureInfo.InvariantCulture, rtmSettings.RtmApiUrl.OriginalString, rtmApi, rtmSettings.RtmApiKey);
            dynamic response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri)).ConfigureAwait(false);
            return response;
        }

        public async Task<string> GetCategories()
        {
            string rtmApi = "GetCategories";
            string rtmSessionId = await GetSessionId().ConfigureAwait(false);
            var uri = string.Format(CultureInfo.InvariantCulture, rtmSettings.RtmApiUrl.OriginalString, rtmApi, rtmSettings.RtmApiKey, rtmSessionId);
            dynamic response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri)).ConfigureAwait(false);
            return response;
        }
        
    }
}
