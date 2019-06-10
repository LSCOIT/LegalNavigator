using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.A2JAuthor
{
    public class LogicParser : IA2JAuthorLogicParser
    {
        private readonly IA2JAuthorLogicInterpreter interpreter;

        public LogicParser(IA2JAuthorLogicInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers, out Dictionary<string, int> order)
        {
            var userAnswersKeyValuePairs = ExtractAnswersVarValues(curatedExperienceAnswers);
            var logicStatements = ExtractAnswersLogicalStatements(curatedExperienceAnswers);

            var evaluatedAnswers = new Dictionary<string, string>();
            order = new Dictionary<string, int>();

            foreach (string logicalStatement in logicStatements)
            {
                foreach (var ifStatement in logicalStatement.SplitOnIFstatements())
                {
                    var leftVarValues = new Dictionary<string, string>();
                    var leftLogic = string.Empty;
                    var rightLogic = string.Empty;
                    if (ifStatement.Contains(Tokens.ParserConfig.SetVariables))
                    {
                        leftLogic = ifStatement.GetStringLeftSide(Tokens.SET);
                        rightLogic = ifStatement.GetStringRightSide(Tokens.SET);
                    }
                    else if (ifStatement.Contains(Tokens.ParserConfig.GoToQuestions))
                    {
                        leftLogic = ifStatement.GetStringLeftSide(Tokens.GOTO);
                        rightLogic = ifStatement.GetStringRightSide(Tokens.GOTO);
                    }

                    if (!string.IsNullOrWhiteSpace(leftLogic) && !string.IsNullOrWhiteSpace(rightLogic))
                    {
                        var newValues = new HashSet<string>();
                        var ANDvars = leftLogic.GetVariablesWithValues(Tokens.AND);
                        if (interpreter.Interpret(userAnswersKeyValuePairs, ANDvars, (x, y) => x && y))
                        {
                            newValues.UnionWith(evaluatedAnswers.AddDistinctRange(rightLogic.AddValue()).Keys);
                        }

                        var ORvars = leftLogic.GetVariablesWithValues(Tokens.OR);
                        if (interpreter.Interpret(userAnswersKeyValuePairs, ORvars, (x, y) => x || y))
                        {
                            newValues.UnionWith(evaluatedAnswers.AddDistinctRange(rightLogic.AddValue()).Keys);
                        }

                        if (ANDvars.Count == 0 && ORvars.Count == 0)
                        {
                            var oneVar = leftLogic.GetVariablesWithValues();
                            if (interpreter.Interpret(userAnswersKeyValuePairs, oneVar, (x, y) => x))
                            {
                                newValues.UnionWith(evaluatedAnswers.AddDistinctRange(rightLogic.AddValue()).Keys);
                            }
                        }

                        order.AddDistinctRange(newValues.ToDictionary(x => x, x => (int)findAnswerNumberByCode(
                            curatedExperienceAnswers,logicalStatement)));
                    }
                }
            }

            var restVariables = evaluatedAnswers.AddDistinctRange(userAnswersKeyValuePairs);
            order.AddDistinctRange(restVariables.Keys.ToDictionary(x => x, x => (int) findAnswerNumberByName(
                curatedExperienceAnswers, x)));
            return evaluatedAnswers;
        }

        private uint findAnswerNumberByCode(CuratedExperienceAnswers curatedExperienceAnswers, string code)
        {
            return curatedExperienceAnswers.ButtonComponents.Cast<AnswerComponent>()
                       .Union(curatedExperienceAnswers.FieldComponents)
                       .FirstOrDefault(x => x.CodeAfter == code || x.CodeBefore == code)
                       ?.AnswerNumber ?? 0;
        }

        private uint findAnswerNumberByName(CuratedExperienceAnswers curatedExperienceAnswers, string name)
        {
            var button = curatedExperienceAnswers.ButtonComponents.FirstOrDefault(x => x.Name == name);
            if (button != null)
            {
                return button.AnswerNumber;
            }

            var field = curatedExperienceAnswers.FieldComponents.FirstOrDefault(x => x.Fields.Any(y => y.Name == name));
            if(field != null)
            {
                return field.AnswerNumber;
            }
            return 0;
        }

        public Dictionary<string, string> Parse(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            return Parse(curatedExperienceAnswers, out _);
        }

        private Dictionary<string, string> ExtractAnswersVarValues(CuratedExperienceAnswers curatedExperienceAnswers)
        {
            var userAnswers = new Dictionary<string, string>();

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
            var statements = new List<string>();

            foreach (var answer in curatedExperienceAnswers.ButtonComponents.Cast<AnswerComponent>()
                .Union(curatedExperienceAnswers.FieldComponents)
                .OrderBy(x => x.AnswerNumber))
            {
                if (!string.IsNullOrWhiteSpace(answer.CodeBefore))
                {
                    statements.Add(answer.CodeBefore);
                }
                if (!string.IsNullOrWhiteSpace(answer.CodeAfter))
                {
                    statements.Add(answer.CodeAfter);
                }
            }

            return statements;
        }
    }
}
