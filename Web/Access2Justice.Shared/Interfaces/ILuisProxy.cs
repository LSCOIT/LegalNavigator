using System.Threading.Tasks;
using System.Collections.Generic;

namespace Access2Justice.Shared
{
    public interface ILuisProxy
    {
        Task<string> GetIntents(string query);
        IntentWithScore ParseLuisIntent(string LuisResponse);
        IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore);
    }
}
