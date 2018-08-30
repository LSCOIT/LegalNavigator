using System.Collections.Generic;

namespace Access2Justice.Shared
{
    public interface IPersonalizedPlanEngine
    {
        bool IsConditionSatisfied(Dictionary<string, string> ANDvariables, Dictionary<string, string> ORvariables, Dictionary<string, string> inputVars);
        Dictionary<string, string> MatchAnswersVarsWithPersonalizedPlanVars(string logic, Dictionary<string, string> answerVars);
    }
}