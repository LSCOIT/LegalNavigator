using System.Collections.Generic;
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
        Task<dynamic> GetBreadCrumbDataByIdAsync(string id);
    }
}