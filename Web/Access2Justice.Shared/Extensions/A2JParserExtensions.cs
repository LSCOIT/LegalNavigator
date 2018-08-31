using Access2Justice.Shared.A2JAuthor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Access2Justice.Shared.Extensions
{
    public static class A2JParserExtensions
    {
        public static List<string> IFstatements(this string logic)
        {
            return logic.SplitAndReturnFullSentencesOn("END IF");
        }

        public static Dictionary<string, string> SETvars(this string logic)
        {
            var rightOf = logic.GetStringBetween("SET", "END IF");
            return rightOf.SetValueTOVar();
        }

        public static Dictionary<string, string> ANDvars(this string logic)
        {
            var leftCondition = logic.GetStringBetween("IF", "SET");
            return leftCondition.GetVariablesWithValues("AND");
        }

        public static Dictionary<string, string> ORvars(this string logic)
        {
            var leftCondition = logic.GetStringBetween("IF", "SET");
            return leftCondition.GetVariablesWithValues("OR");
        }

        public static string GetStringOnTheRightOf(this string inputText, string splitWord)
        {
            return inputText.Substring(inputText.IndexOf(splitWord) + splitWord.Length);
        }

        public static string GetStringOnTheLeftOf(this string inputText, string splitWord)
        {
            return inputText.Substring(0, inputText.IndexOf(splitWord));
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
                var fullSentence = string.Concat(sentence + splitWord + " ");
                sentences.Add(fullSentence);
            }

            return sentences;
        }

        public static List<string> SplitOn(this string inputText, string splitWord)
        {
            return inputText.Split(new string[] { splitWord }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static Dictionary<string, string> GetVariablesWithValues(this string inputText, string operand)
        {
            // Todo:@Alaa extend this to allow extraction of other data types (beside true/fals), return a dic of <string, string>
            var varsValues = new Dictionary<string, string>();

            if (inputText.ToUpperInvariant().Contains(operand))
            {
                var variables = inputText.SplitOn(operand);
                foreach (var varialbe in variables)
                {
                    if (varialbe.ToUpperInvariant().Contains("TRUE"))
                    {
                        varsValues.Add(varialbe.GetStringBetween("[", "]"), "true");
                    }
                    else if (varialbe.ToUpperInvariant().Contains("FALSE"))
                    {
                        varsValues.Add(varialbe.GetStringBetween("[", "]"), "false");
                    }
                    // other 'else if' could be added here if we are dealing with more than true/false values
                }
            }

            return varsValues;
        }

        public static Dictionary<string, string> SetValueTOVar(this string inputText)
        {
            var varsValues = new Dictionary<string, string>();
            if (inputText.ToUpperInvariant().Contains("TO"))
            {
                var variableName = inputText.GetStringBetween("[", "]");
                var valueString = inputText.GetStringOnTheRightOf("TO");

                if (valueString.ToUpperInvariant().Contains("TRUE"))
                {
                    varsValues.Add(variableName, "TRUE");
                }
                else if (valueString.ToUpperInvariant().Contains("FALSE"))
                {
                    varsValues.Add(variableName, "FALSE");
                }
                else
                {
                    varsValues.Add(variableName, valueString);
                }
            }

            return varsValues;
        }
    }
}
