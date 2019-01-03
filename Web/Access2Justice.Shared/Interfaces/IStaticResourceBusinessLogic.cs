using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IStaticResourceBusinessLogic
    {
        Task<dynamic> GetPageStaticResourcesDataAsync(Location location);
        Task<dynamic> UpsertStaticHomePageDataAsync(HomeContent homePageContent);
        Task<dynamic> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromiseContent);
        Task<dynamic> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent);
        Task<dynamic> UpsertStaticNavigationDataAsync(Navigation navigationContent);
        Task<dynamic> UpsertStaticAboutPageDataAsync(AboutContent aboutContent);
        Task<dynamic> UpsertStaticPersnalizedPlanPageDataAsync(PersonalizedPlanContent personalizedPlanContent);
        Task<dynamic> UpsertStaticGuidedAssistantPageDataAsync(GuidedAssistantPageContent guidedAssistantPageContent); 

    }
}