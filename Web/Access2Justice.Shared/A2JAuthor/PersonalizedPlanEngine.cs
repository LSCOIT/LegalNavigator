using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class PersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IParse parser;

        public PersonalizedPlanEngine(IParse parser)
        {
            this.parser = parser;
        }

        public A2JPersonalizedPlan ExtractStepsInScope(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            Dictionary<string, string> setVars = parser.Evaluate(userAnswers);
            A2JPersonalizedPlan compiledUserPlan = parser.Compile(setVars);

            return compiledUserPlan;
        }
    }
}
