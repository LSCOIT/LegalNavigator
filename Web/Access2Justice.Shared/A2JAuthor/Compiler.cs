using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Compiler : ICompile
    {
        public List<JToken> Compile(JObject personalizedPlan, Dictionary<string, string> evaluatedUserAnswers)
        {
            var stepsInScope = new List<JToken>();

            var root = personalizedPlan
                .Properties()
                .GetArrayValue("rootNode").FirstOrDefault();

                foreach (var child in root.GetValueAsArray("children"))
                {
                    var states = child.GetArrayValue("state");
                    foreach (var state in states)
                    {
                        foreach (var answer in evaluatedUserAnswers)
                        {
                            if (answer.Key == state.GetValue("leftOperand") && answer.Value == state.GetValue("operator"))
                            {
                                stepsInScope.Add(child);
                            }
                        }
                    }
                }

            return stepsInScope;
        }
    }
}
