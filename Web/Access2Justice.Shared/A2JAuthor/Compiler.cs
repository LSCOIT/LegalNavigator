using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Compiler : ICompile
    {
        A2JPersonalizedPlan ICompile.Compile(Dictionary<string, string> evaluatedUserAnswers)
        {
            return new A2JPersonalizedPlan()
            {
                Active = "true"
            };
        }
    }
}
