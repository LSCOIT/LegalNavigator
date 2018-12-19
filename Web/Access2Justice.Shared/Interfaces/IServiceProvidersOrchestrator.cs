using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IServiceProvidersOrchestrator
    {
        Task<bool> LoadServiceProviders(string topicName);
    }
}