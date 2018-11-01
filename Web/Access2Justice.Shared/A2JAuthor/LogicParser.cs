using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;

namespace Access2Justice.Shared.A2JAuthor
{
    public class LogicParser : IA2JAuthorLogicParser
    {
        private readonly IA2JAuthorLogicInterpreter interpreter;

        public LogicParser(IA2JAuthorLogicInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            var userAnswersKeyValuePairs = ExtractAnswersVarValues(curatedExperienceAnswers);
            var logicStatements = ExtractAnswersLogicalStatements(curatedExperienceAnswers);

            var evaluatedAnswers = new Dictionary<string, string>();
            foreach (string logicalStatement in logicStatements)
            {
                foreach (var ifStatement in logicalStatement.IFstatements())
                {
                    Dictionary<string, string> leftVarValues = new Dictionary<string, string>();
                    var leftLogic = string.Empty;
                    var rightLogic = string.Empty;
                    if (ifStatement.Contains(Tokens.ParserConfig.SetVariables))
                    {
                        leftLogic = ifStatement.GetStringOnTheLeftOf(Tokens.SET);
                        rightLogic = ifStatement.GetStringOnTheRightOf(Tokens.SET);
                    }
                    else if(ifStatement.Contains(Tokens.ParserConfig.GoToQuestions))
                    {
                        leftLogic = ifStatement.GetStringOnTheLeftOf(Tokens.GOTO);
                        rightLogic = ifStatement.GetStringOnTheRightOf(Tokens.GOTO);
                    }

                    if (!string.IsNullOrWhiteSpace(leftLogic) && !string.IsNullOrWhiteSpace(rightLogic))
                    {
                        var ANDvars = leftLogic.GetVariablesWithValues(Tokens.AND);
                        if (interpreter.Interpret(userAnswersKeyValuePairs, ANDvars, (x, y) => x && y))
                        {
                            evaluatedAnswers.AddRange(rightLogic.SetValue());
                        }

                        var ORvars = leftLogic.GetVariablesWithValues(Tokens.OR);
                        if (interpreter.Interpret(userAnswersKeyValuePairs, ORvars, (x, y) => x || y))
                        {
                            evaluatedAnswers.AddRange(rightLogic.SetValue());
                        }

                        if (ANDvars.Count == 0 && ORvars.Count == 0)
                        {
                            var oneVar = leftLogic.GetVariablesWithValues();
                            if (interpreter.Interpret(userAnswersKeyValuePairs, oneVar, (x, y) => x))
                            {
                                evaluatedAnswers.AddRange(rightLogic.SetValue());
                            }
                        }
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
                if (!string.IsNullOrWhiteSpace(button.Name) && !string.IsNullOrWhiteSpace(button.Value))
                {
                    if (!userAnswers.ContainsKey(button.Name))
                    {
                        userAnswers.Add(button.Name, button.Value);
                    }
                    else
                    {
                        userAnswers[button.Name] = button.Value;
                    }
                }
            }

            foreach (FieldComponent fieldComponent in curatedExperienceAnswers.FieldComponents)
            {
                foreach (AnswerField field in fieldComponent.Fields)
                {
                    if (!string.IsNullOrWhiteSpace(field.Name) && !string.IsNullOrWhiteSpace(field.Value))
                    {
                        if (!userAnswers.ContainsKey(field.Name))
                        {
                            userAnswers.Add(field.Name, field.Value);
                        }
                        else
                        {
                            userAnswers[field.Name] = field.Value;
                        }
                    }
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
