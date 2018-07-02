using System;

namespace Access2Justice.CosmosDb.Interfaces
{
    public interface ICosmosDbSettings
    {
        string AuthKey { get; }
        Uri Endpoint { get; }
        string DatabaseId { get; }
        string TopicCollectionId { get; }
        string ResourceCollectionId { get; }
        string UserProfileCollectionId { get; }
        int PageResultsCount { get; }
        string CuratedExperienceCollectionId { get; }
    }
}