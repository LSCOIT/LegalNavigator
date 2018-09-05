using System.Collections.Generic;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IParse
    {
        Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers);
    }
}