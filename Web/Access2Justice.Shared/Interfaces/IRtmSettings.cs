using System;

namespace Access2Justice.Shared.Interfaces
{
    public interface IRtmSettings
    {        
        string ApiKey { get; }
        Uri SessionURL { get; }
        Uri ServiceProviderURL { get; }
        Uri ServiceProviderDetailURL { get; }

    }
}
