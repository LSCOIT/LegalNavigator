using System;

namespace Access2Justice.Shared
{
    public interface IAdminSettings
    {
        string BaseTemplateFileFullName { get; }
        string BaseTemplatePropertyForTemplateOrder { get; }
        string TemplateFileName { get; }
        string GuideTemplateFileName { get; }
        string TemplateFileType { get; }
        string RootNode { get; }
        string ChildrenNode { get; }
        string UploadFolderName { get; }
        string WebRootFolderName { get; }
        string SuccessMessage { get; }
        string FailureMessage { get; }
        string FileExtensionMessage { get; }
        string ValidationMessage { get; }
        string NameValidationMessage { get; }
        string MissingTopicMessage { get; }
        string ModelStateInvalidMessage { get; }
        string CouldNotRetrieveTemplateOrderMessage { get; }
        string NoTemplateFilesMessage { get; }
        string SameTitleInModelAndFileMessage { get; }
        string A2JTemplateDbSaveError { get; }
        string CuratedTemplateDbSaveError { get; }
        string MissingNodeMessage { get; }
    }
}
