﻿using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class A2JParser : IA2JAuthorParser
    {
        private readonly IEnumerable<IEqualityComparer<KeyValuePair<string, string>>> evaluators;

        public A2JParser(IEnumerable<IEqualityComparer<KeyValuePair<string, string>>> logicalEvaluators)
        {
            evaluators = logicalEvaluators;
        }

        public Dictionary<string, string> Parse(string logic, Dictionary<string, string> inputVars)
        {
            var IFstatements = logic.SplitAndReturnFullSentencesOn("END IF");
            var varsInScopeForPersonalizedPlan = new Dictionary<string, string>();

            foreach (var IFstatement in IFstatements)
            {
                // READING THE VARS AND CONDITIONS
                var leftCondition = IFstatement.GetStringBetween("IF", "SET");

                var ANDvariables = leftCondition.GetANDvariables();
                var ORvariables = leftCondition.GetORvariables();

                // SETTING THE BOOLIAN RESULT IN THE SET VAR
                var rightOf = IFstatement.GetStringBetween("SET", "END IF");
                var SETvariables = rightOf.SetValueTOVar();

                // COMPUTE RESULT
                if (IsConditionSatisfied(ANDvariables, ORvariables, inputVars))
                {
                    foreach (var SETvar in SETvariables)
                    {
                        varsInScopeForPersonalizedPlan.Add(SETvar.Key, SETvar.Value);
                    }
                }
            }

            return varsInScopeForPersonalizedPlan;
        }

        public bool IsConditionSatisfied(Dictionary<string, string> ANDvariables, Dictionary<string, string> ORvariables,
            Dictionary<string, string> inputVars)
        {
            foreach (var evaluator in evaluators)
            {
                var test = inputVars.LogicEvaluator(ANDvariables, evaluator);
            }
            //var test = inputVars.LogicEvaluator(ANDvariables, evaluators.First());


            // Todo:@Alaa implement this logic
            // this is awfully simplistic at this point. only for a POC
            foreach (var inputVar in inputVars)
            {
                if (ANDvariables.Keys.Contains(inputVar.Key) && ANDvariables.Values.Contains(inputVar.Value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
