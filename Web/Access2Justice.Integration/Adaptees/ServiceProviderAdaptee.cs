using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Access2Justice.Integration.Adapters
{
    public class ServiceProviderAdaptee
    {
        private readonly IRtmSettings rtmSettings;
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;

        //public ServiceProviderAdaptee(IRtmSettings rtmSettings, IWebSearchBusinessLogic webSearchBusinessLogic)
        //{
        //    this.rtmSettings = rtmSettings;
        //    this.webSearchBusinessLogic = webSearchBusinessLogic;
        //}

        public async Task<IEnumerable<ServiceProvider>> GetServiceProviders(string organizationalUnit, Topic topic)
        {
            string rtmApi = "ServiceProviders";
            string rtmSessionId = "1ZWQL0mwagl6hB9yCyGV"; // await GetSessionId().ConfigureAwait(false);
            string otherParams = "st=c,catid=364,sn='',zip=99504,county=''"; // hardcoded for demo 
            var uri = string.Format(CultureInfo.InvariantCulture, rtmSettings.RtmApiUrl.OriginalString, rtmApi, rtmSettings.RtmApiKey, otherParams, rtmSessionId);
            dynamic response = await webSearchBusinessLogic.SearchWebResourcesAsync(new Uri(uri)).ConfigureAwait(false);
            List<ServiceProvider> serviceProviders = JsonUtilities.DeserializeDynamicObject<List<ServiceProvider>>(response);
            var serviceProvidersList = serviceProviders.Where(x => x.OrganizationalUnit == organizationalUnit);
            return serviceProvidersList;
        }
    }
}

