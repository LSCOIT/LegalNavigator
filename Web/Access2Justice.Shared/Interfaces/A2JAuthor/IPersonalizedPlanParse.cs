using System.Collections.Generic;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IPersonalizedPlanParse
    {
        Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers);
    }
}