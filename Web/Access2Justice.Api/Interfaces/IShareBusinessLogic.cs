using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IShareBusinessLogic
    {
        Task<ShareViewModel> ShareResourceDataAsync(ShareInput shareInput);
        Task<dynamic> ShareResourceAsync(SendLinkInput sendLinkInput);
        Task<dynamic> TakeResourceAsync(SendLinkInput sendLinkInput);
        Task<ShareViewModel> CheckPermaLinkDataAsync(ShareInput shareInput);
        Task<object> UnshareResourceDataAsync(ShareInput shareInput);
        Task UpdatePlanIsSharedStatus(string planId, bool isShared);
        Task<object> GetPermaLinkDataAsync(string permaLink);
    }
}
