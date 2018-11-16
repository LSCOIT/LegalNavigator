using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Adapters
{
    public class ServiceProviderAdapter : ServiceProviderAdaptee, IServiceProviderAdapter
    {
        private readonly IRtmSettings rtmSettings;
        private readonly IWebSearchBusinessLogic webSearchBusinessLogic;

        public async Task<IEnumerable<ServiceProvider>> GetServiceProviders(string organizationalUnit, Topic topic)
        {
            return await GetRTMServiceProviders(organizationalUnit, null).ConfigureAwait(false);
        }

        public ServiceProvider GetServiceProviderDetails(string id)
        {
            throw new NotImplementedException();
        }
    }
}

