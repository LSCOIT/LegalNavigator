using System.Threading.Tasks;
using Access2Justice.Shared.Models.Integration;

namespace Access2Justice.Integration.Interfaces
{
    public interface IServiceProviderAdapter
    {
        Task<dynamic> GetServiceProviders(string TopicName);

        ServiceProvider GetServiceProviderDetails(string id);
    }
}