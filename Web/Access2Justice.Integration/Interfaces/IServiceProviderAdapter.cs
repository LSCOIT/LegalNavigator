using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;

namespace Access2Justice.Integration.Interfaces
{
    public interface IServiceProviderAdapter
    {
        Task<IEnumerable<ServiceProvider>> GetServiceProviders(string organizationalUnit, Topic topic);

        Task<ServiceProvider> GetServiceProviderDetails(string id);
    }
}
