using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;

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

        public JObject Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            return compiler.Compile(
                personalizedPlan, 
                parser.Parse(userAnswers));
        }

        //public PersonalizedPlanSteps Map(A2JPersonalizedPlan personalizedPlan)
        //{
        //    // Todo:@Alaa I want to breakdown the a2j personalized plan to an intermediary dto object (other than PersonalizedPlanSteps)
        //    // to make it easier for the personalized plan business logic to map to PersonalizedActionPlanViewModel.
        //    throw new NotImplementedException();
        //}
    }
}
