using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IPersonalizedPlanEngine
    {
        A2JPersonalizedPlan Build(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers);
        PersonalizedPlanSteps Map(A2JPersonalizedPlan personalizedPlan);
    }
}