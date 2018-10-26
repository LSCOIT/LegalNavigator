using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IShareBusinessLogic
    {
        Task<ShareViewModel> ShareResourceDataAsync(ShareInput shareInput);
        Task<ShareViewModel> CheckPermaLinkDataAsync(ShareInput shareInput);
        Task<object> UnshareResourceDataAsync(ShareInput shareInput);
        Task<Document> UpdatePlanIsSharedStatus(string planId, bool isShared);
        Task<object> GetPermaLinkDataAsync(string permaLink);
    }
}
