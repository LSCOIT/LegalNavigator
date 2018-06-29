using Access2Justice.Shared.Models;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ILuisBusinessLogic
    {
        int ApplyThreshold(IntentWithScore intentWithScore);
        Task<dynamic> GetInternalResourcesAsync(string intent,Location location);
        IntentWithScore ParseLuisIntent(string LuisResponse);
        Task<dynamic> GetResourceBasedOnThresholdAsync(LuisInput luisInput);
        Task<dynamic> GetWebResourcesAsync(string query);
    }
}
