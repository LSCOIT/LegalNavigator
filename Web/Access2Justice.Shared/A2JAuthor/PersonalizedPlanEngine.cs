using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class A2JAuthorPersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IPersonalizedPlanParse parser;
        private readonly IPersonalizedPlanCompile compiler;

        public A2JAuthorPersonalizedPlanEngine(IPersonalizedPlanParse parser, IPersonalizedPlanCompile compiler)
        {
            this.parser = parser;
            this.compiler = compiler;
        }

        public UnprocessedPersonalizedPlan Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            return compiler.Compile(personalizedPlan, parser.Parse(userAnswers));
        }
    }
}
