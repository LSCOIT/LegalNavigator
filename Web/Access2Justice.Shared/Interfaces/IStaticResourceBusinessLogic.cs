using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IStaticResourceBusinessLogic
    {
        Task<dynamic> GetPageStaticResourceDataAsync(string name);
        Task<dynamic> UpsertStaticHomePageDataAsync(HomeContent homePageContent);
        Task<dynamic> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromiseContent);
        Task<dynamic> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent);
        Task<dynamic> UpsertStaticHeaderDataAsync(Header headerContent);
        Task<dynamic> UpsertStaticFooterDataAsync(Footer footerContent);
        Task<dynamic> UpsertStaticNavigationDataAsync(Navigation navigationContent);
    }
}