using Access2Justice.Shared.Interfaces.A2JAuthor;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Evaluator : IEvaluate
    {
        public bool Evaluate(Dictionary<string, string> answers, OrderedDictionary logic, Func<bool, bool, bool> answersLogicEvaluator)
        {
            if(logic.Count == 0)
            {
                return false;
            }
            else if(logic.Count == 1)
            {
                return answers.Where(x => x.Key == (string)logic[0] && x.Value == (string)logic[0]).Any();
            }

            var initialResult = false;
            var finalResult = !answersLogicEvaluator(true, false);

            object[] keys = new object[logic.Keys.Count];
            logic.Keys.CopyTo(keys, 0);

            for (int i = 0; i < logic.Keys.Count - 1; i++)
            {
                var valueN = answers.Where(x => x.Key == (string)keys[i] && x.Value == (string)logic[i]).Any();
                var valueNPlus1 = answers.Where(x => x.Key == (string)keys[i + 1] && x.Value == (string)logic[i + 1]).Any();

                initialResult = answersLogicEvaluator(valueN, valueNPlus1);
                finalResult = answersLogicEvaluator(initialResult, finalResult);
            }

            return finalResult;
        }
    }
}
