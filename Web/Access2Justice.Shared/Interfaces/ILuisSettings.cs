using System;

namespace Access2Justice.Shared
{
    public interface ILuisSettings
    {
        Uri Endpoint { get;}
        string TopIntentsCount { get;}
    }
}
