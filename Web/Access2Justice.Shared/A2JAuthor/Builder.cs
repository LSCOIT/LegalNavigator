using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Builder : IBuild
    {
        private readonly IParse parser;
        private readonly ICompile compiler;

        public Builder(IParse parser, ICompile compiler)
        {
            this.parser = parser;
            this.compiler = compiler;
        }

        public A2JPersonalizedPlan Build(A2JPersonalizedPlan personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            var evaluatedAnswers = parser.Parse(userAnswers);
            var compiledPlan = compiler.Compile(personalizedPlan, evaluatedAnswers);

             // Todo:@Alaa remove this
            return compiledPlan;
        }
    }
}
