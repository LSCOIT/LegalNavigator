using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IPersonalizedPlanEngine
    {
        JObject Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers);
        //PersonalizedPlanSteps Map(A2JPersonalizedPlan personalizedPlan);
    }
}