using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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

        public List<JToken> Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            return compiler.Compile(personalizedPlan, parser.Parse(userAnswers));
        }
    }
}
