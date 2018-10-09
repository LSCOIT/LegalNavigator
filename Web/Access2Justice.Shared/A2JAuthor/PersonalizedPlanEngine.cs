using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class A2JAuthorPersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IParse parser;
        private readonly ICompile compiler;

        public A2JAuthorPersonalizedPlanEngine(IParse parser, ICompile compiler)
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
