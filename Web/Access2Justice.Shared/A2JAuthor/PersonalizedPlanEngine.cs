using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;

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
            return compiler.Compile(
                personalizedPlan, 
                parser.Parse(userAnswers));
        }

        public PersonalizedPlanSteps Map(A2JPersonalizedPlan personalizedPlan)
        {


            var breakpoint = string.Empty; // Todo:@Alaa - remove this temp code
            return null;
        }
    }
}
