﻿using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class Parser : IParse
    {
        private readonly IEvaluate evaluator;

        public Parser(IEvaluate evaluator)
        {
            this.evaluator = evaluator;
        }

        public Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            Dictionary<string, string> userAnswersKeyValuePairs = ExtractAnswersVarValues(curatedExperienceAnswers);
            List<string> logicStatements = ExtractAnswersLogicalStatements(curatedExperienceAnswers);

            Dictionary<string, string> evaluatedAnswers = new Dictionary<string, string>();

            // Todo:@Alaa one logic text could have more than one logical statements, code should accomodate for this.
            foreach (string logicalStatement in logicStatements)
            {
                foreach (var ifStatement in logicalStatement.IFstatements())
                {
                    Dictionary<string, string> leftVarValues = new Dictionary<string, string>();

                    string leftLogic = ifStatement.GetStringOnTheLeftOf("SET");
                    string rightLogic = ifStatement.GetStringOnTheRightOf("SET");

                    var ANDvars = leftLogic.GetVariablesWithValues("AND");
                    if (evaluator.Evaluate(userAnswersKeyValuePairs, ANDvars, (x, y) => x && y))
                    {                     
                        evaluatedAnswers.AddRange(rightLogic.SetValueTOVar());
                    }

                    var ORvars = leftLogic.GetVariablesWithValues("OR");
                    if(evaluator.Evaluate(userAnswersKeyValuePairs, ORvars, (x, y) => x || y))
                    {
                        evaluatedAnswers.AddRange(rightLogic.SetValueTOVar());
                    }

                    string breakpoint = string.Empty; // Todo:@Alaa - remove this temp code
                }
            }

            return evaluatedAnswers;
        }

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
            // in logical statements we have codeBefore and codeAfter, I consolidated them here but maybe
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
