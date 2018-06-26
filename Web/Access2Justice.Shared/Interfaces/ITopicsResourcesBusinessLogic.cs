using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<dynamic> GetBreadcrumbDataAsync(string id);
        Task<dynamic> GetResourceActionPlanAsync(string ParentTopicId, string filterValue);
    }
}