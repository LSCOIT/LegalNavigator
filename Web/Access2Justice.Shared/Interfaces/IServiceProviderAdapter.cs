using Access2Justice.Shared.Models.Integration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IServiceProviderAdapter
    {
        Task<List<string>> GetServiceProviders(string topicName);
        Task<ServiceProvider> GetServiceProviderDetails(string providerId);
    }
}
