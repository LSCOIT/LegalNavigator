using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class PersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IIfElseParser parserOld;
        private readonly IParse parser;

        public PersonalizedPlanEngine(IIfElseParser a2JParser, IParse parser)
        {
            parserOld = a2JParser;
            this.parser = parser;
        }

        public Dictionary<string, string> MatchAnswersVarsWithPersonalizedPlanVars(string logic, Dictionary<string, string> userAnswers)
        {
            var planVars = new Dictionary<string, string>();

            foreach (var statement in logic.IFstatements())
            {
                var computedVars = parserOld.SET(statement.SETvars()).IF(statement.ANDvars()).AND(statement.ANDvars()).IsEqualTo(userAnswers);
                planVars.AddRange(computedVars);
            }
            
            return planVars;
        }

        public bool IsConditionSatisfied(Dictionary<string, string> ANDvariables, Dictionary<string, string> ORvariables,
            Dictionary<string, string> inputVars)
        {
            // Todo:@Alaa implement this logic
            // this is awfully simplistic at this point. only for a POC
            foreach (var inputVar in inputVars)
            {
                if (ANDvariables.Keys.Contains(inputVar.Key) && ANDvariables.Values.Contains(inputVar.Value))
                {
                    return true;
                }
            }

            return false;
        }


        public void MatchAnswersVarsWithPersonalizedPlanVarsV2(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            var evaluatedUserAnswers = parser.Evaluate(userAnswers);
            var compiledUserPlan = parser.Compile(evaluatedUserAnswers);

        }
    }
}
