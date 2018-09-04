using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public interface IPersonalizedPlanEngine
    {
        bool IsConditionSatisfied(Dictionary<string, string> ANDvariables, Dictionary<string, string> ORvariables, Dictionary<string, string> inputVars);
        Dictionary<string, string> MatchAnswersVarsWithPersonalizedPlanVars(string logic, Dictionary<string, string> answerVars);
         // Todo:@Alaa remove
        void MatchAnswersVarsWithPersonalizedPlanVarsV2(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers);
    }
}