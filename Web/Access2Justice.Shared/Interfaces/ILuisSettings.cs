using System;

namespace Access2Justice.Shared
{
    public interface ILuisSettings
    {
        Uri Endpoint { get;}
        int TopIntentsCount { get;}
        decimal UpperThreshold { get; }
        decimal LowerThreshold { get; }
    }
}
