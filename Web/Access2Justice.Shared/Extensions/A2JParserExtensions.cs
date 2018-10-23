using Access2Justice.Shared.A2JAuthor;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace Access2Justice.Shared.Extensions
{
    public static class A2JParserExtensions
    {
        private static object text;

        public static List<string> IFstatements(this string logic)
        {
            return logic.SplitAndReturnFullSentencesOn(Tokens.ENDIF);
        }

        public static Dictionary<string, string> SETvars(this string logic)
        {
            var rightOf = logic.GetStringBetween(Tokens.SET, Tokens.ENDIF);
            return rightOf.SetValue();
        }

        public static OrderedDictionary ANDvars(this string logic)
        {
            var leftCondition = logic.GetStringBetween(Tokens.IF, Tokens.SET);
            return leftCondition.GetVariablesWithValues(Tokens.AND);
        }

        public static OrderedDictionary ORvars(this string logic)
        {
            var leftCondition = logic.GetStringBetween(Tokens.IF, Tokens.SET);
            return leftCondition.GetVariablesWithValues(Tokens.OR);
        }

        public static string GetStringOnTheRightOf(this string inputText, string splitWord)
        {
            if (inputText.IndexOf(splitWord) > 0)
            {
                return inputText.Substring(inputText.IndexOf(splitWord) + splitWord.Length);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetStringOnTheLeftOf(this string inputText, string splitWord)
        {
            if (inputText.IndexOf(splitWord) > 0)
            {
                return inputText.Substring(0, inputText.IndexOf(splitWord));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetStringBetween(this string inputText, string firstWord, string secondWord)
        {
            int startTextIndex = inputText.IndexOf(firstWord) + firstWord.Count();
            int sentenceLength = inputText.IndexOf(secondWord) - startTextIndex;

            return inputText.Substring(startTextIndex, sentenceLength);
        }

        public static List<string> SplitAndReturnFullSentencesOn(this string inputText, string splitWord)
        {
            var splittedSentences = inputText.Split(new string[] { splitWord }, StringSplitOptions.RemoveEmptyEntries);

            var sentences = new List<string>();
            foreach (var sentence in splittedSentences)
            {
                var fullSentence = string.Concat(sentence + splitWord + Tokens.EmptySpace);
                sentences.Add(fullSentence);
            }

            return sentences;
        }

        public static List<string> SplitOn(this string inputText, string splitWord)
        {
            return inputText.Split(new string[] { splitWord }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }


        public static OrderedDictionary GetVariablesWithValues(this string inputText, string operand = "")
        {
            var varsValues = new OrderedDictionary();
            if (inputText.ToUpperInvariant().Contains(operand))
            {
                var variables = inputText.SplitOn(operand);

                foreach (var varialbe in variables)
                {
                    var varValue = varialbe.GetStringBetween(Tokens.VarNameLeftSign, Tokens.VarNameRightSign);

                    if (varialbe.ToUpperInvariant().Contains(Tokens.TrueTokens.TrueText))
                    {
                        varsValues.Add(varValue, Tokens.TrueTokens.LogicalTrue);
                    }
                    else if (varialbe.ToUpperInvariant().Contains(Tokens.FalseTokens.FalseText))
                    {
                        varsValues.Add(varValue, Tokens.FalseTokens.LogicalFalse);
                    }
                    else
                    {
                         // Todo:@Alaa factor out this code (it is being used in muliple locatoins by now)
                        if (!varsValues.Contains(varValue))
                        {
                            varsValues.Add(varValue, inputText.RemoveQuotes());
                        }
                        else
                        {
                            varsValues[varValue] = inputText.RemoveQuotes();
                        }
                    }
                }
            }

            return varsValues;
        }

        public static Dictionary<string, string> SetValue(this string inputText)
        {
            var varsValues = new Dictionary<string, string>();
            var variableName = string.Empty;
            if (inputText.Contains(Tokens.TO))
            {
                variableName = inputText.GetStringBetween(Tokens.VarNameLeftSign, Tokens.VarNameRightSign);
                var valueString = inputText.GetStringOnTheRightOf(Tokens.TO);

                if (valueString.ToUpperInvariant().Contains(Tokens.TrueTokens.TrueText))
                {
                    varsValues.Add(variableName, Tokens.TrueTokens.LogicalTrueText);
                }
                else if (valueString.ToUpperInvariant().Contains(Tokens.FalseTokens.FalseText))
                {
                    varsValues.Add(variableName, Tokens.FalseTokens.LogicalFalseText);
                }
                else
                {
                    varsValues.Add(variableName, valueString);
                }
            }
            else
            {
                // todo: take this an extra step - double check the var you extracted matches some curated experience step name.
                varsValues.Add(Tokens.GOTO + "-" + Guid.NewGuid(), inputText.RemoveQuotes());
            }

            return varsValues;
        }

        public static string RemoveQuotes(this string inputText)
        {
            var matchQuotes = Regex.Matches(inputText, "\"([^\"]*)\"");
            var matchedString = string.Empty;
            if (matchQuotes.Any())
            {
                matchedString = matchQuotes.FirstOrDefault().Value.Replace("\"", "");
            }

            return matchedString;
        }
    }
}
