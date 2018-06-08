using Access2Justice.Shared.Models;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        Task<dynamic> GetTopicAsync(string keyword);
        Task<dynamic> GetResourcesAsync(string resourcesIds);
        Task<dynamic> GetTopicsAsync();
        Task<dynamic> GetSubTopicsAsync(string ParentTopicId);
        Task<dynamic> GetReourceDetailAsync(string ParentTopicId);
        Task<dynamic> GetDocumentData(string id);
        Task<dynamic> GetFirstPageResource(ResourceFilter resourceFilter, string queryFilter);
        Task<dynamic> GetPagedResourcesAsync(ResourceFilter resourceFilter, string queryFilter);
        string FilterPagedResource(ResourceFilter resourceFilter);
    }
}