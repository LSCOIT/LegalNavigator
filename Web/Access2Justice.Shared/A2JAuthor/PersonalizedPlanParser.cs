using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class PersonalizedPlanParser : IPersonalizedPlanParse
    {
        private readonly IPersonalizedPlanEvaluate evaluator;

        public PersonalizedPlanParser(IPersonalizedPlanEvaluate evaluator)
        {
            this.evaluator = evaluator;
        }

        public Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers, string parserConfig)
        {
            Dictionary<string, string> userAnswersKeyValuePairs = ExtractAnswersVarValues(curatedExperienceAnswers);
            List<string> logicStatements = ExtractAnswersLogicalStatements(curatedExperienceAnswers);

            Dictionary<string, string> evaluatedAnswers = new Dictionary<string, string>();
            foreach (string logicalStatement in logicStatements)
            {
                foreach (var ifStatement in logicalStatement.IFstatements())
                {
                    Dictionary<string, string> leftVarValues = new Dictionary<string, string>();
                    var leftLogic = string.Empty;
                    var rightLogic = string.Empty;
                    if (parserConfig == Tokens.ParserConfig.SetVariables)
                    {
                        leftLogic = ifStatement.GetStringOnTheLeftOf(Tokens.SET);
                        rightLogic = ifStatement.GetStringOnTheRightOf(Tokens.SET);
                    }
                    else
                    {
                        leftLogic = ifStatement.GetStringOnTheLeftOf(Tokens.GOTO);
                        rightLogic = ifStatement.GetStringOnTheRightOf(Tokens.GOTO);
                    }
                    var ANDvars = leftLogic.GetVariablesWithValues(Tokens.AND);
                    if (evaluator.Evaluate(userAnswersKeyValuePairs, ANDvars, (x, y) => x && y))
                    {                     
                        evaluatedAnswers.AddRange(rightLogic.SetValueTOVar());
                    }

                    var ORvars = leftLogic.GetVariablesWithValues(Tokens.OR);
                    if(evaluator.Evaluate(userAnswersKeyValuePairs, ORvars, (x, y) => x || y))
                    {
                        evaluatedAnswers.AddRange(rightLogic.SetValueTOVar());
                    }

                    if(ANDvars.Count == 0 && ORvars.Count == 0)
                    {
                        evaluatedAnswers.AddRange(rightLogic.SetValueTOVar());
                    }
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
