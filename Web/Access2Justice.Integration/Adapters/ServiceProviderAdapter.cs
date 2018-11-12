using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Adapters
{
    public class ServiceProviderAdapter : IServiceProviderAdapter
    {
        public ServiceProvider GetServiceProviderDetails(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ServiceProvider>> GetServiceProviders(string organizationalUnit, Topic topic)
        {
            ServiceProviderAdaptee serviceProviderAdaptee = new ServiceProviderAdaptee();
            return await serviceProviderAdaptee.GetServiceProviders(organizationalUnit, null).ConfigureAwait(false);           
        }
    }

}

