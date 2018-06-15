using Access2Justice.Shared.Models;
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
        //Added for Topic and Resource Tools API
        Task<dynamic> GetTopicAsync(string topicName);
        Task<dynamic> GetTopicDetailsAsync(string topicName);
        Task<dynamic> GetResourceDetailAsync(string resourceName, string resourceType);
        Task<IEnumerable<Topic>> GetTopicMandatoryDetailsAsync(string topicName);
        Task<IEnumerable<object>> CreateTopicsAsync(string path);
        Task<object> CreateTopicDocumentAsync(Topic topic);
        Task<IEnumerable<object>> CreateResourcesAsync(string type, string path);
        Task<IEnumerable<object>> CreateResourceDocumentAsync(dynamic resource);
    }
}