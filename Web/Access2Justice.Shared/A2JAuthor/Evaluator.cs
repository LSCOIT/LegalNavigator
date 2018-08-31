using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Evaluator : IEvaluate
    {
        public Dictionary<string, string> Evaluate(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("var1", "value1");

            return dic;
        }
    }
}
