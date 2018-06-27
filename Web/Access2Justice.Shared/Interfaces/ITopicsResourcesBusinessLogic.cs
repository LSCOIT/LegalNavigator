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
        Task<dynamic> GetSubTopicsAsync(string parentTopicId);
        Task<dynamic> GetResourceAsync(string parentTopicId);
        Task<dynamic> GetDocumentAsync(string id);
        Task<dynamic> GetBreadcrumbDataAsync(string id);
        Task<dynamic> GetPlanDataAsync(string planId);
        Task<dynamic> GetPersonalizedResourcesAsync(ResourceFilter resourceFilter);
    }
}