using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IA2JAuthorBusinessLogic
    {
        CuratedExperience ConvertA2JAuthorToCuratedExperience(JObject a2jSchema);
        Task<A2JPersonalizedPlan> GetA2JPersonalizedPlanAsync();
    }
}
