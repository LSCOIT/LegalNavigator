using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IPersonalizedPlanEngine
    {
        Task<UnprocessedPersonalizedPlan> Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers);
    }
}