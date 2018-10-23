using System.Collections.Generic;
using Access2Justice.Shared.A2JAuthor;
using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IA2JAuthorLogicParser
    {
        Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers);
    }
}