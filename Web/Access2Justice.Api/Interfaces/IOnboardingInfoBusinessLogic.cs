using Access2Justice.Shared.Models.Integration;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IOnboardingInfoBusinessLogic
    {
        OnboardingInfo GetOnboardingInfo(string organizationType);
    }
}
