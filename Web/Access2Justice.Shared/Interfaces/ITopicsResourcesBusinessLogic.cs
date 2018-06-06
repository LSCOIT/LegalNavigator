using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        Task<dynamic> GetTopicAsync(string keyword);
        Task<dynamic> GetResourcesAsync(string resourcesIds);


        Task<dynamic> GetTopLevelTopicsAsync();
        Task<dynamic> GetSubTopicsAsync(string ParentTopicId);
        Task<dynamic> GetReourceAsync(string ParentTopicId);
        Task<dynamic> GetDocumentAsync(string id);
    }
}