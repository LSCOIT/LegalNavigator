using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Access2Justice.Shared.A2JAuthor
{
    public class PersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IIfElseParser parser;

        public PersonalizedPlanEngine(IIfElseParser a2JParser)
        {
            parser = a2JParser;
        }

        public Dictionary<string, string> MatchAnswersVarsWithPersonalizedPlanVars(string logic, Dictionary<string, string> answerVars)
        {
            var temp = new IfElseParser();
            return parser.OnIfStatements("").SET(new Dictionary<string, string>()).IF(new Dictionary<string, string>()).AND(new Dictionary<string, string>()).IsEqualTo(new Dictionary<string, string>());
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
    }
}
