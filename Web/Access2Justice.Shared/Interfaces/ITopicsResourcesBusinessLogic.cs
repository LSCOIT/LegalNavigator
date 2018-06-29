using System.Threading.Tasks;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        Task<dynamic> GetTopicsAsync(string keyword, Location location);
        Task<dynamic> GetResourcesAsync(dynamic resourcesIds);
        Task<dynamic> GetTopLevelTopicsAsync();
        Task<dynamic> GetSubTopicsAsync(string parentTopicId);
        Task<dynamic> GetResourceAsync(string parentTopicId);
        Task<dynamic> GetDocumentAsync(string id);
        Task<dynamic> GetBreadcrumbDataAsync(string id);
        Task<dynamic> GetPagedResourceAsync(ResourceFilter resourceFilter);
        Task<dynamic> ApplyPaginationAsync(ResourceFilter resourceFilter);
        Task<dynamic> GetResourcesCountAsync(ResourceFilter resourceFilter);
        Task<dynamic> GetPlanDataAsync(string planId);
        Task<dynamic> GetPersonalizedResourcesAsync(ResourceFilter resourceFilter);
    }
}