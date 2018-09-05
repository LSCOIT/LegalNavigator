using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IEvaluate
    {
        bool Evaluate(Dictionary<string, string> answers, OrderedDictionary logic, Func<bool, bool, bool> answersLogicEvaluator);
    }
}