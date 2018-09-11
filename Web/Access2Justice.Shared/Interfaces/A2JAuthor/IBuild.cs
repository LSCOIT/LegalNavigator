using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IPersonalizedPlanFactory
    {
        A2JPersonalizedPlan Build(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers);
    }
}