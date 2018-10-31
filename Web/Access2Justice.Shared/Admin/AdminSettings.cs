using System;
using System.Globalization;
using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared.Admin
{
    public class AdminSettings : IAdminSettings
    {
        public AdminSettings(IConfiguration configuration)
        {
            try
            {
                BaseTemplateFileFullName = configuration.GetSection("CuratedExpImport:BaseTemplateFileFullName").Value;
                BaseTemplatePropertyForTemplateOrder = configuration.GetSection("CuratedExpImport:BaseTemplatePropertyForTemplateOrder").Value;
                TemplateFileName = configuration.GetSection("CuratedExpImport:TemplateFileName").Value;
                GuideTemplateFileName = configuration.GetSection("CuratedExpImport:GuideTemplateFileName").Value;
                TemplateFileType = configuration.GetSection("CuratedExpImport:TemplateFileType").Value;
                RootNode = configuration.GetSection("CuratedExpImport:RootNode").Value;
                ChildrenNode = configuration.GetSection("CuratedExpImport:ChildrenNode").Value;
                UploadFolderName = configuration.GetSection("CuratedExpImport:UploadFolderName").Value;
                WebRootFolderName = configuration.GetSection("CuratedExpImport:WebRootFolderName").Value;
                SuccessMessage = configuration.GetSection("CuratedExpImport:SuccessMessage").Value;
                FailureMessage = configuration.GetSection("CuratedExpImport:FailureMessage").Value;
                FileExtensionMessage = configuration.GetSection("CuratedExpImport:FileExtensionMessage").Value;
                ValidationMessage = configuration.GetSection("CuratedExpImport:ValidationMessage").Value;
                NameValidationMessage = configuration.GetSection("CuratedExpImport:NameValidationMessage").Value;
                MissingTopicMessage = configuration.GetSection("CuratedExpImport:MissingTopicMessage").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        public string BaseTemplateFileFullName { get; set; }

        public string BaseTemplatePropertyForTemplateOrder { get; set; }

        public string TemplateFileName { get; set; }

        public string GuideTemplateFileName { get; set; }

        public string TemplateFileType { get; set; }

        public string RootNode { get; set; }

        public string ChildrenNode { get; set; }

        public string UploadFolderName { get; set; }

        public string WebRootFolderName { get; set; }

        public string SuccessMessage { get; set; }

        public string FailureMessage { get; set; }

        public string FileExtensionMessage { get; set; }

        public string NameValidationMessage { get; set; }

        public string MissingTopicMessage { get; set; }

        public string ValidationMessage { get; set; }
    }
}