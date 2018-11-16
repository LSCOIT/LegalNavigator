using Access2Justice.Shared.Models.Integration;
using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Interfaces;
using System.Threading.Tasks;
using Access2Justice.Shared;
using System;


namespace Access2Justice.Integration.Adapters
{
    public class ServiceProviderAdapter : ServiceProviderAdaptee, IServiceProviderAdapter
    {
        private readonly IRtmSettings rtmSettings;
        private readonly IHttpClientService httpClient;

        public ServiceProviderAdapter(IRtmSettings rtmSettings, IHttpClientService httpClient)
        {
            this.rtmSettings = rtmSettings;
            this.httpClient = httpClient;
        }

        public async Task<dynamic> GetServiceProviders(string TopicName)
        {
            dynamic result = null; 
            if (string.IsNullOrEmpty(TopicName))
            {
                return result;
            }
            result = await GetRTMServiceProvidersAsync(TopicName, rtmSettings, httpClient).ConfigureAwait(false);
            return result;
        }

        public ServiceProvider GetServiceProviderDetails(string id)
        {
            throw new NotImplementedException();
        }
    }
}

