using System.Collections.Generic;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces
{
    public interface ILuisBusinessLogic
    {
        bool IsIntentAccurate(IntentWithScore intentWithScore);
        Task<dynamic> GetInternalResourcesAsync(string intent, LuisInput luisInput, IEnumerable<string> relevantIntents);
        IntentWithScore ParseLuisIntent(string LuisResponse, Location location = null);
        Task<dynamic> GetResourceBasedOnThresholdAsync(LuisInput luisInput);
        Task<dynamic> GetWebResourcesAsync(string query);
    }
}
