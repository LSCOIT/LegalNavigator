using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ILuisBusinessLogic
    {
        string ApplyThreshold(IntentWithScore intentWithScore);
        Task<dynamic> GetInternalResources(string query);
        IntentWithScore ParseLuisIntent(string LuisResponse);
    }
}
