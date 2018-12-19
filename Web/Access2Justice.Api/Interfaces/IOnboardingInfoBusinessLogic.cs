using Access2Justice.Shared.Models.Integration;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IOnboardingInfoBusinessLogic
    {
        Task<OnboardingInfo> GetOrganizationOnboardingInfo(string organizationId);
        Task<object> PostOnboardingInfo(OnboardingInfo onboardingInfo);
        
    }
}
