using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        Task<dynamic> GetTopicAsync(string keyword);
        Task GetResources(IEnumerable<string> resourcesIds);
    }
}
