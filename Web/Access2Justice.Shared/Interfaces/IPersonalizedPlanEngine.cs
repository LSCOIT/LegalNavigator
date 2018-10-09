using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Shared.Interfaces
{
    public interface IPersonalizedPlanEngine
    {
        UnprocessedPersonalizedPlan Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers);
    }
}