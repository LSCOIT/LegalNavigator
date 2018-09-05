using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Access2Justice.Shared.A2JAuthor
{
    public class PersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IParse parser;
        private readonly ICompile compiler;

        public PersonalizedPlanEngine(IParse parser, ICompile compiler)
        {
            this.parser = parser;
            this.compiler = compiler;
        }

        public A2JPersonalizedPlan Build(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            Dictionary<string, string> setVars = parser.Parse(userAnswers);
            //A2JPersonalizedPlan compiledUserPlan = evaluater.Evaluate((setVars);

            return null;
        }

        private Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            var parsed = parser.Parse(curatedExperienceAnswers);

            return parsed;
        }

        private A2JPersonalizedPlan Compile(Dictionary<string, string> evaluatedUserAnswers)
        {
            throw new System.NotImplementedException();
        }
    }
}
