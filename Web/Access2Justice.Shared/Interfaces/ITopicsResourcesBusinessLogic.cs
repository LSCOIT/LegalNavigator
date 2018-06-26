﻿using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        //string Path { get; }
        Task<dynamic> GetTopicsAsync(string keyword);
        Task<dynamic> GetResourcesAsync(dynamic resourcesIds);
        Task<dynamic> GetTopLevelTopicsAsync();
        Task<dynamic> GetSubTopicsAsync(string ParentTopicId);
        Task<dynamic> GetResourceAsync(string ParentTopicId);
        Task<dynamic> GetDocumentAsync(string id);
        Task<dynamic> GetBreadcrumbDataAsync(string id);
        Task<dynamic> GetTopicDetailsAsync(string topicName);
        Task<dynamic> GetResourceDetailAsync(string resourceName, string resourceType);
        dynamic GetReferences(dynamic resource);
        dynamic GetReferenceTags(dynamic tagValues);
        dynamic GetLocations(dynamic locationValues);
        dynamic GetConditions(dynamic conditionValues);
        dynamic GetParentTopicIds(dynamic ParentTopicIdValues);
        Task<IEnumerable<object>> CreateResourcesUploadAsync(string path);
        Task<IEnumerable<object>> CreateResourceDocumentAsync(dynamic resource);
        dynamic CreateResourcesForms(dynamic resource);
        dynamic CreateResourcesActionPlans(dynamic resource);
        dynamic CreateResourcesArticles(dynamic resource);
        dynamic CreateResourcesVideos(dynamic resource);
        dynamic CreateResourcesOrganizations(dynamic resource);
        dynamic CreateResourcesEssentialReadings(dynamic resource);
        Task<IEnumerable<object>> CreateTopicsUploadAsync(string path);
        Task<IEnumerable<object>> CreateTopicDocumentAsync(dynamic topic);
        dynamic CreateTopics(dynamic topic);
    }
}