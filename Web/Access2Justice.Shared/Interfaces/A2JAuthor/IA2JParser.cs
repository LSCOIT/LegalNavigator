using System.Collections.Generic;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IParse
    {     
        Dictionary<string, string> Evaluate(CuratedExperienceAnswers curatedExperienceAnswers);
        A2JPersonalizedPlan Compile(Dictionary<string, string> evaluatedUserAnswers);
    }
}