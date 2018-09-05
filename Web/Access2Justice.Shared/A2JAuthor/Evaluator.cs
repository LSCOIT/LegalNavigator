using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Evaluator : IEvaluate
    {
        public bool Evaluate(Dictionary<string, string> answersDic, OrderedDictionary logicDic, Func<bool, bool, bool> myFunc)
        {
            var result = false;
            var final = false;

            if (myFunc(true, false) == false)
            {
                final = true;
            }

            object[] keys = new object[logicDic.Keys.Count];
            logicDic.Keys.CopyTo(keys, 0);
            for (int i = 0; i < logicDic.Keys.Count - 1; i++)
            {

                var value1 = answersDic.Where(x => x.Key == (string)keys[i] && x.Value == (string)logicDic[i]).Any();
                var value2 = answersDic.Where(x => x.Key == (string)keys[i + 1] && x.Value == (string)logicDic[i + 1]).Any();

                result = myFunc(value1, value2);
                final = myFunc(result, final);
            }

            return final;
        }
    }
}
