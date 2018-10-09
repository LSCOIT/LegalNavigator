﻿using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class A2JAuthorPersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IPersonalizedPlanParse parser;

        public A2JAuthorPersonalizedPlanEngine(IPersonalizedPlanParse parser)
        {
            this.parser = parser;
        }

        // delete the compiler, map a2j author to the the UnprocessedPersonalizedPlan here
        public UnprocessedPersonalizedPlan Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            var stepsInScope = new List<JToken>();
            var evaluatedUserAnswers = parser.Parse(userAnswers);

            var root = personalizedPlan
                .Properties()
                .GetArrayValue("rootNode")
                .FirstOrDefault();

            foreach (var child in root.GetValueAsArray<JArray>("children"))
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

            var breakpoint = string.Empty; // Todo:@Alaa - remove this temp code
            return null;
        }
    }
}
