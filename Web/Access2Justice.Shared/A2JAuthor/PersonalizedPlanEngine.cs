using System;
using System.Collections.Generic;
using System.Linq;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class PersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IA2JAuthorLogicParser parser;

        public PersonalizedPlanEngine(IA2JAuthorLogicParser parser)
        {
            this.parser = parser;
        }

        public UnprocessedPersonalizedPlan Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers)
        {
            var stepsInScope = new List<JToken>();
            var evaluatedUserAnswers = parser.Parse(userAnswers);
            var a2jAnswers = MapStringsToA2JAuthorValues(evaluatedUserAnswers);

            var root = personalizedPlan
                .Properties()
                .GetArrayValue("rootNode")
                .FirstOrDefault();

            foreach (var child in root.GetValueAsArray<JArray>("children"))
            {
                var states = child.GetArrayValue("state");
                foreach (var state in states)
                {
                    foreach (var answer in a2jAnswers)
                    {
                        if (answer.Key == state.GetValue("leftOperand") && answer.Value == state.GetValue("operator"))
                        {
                            stepsInScope.Add(child);
                        }
                    }
                }
            }

            var unprocessedPlan = new UnprocessedPersonalizedPlan();
            unprocessedPlan.Id = Guid.NewGuid();

            var unprocessedTopic = new UnprocessedTopic();
            unprocessedTopic.Name = personalizedPlan.Properties().GetValue("title");

            foreach (var step in stepsInScope)
            {
                foreach (var childrenRoot in step.GetValueAsArray<JArray>("children"))
                {
                    var unprocessedStep = new UnprocessedStep();
                    foreach (var child in childrenRoot.GetValueAsArray<JObject>("rootNode").GetValueAsArray<JArray>("children"))
                    {

                        var state = child.GetArrayValue("state").FirstOrDefault();
                        unprocessedStep.Id = Guid.NewGuid();

                        if (!string.IsNullOrWhiteSpace(state.GetValue("title")))
                        {
                            unprocessedStep.Title = state.GetValue("title");
                            continue;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(state.GetValue("userContent")))
                            {
                                unprocessedStep.Description = state.GetValue("userContent").ExtractIdsRemoveCustomA2JTags().SanitizedHtml;
                                unprocessedStep.ResourceIds = state.GetValue("userContent").ExtractIdsRemoveCustomA2JTags().ResourceIds;
                            }
                        }
                        unprocessedTopic.UnprocessedSteps.Add(unprocessedStep);
                    }
                }
            }

            unprocessedPlan.UnprocessedTopics.Add(unprocessedTopic);
            return unprocessedPlan;
        }

        // boolian values in A2J Author have different names - true is called "is-true", "false is "is-false" so I had to 
        // convert boolian text to A2J Author boolian values.
        private Dictionary<string, string> MapStringsToA2JAuthorValues(Dictionary<string, string> evaluatedUserAnswers)
        {
            var a2jAnswers = new Dictionary<string, string>();
            foreach (var answer in evaluatedUserAnswers)
            {
                if(answer.Value.ToUpper() == Tokens.TrueTokens.True)
                {
                    a2jAnswers.Add(answer.Key, Tokens.TrueTokens.A2JLogicalTrue);
                }
                else if(answer.Value.ToUpper() == Tokens.FalseTokens.False)
                {
                    a2jAnswers.Add(answer.Key, Tokens.FalseTokens.A2JLogicalFalse);
                }
                else
                {
                    a2jAnswers.Add(answer.Key, answer.Value);
                }
            }

            return a2jAnswers;
        }
    }
}
