using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IA2JAuthorLogicInterpreter
    {
        bool Interpret(Dictionary<string, string> answers, OrderedDictionary logic, Func<bool, bool, bool> answersLogicEvaluator);
    }
}