using System;

namespace Access2Justice.Shared.Interfaces
{
    public interface ICosmosDbSettings
    {
        string AuthKey { get; }
        Uri Endpoint { get; }
        string DatabaseId { get; }
        string TopicsCollectionId { get; }
        string ResourcesCollectionId { get; }
        string ProfilesCollectionId { get; }
        int PageResultsCount { get; }
        string CuratedExperiencesCollectionId { get; }
        string ActionPlansCollectionId { get; }
        string StaticResourcesCollectionId { get; }
        string UserResourcesCollectionId { get; }
        string A2JAuthorDocsCollectionId { get; }
        string RolesCollectionId { get; }
        string GuidedAssistantAnswersCollectionId { get; }
        string StateProvincesCollectionId { get; }
    }
}
