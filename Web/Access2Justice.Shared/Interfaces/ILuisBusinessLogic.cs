using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ILuisBusinessLogic
    {
        int ApplyThreshold(IntentWithScore intentWithScore);
        Task<dynamic> GetInternalResourcesAsync(string intent);
        IntentWithScore ParseLuisIntent(string LuisResponse);
        Task<dynamic> GetResourceBasedOnThresholdAsync(string query);
        Task<dynamic> GetWebResourcesAsync(string query);
    }
}
