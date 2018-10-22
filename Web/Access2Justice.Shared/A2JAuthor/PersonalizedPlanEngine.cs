﻿using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Shared.A2JAuthor
{
    public class A2JAuthorPersonalizedPlanEngine : IPersonalizedPlanEngine
    {
        private readonly IA2JAuthorLogicParser parser;
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;

        public A2JAuthorPersonalizedPlanEngine(IA2JAuthorLogicParser parser, IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings)
        {
            this.parser = parser;
            this.dynamicQueries = dynamicQueries;
            this.cosmosDbSettings = cosmosDbSettings;
        }

        public async Task<UnprocessedPersonalizedPlan> Build(JObject personalizedPlan, CuratedExperienceAnswers userAnswers)
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
                                unprocessedStep.Description = state.GetValue("userContent");
                                unprocessedStep.ResourceIds = ExtractResourceIds(state.GetValue("userContent"));
                            }
                        }
                        unprocessedTopic.UnprocessedSteps.Add(unprocessedStep);
                    }
                }
            }

            unprocessedPlan.UnprocessedTopics.Add(unprocessedTopic);
            return unprocessedPlan;
        }

        private List<Guid> ExtractResourceIds(string html)
        {
            var matched = new List<Guid>();
            int indexStart = 0, indexEnd = 0;
            bool exit = false;
            while (!exit)
            {
                indexStart = html.IndexOf(Tokens.CustomHtmlTag);
                indexEnd = html.IndexOf(Tokens.CustomHtmlClosingTag);
                if (indexStart != -1 && indexEnd != -1)
                {
                    matched.Add(new Guid(html.Substring(indexStart + Tokens.CustomHtmlTag.Length,
                        indexEnd - indexStart - Tokens.CustomHtmlTag.Length)));
                    html = html.Substring(indexEnd + Tokens.CustomHtmlClosingTag.Length);
                }
                else
                    exit = true;
            }
            return matched;
        }
    }
}
