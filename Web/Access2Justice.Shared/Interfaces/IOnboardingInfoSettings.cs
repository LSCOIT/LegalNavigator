using System;

namespace Access2Justice.Shared
{
    public interface IOnboardingInfoSettings
    {
        string HostAddress { get; }
        string PortNumber { get; }
        string UserName { get; }
        string Password { get; }
        string FromAddress { get; }
        string Subject { get; }
        string FallbackToAddress { get; }
        string FallbackBodyMessage { get; }
    }
}
