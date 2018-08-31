using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IEvaluate
    {
        Dictionary<string, string> Evaluate(CuratedExperienceAnswers curatedExperienceAnswers);
    }
}
