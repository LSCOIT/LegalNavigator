using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Interfaces
{
    public interface IBingSettings
    {
        Uri BingSearchUrl { get; }
        string SubscriptionKey { get; }
        string CustomConfigId { get; }
        Int16 DefaultCount { get; }
        Int16 DefaultOffset { get; }
    }
}
