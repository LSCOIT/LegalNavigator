using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Compiler : ICompile
    {
        public A2JPersonalizedPlan Compile(A2JPersonalizedPlan personalizedPlan, Dictionary<string, string> evaluatedUserAnswers)
        {
            // Todo:@Alaa implement this

            var breakpoint = string.Empty; // Todo:@Alaa - remove this temp code

            return new A2JPersonalizedPlan()
            {
                Active = "true"
            };
        }
    }
}
