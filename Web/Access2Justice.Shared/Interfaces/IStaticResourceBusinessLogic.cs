using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IStaticResourceBusinessLogic
    {
        Task<dynamic> GetPageStaticResourceDataAsync(string name, Location location);
        Task<dynamic> UpsertStaticHomePageDataAsync(HomeContent homePageContent, Location location);
        Task<dynamic> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromiseContent, Location location);
        Task<dynamic> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent, Location location);
        Task<dynamic> UpsertStaticNavigationDataAsync(Navigation navigationContent, Location location);
    }
}