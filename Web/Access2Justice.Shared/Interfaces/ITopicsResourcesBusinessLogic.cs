﻿using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ITopicsResourcesBusinessLogic
    {
        Task<dynamic> GetTopicsAsync(string keyword, Location location);
        Task<dynamic> GetResourcesAsync(dynamic resourcesIds);
        Task<dynamic> GetTopLevelTopicsAsync(Location location);
        Task<dynamic> GetSubTopicsAsync(string parentTopicId, Location location);
        Task<dynamic> GetResourceByIdAsync(string id,Location location);
        Task<dynamic> GetResourceAsync(string parentTopicId,Location location);
        Task<dynamic> GetDocumentAsync(string id,Location location);
        Task<dynamic> GetBreadcrumbDataAsync(string id);
        Task<dynamic> GetTopicDetailsAsync(string topicName);
        Task<dynamic> GetResourceDetailAsync(string resourceName, string resourceType);
        dynamic GetReferences(dynamic resource);
        dynamic GetTopicTags(dynamic tagValues);
        dynamic GetLocations(dynamic locationValues);
        dynamic GetConditions(dynamic conditionValues);
        dynamic GetParentTopicIds(dynamic ParentTopicIdValues);
        dynamic GetQuickLinks(dynamic quickLinksValues);
        Task<IEnumerable<object>> UpsertResourcesUploadAsync(string path);
        Task<IEnumerable<object>> UpsertResourceDocumentAsync(dynamic resource);
        dynamic UpsertResourcesForms(dynamic resource);
        dynamic UpsertResourcesActionPlans(dynamic resource);
        dynamic UpsertResourcesArticles(dynamic resource);
        dynamic UpsertResourcesVideos(dynamic resource);
        dynamic UpsertResourcesOrganizations(dynamic resource);
        dynamic UpsertResourcesEssentialReadings(dynamic resource);
        Task<IEnumerable<object>> UpsertTopicsUploadAsync(string path);
        Task<IEnumerable<object>> UpsertTopicDocumentAsync(dynamic topic);
        dynamic UpsertTopics(dynamic topic);
        Task<dynamic> GetPagedResourceAsync(ResourceFilter resourceFilter);
        Task<dynamic> ApplyPaginationAsync(ResourceFilter resourceFilter);
        Task<dynamic> GetResourcesCountAsync(ResourceFilter resourceFilter);
        Task<dynamic> GetPersonalizedResourcesAsync(ResourceFilter resourceFilter);
        Task<dynamic> GetOrganizationsAsync(Location location);
    }
}