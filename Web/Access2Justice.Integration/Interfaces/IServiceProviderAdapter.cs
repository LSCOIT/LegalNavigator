using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models.Integration;

namespace Access2Justice.Integration.Interfaces
{
    public interface IServiceProviderAdapter
    {
        Task<List<string>> GetServiceProviders(string topicName);
        Task<ServiceProvider> GetServiceProviderDetails(string providerId);
    }
}