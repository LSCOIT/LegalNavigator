using Access2Justice.Shared.A2JAuthor;
using System;

namespace Access2Justice.Shared.Extensions
{
    public static class StringExtensions
    {

        public static string RemoveHtmlTags(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            return text.SanitizeString(Constants.HtmlLeftBracket, Constants.HtmlRightBracket);
        }

        public static string RemoveCustomA2JFunctions(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            return text.SanitizeString(Constants.A2JAuthorCustomFunctionTags, Constants.A2JAuthorCustomFunctionTags);
        }

        public static PersonalizedPlanCustomTags ExtractIdsRemoveCustomA2JTags(this string text)
        {
            var indexStart = 0;
            var indexEnd = 0;
            var curatedExperienceContent = new PersonalizedPlanCustomTags();
            var exit = false;
            while (!exit)
            {
                indexStart = text.IndexOf(Tokens.CustomHtmlTag);
                indexEnd = text.IndexOf(Tokens.CustomHtmlClosingTag);
                if (indexStart != -1 && indexEnd != -1)
                {
                    curatedExperienceContent.ResourceIds.Add(new Guid(
                        text.Substring(indexStart + Tokens.CustomHtmlTag.Length,
                        indexEnd - indexStart - Tokens.CustomHtmlTag.Length)));

                    var truncateText = text.Substring(indexStart, indexEnd - indexStart + Tokens.CustomHtmlClosingTag.Length);
                    text = text.Replace(truncateText, "");
                    curatedExperienceContent.SanitizedHtml = text;
                }
                else
                    exit = true;
            }
            return curatedExperienceContent;
        }

        public static string SanitizeString(this string text, string startString, string endString)
        {
            var indexStart = 0;
            var indexEnd = 0;
            var exit = false;
            while (!exit)
            {
                indexStart = text.IndexOf(startString);
                indexEnd = text.IndexOf(endString, indexStart + 1);
                if (indexStart != -1 && indexEnd != -1)
                {
                    var truncateText = text.Substring(indexStart, indexEnd - indexStart + endString.Length);
                    text = text.Replace(truncateText, "");
                }
                else
                    exit = true;
            }
            return text;
        }
    }
}
