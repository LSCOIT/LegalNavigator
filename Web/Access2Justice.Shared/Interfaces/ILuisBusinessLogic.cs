using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ILuisBusinessLogic
    {
        bool IsIntentAccurate(IntentWithScore intentWithScore);
        Task<dynamic> GetInternalResourcesAsync(string intent,Location location, IEnumerable<string> relevantIntents, string resourceType);
        IntentWithScore ParseLuisIntent(string LuisResponse);
        Task<dynamic> GetResourceBasedOnThresholdAsync(LuisInput luisInput);
        Task<dynamic> GetWebResourcesAsync(string query);
    }
}
