using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        Task<dynamic> GetTopicsAsync(string keyword);
        Task<dynamic> GetResourcesAsync(dynamic resourcesIds);
        Task<dynamic> GetTopLevelTopicsAsync();
        Task<dynamic> GetSubTopicsAsync(string ParentTopicId);
        Task<dynamic> GetResourceAsync(string ParentTopicId);
        Task<dynamic> GetDocumentAsync(string id);
        Task<dynamic> GetFirstPageResource(ResourceFilter resourceFilter, string queryFilter);
        Task<dynamic> GetPagedResourcesAsync(ResourceFilter resourceFilter, string queryFilter);
        string FilterPagedResource(ResourceFilter resourceFilter);
    }
}