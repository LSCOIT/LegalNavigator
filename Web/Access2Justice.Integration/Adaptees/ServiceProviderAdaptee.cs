using Access2Justice.Shared.Models.Integration;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared;
using System;

namespace Access2Justice.Integration.Adapters
{
    public class ServiceProviderAdaptee
    {
        public async Task<dynamic> GetRTMServiceProvidersAsync(string TopicName, IRtmSettings rtmSettings, IHttpClientService httpClient)
        {
            try
            {
                var response = await httpClient.GetAsync(rtmSettings.SessionURL).ConfigureAwait(false);
                string sessionId = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(sessionId))
                {

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

