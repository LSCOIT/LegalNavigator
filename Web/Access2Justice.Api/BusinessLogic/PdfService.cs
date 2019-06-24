using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace Access2Justice.Api.BusinessLogic
{
    public class PdfService : IPdfService
    {
        private readonly IConverter pdfConverter;
        private readonly ITemplateService templateService;

        private const string _planTemplate = "Templates/PersonalActionPlan";

        public PdfService(ITemplateService templateService, IConverter pdfConverter)
        {
            this.templateService = templateService;
            this.pdfConverter = pdfConverter;
        }

        public async Task<byte[]> PrintPlan(PersonalizedPlanViewModel personalizedPlan)
        {
            foreach (var personalizedPlanTopic in personalizedPlan.Topics)
            {
                foreach (var planStep in personalizedPlanTopic.Steps)
                {
                    planStep.Description = Regex.Replace(planStep.Description,
                        @"(&nbsp;)?((http|https|ftp)\://[a-z\.\-0-9/#_\?\+&amp;%\$\=~]+){1,1}(\s(\[word\]|\[pdf\])){1,1}",
                        " <a href='$2'>$2</a>&nbsp;$5", RegexOptions.IgnoreCase);
                }
            }

            return await getFile(personalizedPlan, _planTemplate);
        }

        private async Task<byte[]> getFile<T>(T personalizedPlan, string templateFile)
        {
            var content = await templateService.RenderTemplateAsync(templateFile, personalizedPlan);
            var result = pdfConverter.Convert(new HtmlToPdfDocument
            {
                Objects =
                {
                    new ObjectSettings
                    {
                        UseLocalLinks = false,
                        HtmlContent = content
                    }
                }
            });
            return result;
        }
    }
}
