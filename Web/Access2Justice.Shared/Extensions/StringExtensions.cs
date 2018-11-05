using Access2Justice.Shared.A2JAuthor;
using System;
using System.Text.RegularExpressions;
using System.Web;
using Vereyon.Web;

namespace Access2Justice.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveHtmlTags(this string text, bool keepTextFormatingTags = false, bool keepHyperlinkTags = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var sanitizer = new HtmlSanitizer();

            if (keepTextFormatingTags)
            {
                // allow these tags and remove everything else
                sanitizer.Tag("strong");
                sanitizer.Tag("b").Rename("strong");
                sanitizer.Tag("i");
                sanitizer.Tag("u");
                sanitizer.Tag("br");
            }

            if (keepHyperlinkTags)
            {
                // allow urls
                sanitizer.Tag("a").SetAttribute("target", "_blank")
                    .SetAttribute("rel", "nofollow")
                    .CheckAttribute("href", HtmlSanitizerCheckType.Url)
                    .RemoveEmpty();
            }

            string cleanHtml = sanitizer.Sanitize(Regex.Unescape(text));
            return HttpUtility.HtmlDecode(cleanHtml);
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
                }
                else
                {
                    exit = true;
                }
            }
            curatedExperienceContent.SanitizedHtml = text;
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
