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
    }
}
