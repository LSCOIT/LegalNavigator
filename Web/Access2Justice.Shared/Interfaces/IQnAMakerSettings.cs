using System;

namespace Access2Justice.Shared.Interfaces
{
    public interface IQnAMakerSettings
    {
        Uri Endpoint { get; }
        string KnowledgeId { get; }
        string AuthorizationKey { get; }
    }
}
