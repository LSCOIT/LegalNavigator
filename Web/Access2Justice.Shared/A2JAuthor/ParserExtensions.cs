using Access2Justice.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace Access2Justice.Shared.A2JAuthor
{
    public static class ParserExtensions
    {
        public static Dictionary<string, string> GetSETvars(this string logic)
        {
            var rightOf = logic.GetStringBetween(Tokens.SET, Tokens.ENDIF);
            return rightOf.AddValue();
        }

        public static OrderedDictionary GetANDvars(this string logic)
        {
            var leftCondition = logic.GetStringBetween(Tokens.IF, Tokens.SET);
            return leftCondition.GetVariablesWithValues(Tokens.AND);
        }

        public static OrderedDictionary GetORvars(this string logic)
        {
            var leftCondition = logic.GetStringBetween(Tokens.IF, Tokens.SET);
            return leftCondition.GetVariablesWithValues(Tokens.OR);
        }

        public static string GetStringRightSide(this string inputText, string splitWord)
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

        public static string GetStringLeftSide(this string inputText, string splitWord)
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

        public static List<string> SplitOnIFstatements(this string logic)
        {
            return logic.SplitAndReturnFullSentencesOn(Tokens.ENDIF);
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

                    if (varialbe.ToUpperInvariant().Contains(Tokens.TrueTokens.True))
                    {
                        varsValues.Add(varValue, Tokens.TrueTokens.True.ToLower());
                    }
                    else if (varialbe.ToUpperInvariant().Contains(Tokens.FalseTokens.False))
                    {
                        varsValues.Add(varValue, Tokens.FalseTokens.False.ToLower());
                    }
                    else
                    {
                        varsValues.AddDistinctKeyValue(varValue, inputText.RemoveQuotes());
                    }
                }
            }

            return varsValues;
        }

        public static Dictionary<string, string> AddValue(this string inputText)
        {
            var varsValues = new Dictionary<string, string>();
            // if has multiple SET tokens, then input text might be like [set var expression 1] SET [set var expression 2] SET ...
			if (inputText.Contains(Tokens.SET))
			{
				foreach (var keyValue in inputText.GetStringLeftSide(Tokens.SET).AddValue())
				{
					varsValues[keyValue.Key] = keyValue.Value;
				}
				foreach (var keyValue in inputText.GetStringRightSide(Tokens.SET).AddValue())
				{
					varsValues[keyValue.Key] = keyValue.Value;
				}

				return varsValues;
			}

			if (inputText.Contains(Tokens.TO))
			{
				var variableName = inputText.GetStringBetween(Tokens.VarNameLeftSign, Tokens.VarNameRightSign);
				var valueString = inputText.GetStringRightSide(Tokens.TO);

				if (valueString.ToUpperInvariant().Contains(Tokens.TrueTokens.True))
				{
					varsValues.Add(variableName, Tokens.TrueTokens.True.ToLower());
				}
				else if (valueString.ToUpperInvariant().Contains(Tokens.FalseTokens.False))
				{
					varsValues.Add(variableName, Tokens.FalseTokens.False.ToLower());
				}
				else
				{
					varsValues.Add(variableName, valueString);
				}
			}
			else
			{
				// you could take this an extra step by double checking the var you extracted 
				// matches some curated experience step name.
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

        public static string RemoveHtmlTags(this string inputText)
        {
            return inputText.Replace("<BR/>", new string(' ', 1));
        }
    }
}
