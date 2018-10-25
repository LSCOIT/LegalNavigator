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
        string UserProfilesCollectionId { get; }
        int PageResultsCount { get; }
        string CuratedExperiencesCollectionId { get; }
        string CuratedExperienceAnswersCollectionId { get; }
        string PersonalizedActionsPlanCollectionId { get; }
        string StaticResourcesCollectionId { get; }
        string UserSavedResourcesCollectionId { get; }
        string UserResourcesCollectionId { get; }
        string A2JAuthorTemplatesCollectionId { get; }
        string UserRolesCollectionId { get; }
    }
}
