﻿using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Parser : IParse, IEvaluate, ICompile
    {
        private readonly IEvaluate evaluator;
        private readonly ICompile compilePersonalizedPlan;

        public Parser(IEvaluate evaluator, ICompile compilePersonalizedPlan)
        {
            this.evaluator = evaluator;
            this.compilePersonalizedPlan = compilePersonalizedPlan;
        }

        public Dictionary<string, string> Evaluate(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            Dictionary<string, string> userAnswersKeyValuePairs = ExtractAnswersVarValues(curatedExperienceAnswers);
            List<string> logicStatements = ExtractAnswersLogicalStatements(curatedExperienceAnswers);

            Dictionary<string, string> actionPlanKeyValuePairs = new Dictionary<string, string>();

            foreach (string logicStatement in logicStatements)
            {
                Dictionary<string, string> leftVarValues = new Dictionary<string, string>();


                string leftLogic = logicStatement.GetStringOnTheLeftOf("SET");
                // Todo:@Alaa AND and OR must be treated differently
                leftVarValues.AddRange(leftLogic.GetVariablesWithValues("AND")).AddRange(leftLogic.GetVariablesWithValues("OR"));


                //Dictionary<string, string> rightVarValues = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> leftVarValue in leftVarValues)
                {
                    KeyValuePair<string, string> plan = userAnswersKeyValuePairs.Where(x => x.Key == leftVarValue.Key && x.Value == leftVarValue.Value).FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(plan.Key))
                    {
                        // extract right to SET and set values
                        string rightLogic = logicStatement.GetStringOnTheRightOf("SET");

                        actionPlanKeyValuePairs.AddRange(rightLogic.SetValueTOVar());
                    }

                    string breakpoint = string.Empty; // Todo:@Alaa - remove this temp code
                }
            }

            return actionPlanKeyValuePairs;
        }

        public A2JPersonalizedPlan Compile(Dictionary<string, string> setVars)
        {
            // 1. PlanDictionary now has all the vars needed to loop through the template, loop through the children and extract those that match, return children that are in scope.
            Dictionary<string, string> temp = setVars;

            return null;
        }

        // Todo:@Alaa should we put these somewhere else?
        private Dictionary<string, string> ExtractAnswersVarValues(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            Dictionary<string, string> userAnswers = new Dictionary<string, string>();

            foreach (ButtonComponent button in curatedExperienceAnswers.ButtonComponents)
            {
                userAnswers.Add(button.Name, button.Value);
            }

            foreach (FieldComponent fieldComponent in curatedExperienceAnswers.FieldComponents)
            {
                foreach (AnswerField field in fieldComponent.Fields)
                {
                    userAnswers.Add(field.Name, field.Value);
                }
            }

            return userAnswers;
        }

        private List<string> ExtractAnswersLogicalStatements(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            // Todo:@Alaa in logical statements we have codeBefore and codeAfter, I consolidated them here but maybe
            // we should create a class with two properties of List<string> for each of these. We will check the need for this as
            // the implementation matures.
            List<string> statements = new List<string>();

            foreach (ButtonComponent button in curatedExperienceAnswers.ButtonComponents)
            {
                if (!string.IsNullOrWhiteSpace(button.CodeBefore))
                {
                    statements.Add(button.CodeBefore);
                }
                if (!string.IsNullOrWhiteSpace(button.CodeAfter))
                {
                    statements.Add(button.CodeAfter);
                }
            }

            foreach (FieldComponent fieldComponent in curatedExperienceAnswers.FieldComponents)
            {
                if (!string.IsNullOrWhiteSpace(fieldComponent.CodeBefore))
                {
                    statements.Add(fieldComponent.CodeBefore);
                }
                if (!string.IsNullOrWhiteSpace(fieldComponent.CodeAfter))
                {
                    statements.Add(fieldComponent.CodeAfter);
                }
            }

            return statements;
        }
    }
}
