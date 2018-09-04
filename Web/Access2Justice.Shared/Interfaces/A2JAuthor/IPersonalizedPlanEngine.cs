using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public interface IPersonalizedPlanEngine
    {
        A2JPersonalizedPlan ExtractStepsInScope(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers);
    }
}