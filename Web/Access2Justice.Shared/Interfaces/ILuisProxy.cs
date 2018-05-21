using System.Threading.Tasks;
using System.Collections.Generic;

namespace Access2Justice.Shared
{
    public interface ILuisProxy
    {
        Task<IntentWithScore> GetLuisIntent(LuisInput luisInput);
        IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore);
    }
}
