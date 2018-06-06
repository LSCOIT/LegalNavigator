using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        Task<dynamic> GetTopicsAsync(string keyword);
        Task<dynamic> GetResourcesAsync(string resourcesIds);


        Task<dynamic> GetTopLevelTopicsAsync();
        Task<dynamic> GetSubTopicsAsync(string ParentTopicId);
        Task<dynamic> GetResourceAsync(string ParentTopicId);
        Task<dynamic> GetDocumentAsync(string id);
    }
}