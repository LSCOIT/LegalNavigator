using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Compiler : ICompile
    {
        public A2JPersonalizedPlan Compile(Dictionary<string, string> evaluatedUserAnswers)
        {
            return new A2JPersonalizedPlan()
            {
                Active = "true"
            };
        }
    }
}
