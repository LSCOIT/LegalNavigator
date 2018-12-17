using Access2Justice.Shared.A2JAuthor;
using System;
using System.ComponentModel.DataAnnotations;
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

            var sanitizer = HtmlSanitizer.SimpleHtml5Sanitizer();

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
            // For some reason, the Vereyon.Web nuget is not sanitizing <p> <blockquote> tags. so
            // I'm renaming them to <div> and santizing the html again - rename then remove.
            sanitizer.Tag("p").Rename("div");
            sanitizer.Tag("blockquote").Rename("div");
            string sanitizedHtmlExceptPTag = sanitizer.Sanitize(Regex.Unescape(text));
            return HttpUtility.HtmlDecode(sanitizer.Sanitize(sanitizedHtmlExceptPTag));
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

        public static bool IsValidEmailAddress(this string address) => address != null && new EmailAddressAttribute().IsValid(address);
    }
}
