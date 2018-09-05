using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface ICompile
    {
        A2JPersonalizedPlan Compile(A2JPersonalizedPlan personalizedPlan, Dictionary<string, string> evaluatedUserAnswers);
    }
}