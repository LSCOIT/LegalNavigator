using System.Threading.Tasks;
using System.Collections.Generic;

namespace Access2Justice.Shared
{
    public interface ILuisProxy
    {
        Task<string> GetIntents(string query);
    }
}
