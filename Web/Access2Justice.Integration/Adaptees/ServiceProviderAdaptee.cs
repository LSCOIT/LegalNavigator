using Access2Justice.Shared.Interfaces;
using System.Threading.Tasks;
using Access2Justice.Shared;
using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Access2Justice.Integration.Adapters
{
    public class ServiceProviderAdaptee
    {
        public async Task<dynamic> GetRTMServiceProvidersAsync(string TopicName, IRtmSettings rtmSettings, IHttpClientService httpClient)
        {
            try
            {
                string sessionUrl = string.Format(CultureInfo.InvariantCulture, rtmSettings.SessionURL.OriginalString, rtmSettings.RtmApiKey);
                var sResponse = await httpClient.GetAsync(new Uri(sessionUrl)).ConfigureAwait(false);
                string session = sResponse.Content.ReadAsStringAsync().Result;
                dynamic sessionJson = JsonConvert.DeserializeObject(session);
                string sessionId = sessionJson[0][Constants.RTMSessionId];
                if (!string.IsNullOrEmpty(sessionId))
                {
                    string serviceProviderUrl = string.Format(CultureInfo.InvariantCulture, rtmSettings.ServiceProviderURL.OriginalString, 
                                                              rtmSettings.RtmApiKey, sessionId);
                    var spResponse = await httpClient.GetAsync(new Uri(serviceProviderUrl)).ConfigureAwait(false);
                    string serviceProvider = spResponse.Content.ReadAsStringAsync().Result;
                    if (string.IsNullOrEmpty(serviceProvider))
                    {

                    }
                }
                return sessionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

