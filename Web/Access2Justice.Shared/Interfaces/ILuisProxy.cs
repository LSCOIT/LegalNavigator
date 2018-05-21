using System.Threading.Tasks;
using System.Collections.Generic;

namespace Access2Justice.Shared
{
    public interface ILuisProxy
    {
        Task<IntentWithScore> GetIntents(string query);
        IEnumerable<string> FilterLuisIntents(IntentWithScore intentWithScore);
    }
}
