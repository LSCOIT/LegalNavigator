using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Access2Justice.Shared.Admin;

namespace Access2Justice.Api.BusinessLogic
{
    public class AdminBusinessLogic : IAdminBusinessLogic
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ICuratedExperienceConvertor a2jAuthorBusinessLogic;
        private readonly IAdminSettings adminSettings;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;

        public AdminBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService, IHostingEnvironment hostingEnvironment,
            ICuratedExperienceConvertor a2jAuthorBusinessLogic, IAdminSettings adminSettings,
            ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic)
        {
            this.dynamicQueries = dynamicQueries;
            this.cosmosDbSettings = cosmosDbSettings;
            this.backendDatabaseService = backendDatabaseService;
            this.hostingEnvironment = hostingEnvironment;
            this.a2jAuthorBusinessLogic = a2jAuthorBusinessLogic;
            this.adminSettings = adminSettings;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
        }

        public async Task<JsonUploadResult> UploadCuratedContentPackage(CuratedTemplate curatedTemplate)
        {
            var file = curatedTemplate.TemplateFile.First();
            if (file.Length <= 0 || Path.GetExtension(file.FileName) != Constants.A2JTemplateFileExtension)
            {
                return new JsonUploadResult
                {
                    Message = adminSettings.FileExtensionMessage,
                    ErrorCode = JsonUploadError.FileExtensionIsInvalid
                };
            }
            ExtractZipContent(file, out var templateOrder, out var templateModel);
            if (templateOrder == null || templateOrder.Count == 0)
            {
                return new JsonUploadResult
                {
                    Message = string.Format(CultureInfo.InvariantCulture, adminSettings.CouldNotRetrieveTemplateOrderMessage, adminSettings.BaseTemplateFileFullName),
                    ErrorCode = JsonUploadError.CouldNotRetrieveTemplateOrder
                };
            }
            if (templateModel == null || templateModel.Count == 0)
            {
                return new JsonUploadResult
                {
                    Message = adminSettings.NoTemplateFilesMessage,
                    ErrorCode = JsonUploadError.FilesNotFound
                };
            }
            var newTemplateId = Guid.NewGuid();

            var getTemplatesResult = GetTemplates(templateOrder, templateModel, out var mainTemplate, out var guideTemplate, out var resourceTitle);
            if (getTemplatesResult?.ErrorCode != null)
            {
                return getTemplatesResult;
            }

            if(resourceTitle == "Untitled Template")
            {
                resourceTitle = curatedTemplate.Name;
            }

            if (!string.Equals(resourceTitle.Trim(), curatedTemplate.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                return new JsonUploadResult
                {
                    Message = adminSettings.NameValidationMessage,
                    ErrorCode = JsonUploadError.NameIsIncorrect,
                    Details =
                        string.Format(CultureInfo.InvariantCulture, adminSettings.SameTitleInModelAndFileMessage,
                            fullFileName(templateOrder.FirstOrDefault()?.ToString()))
                };
            }

            var response = await ImportA2JTemplate(mainTemplate, newTemplateId);
            if (response?.Id == null)
            {
                return new JsonUploadResult
                {
                    Message = adminSettings.A2JTemplateDbSaveError,
                    ErrorCode = JsonUploadError.MainTemplateDbSaveError
                };
            }
            var curatedDocumentResponse = ImportCuratedTemplate(guideTemplate, newTemplateId);
            if (curatedDocumentResponse == null || curatedDocumentResponse.CuratedExperienceId == Guid.Empty)
            {
                return new JsonUploadResult
                {
                    Message = adminSettings.CuratedTemplateDbSaveError,
                    ErrorCode = JsonUploadError.CuratedTemplateDbSaveError
                };
            }
            return await CreateGuidedAssistantResource(resourceTitle, curatedTemplate, curatedDocumentResponse.CuratedExperienceId.ToString());

        }

        private JsonUploadResult GetTemplates(List<JToken> templateOrder, List<CuratedExperiencePackageTemplateModel> templateModel,
            out JObject mainTemplate, out JObject guideTemplate, out string resourceTitle)
        {
            var parentTemplateId = templateOrder.FirstOrDefault().ToString();
            mainTemplate = null;
            guideTemplate = null;
            JArray mainTemplateChildren = null;
            resourceTitle = string.Empty;

            foreach (var model in templateModel)
            {
                if (string.Equals(model.TemplateName, adminSettings.GuideTemplateFileName,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    guideTemplate = model.Template;
                    continue;
                }

                if (!model.Template.ContainsKey(adminSettings.RootNode))
                {
                    return new JsonUploadResult
                    {
                        Message = string.Format(CultureInfo.InvariantCulture, adminSettings.MissingNodeMessage, fullFileName(model.TemplateName)),
                        ErrorCode = JsonUploadError.RootNodeIsMissing
                    };
                }
                var rootNode = (JObject)model.Template[adminSettings.RootNode];
                if (!rootNode.ContainsKey(adminSettings.ChildrenNode))
                {
                    return new JsonUploadResult
                    {
                        Message = string.Format(CultureInfo.InvariantCulture, adminSettings.MissingNodeMessage,
                            fullFileName(model.TemplateName), "Children"),
                        ErrorCode = JsonUploadError.ChildrenNodeIsMissing
                    };
                }
                if (model.TemplateName == parentTemplateId)
                {
                    mainTemplate = model.Template;
                    if (!mainTemplate.ContainsKey("title"))
                    {
                        return new JsonUploadResult
                        {
                            Message = string.Format(CultureInfo.InvariantCulture, adminSettings.MissingNodeMessage,
                                fullFileName(model.TemplateName), "Title"),
                            ErrorCode = JsonUploadError.ChildrenNodeIsMissing
                        };
                    }
                    resourceTitle = mainTemplate.GetValue("title").ToString();
                    // var rootNode = (JObject)mainTemplate[adminSettings.RootNode];
                    mainTemplateChildren = (JArray)rootNode[adminSettings.ChildrenNode];
                }
                else if (model.TemplateName != parentTemplateId 
                         && !string.Equals(model.TemplateName, adminSettings.GuideTemplateFileName, 
                             StringComparison.InvariantCultureIgnoreCase))
                {
                    if (mainTemplateChildren == null)
                    {
                        return new JsonUploadResult
                        {
                            Message = string.Format(CultureInfo.InvariantCulture, adminSettings.MissingNodeMessage,
                                fullFileName(parentTemplateId), "Children"),
                            ErrorCode = JsonUploadError.ChildrenNodeIsMissing
                        };
                    }
                    var childrenNode = (JArray)rootNode[adminSettings.ChildrenNode];
                    foreach (var child in childrenNode)
                    {
                        mainTemplateChildren.Add(child);
                    }
                }
            }

            return null;
        }

        private string fullFileName(string x) => $"{adminSettings.TemplateFileName}{x}.json";

        public async Task<Document> ImportA2JTemplate(JObject mainTemplate, Guid newTemplateId)
        {
            mainTemplate.AddFirst(new JProperty(Constants.Id, newTemplateId));

            return await backendDatabaseService.CreateItemAsync(mainTemplate,
                cosmosDbSettings.A2JAuthorDocsCollectionId);
        }

        public CuratedExperience ImportCuratedTemplate(JObject guideTemplate, Guid newTemplateId)
        {
            return a2jAuthorBusinessLogic.ConvertA2JAuthorToCuratedExperience(guideTemplate, true, newTemplateId);
        }

        public async Task<JsonUploadResult> CreateGuidedAssistantResource(string resourceTitle, CuratedTemplate curatedTemplate, string curatedExperienceId)
        {
            var resourceDetails = await topicsResourcesBusinessLogic.GetResourceDetailAsync(resourceTitle, Constants.GuidedAssistant);
            List<GuidedAssistant> resources = JsonUtilities.DeserializeDynamicObject<List<GuidedAssistant>>(resourceDetails);
            var maxVersion = default(long);
            foreach (var resource in resources)
            {
                var resourceDetail = JsonUtilities.DeserializeDynamicObject<GuidedAssistant>(resource);
                if (resourceDetail.Version == default(long))
                {
                    resourceDetail.Version = IncrementVersion(default(long));
                }

                if (maxVersion.CompareTo(resourceDetail.Version) < 0)
                {
                    maxVersion = resourceDetail.Version;
                }
                resourceDetail.Display = "No";

                await backendDatabaseService.UpdateItemAsync(resourceDetail.ResourceId.ToString(),
                    JObject.FromObject(resourceDetail), cosmosDbSettings.ResourcesCollectionId);
            }

            var topicDetails = await topicsResourcesBusinessLogic.GetTopicDetailsAsync(resourceTitle.Trim());
            List<Topic> topics = JsonUtilities.DeserializeDynamicObject<List<Topic>>(topicDetails);

            if (topics.Count == 0)
            {
                return new JsonUploadResult
                {
                    Message = string.Format(CultureInfo.InvariantCulture, adminSettings.MissingTopicMessage,
                        resourceTitle),
                    ErrorCode = JsonUploadError.MissingTopic
                };
            }

            var guidedAssistantResource = new GuidedAssistant();
            var topicTags = new List<TopicTag>();
            var locations = new List<Location>();

            foreach (var topic in topics)
            {
                topicTags.Add(new TopicTag { TopicTags = topic.Id });
                locations.AddRange(topic.Location);
            }

            guidedAssistantResource.ResourceId = Guid.NewGuid();
            guidedAssistantResource.Name = curatedTemplate.Name;
            guidedAssistantResource.Description = curatedTemplate.Description;
            guidedAssistantResource.TopicTags = topicTags;
            guidedAssistantResource.Location = locations;
            guidedAssistantResource.Version = IncrementVersion(maxVersion);
            guidedAssistantResource.Display = "Yes";
            guidedAssistantResource.ResourceType = Constants.GuidedAssistant;
            guidedAssistantResource.CuratedExperienceId = curatedExperienceId;

            await backendDatabaseService.CreateItemAsync(guidedAssistantResource, cosmosDbSettings.ResourcesCollectionId);
            return new JsonUploadResult
            {
                Message = adminSettings.SuccessMessage + " New Curated Experience ID " + curatedExperienceId
            };

        }

        private void GetFilePath(IFormFile file, out string fullPath, out string uploadPath)
        {
            string webRootPath = hostingEnvironment.WebRootPath;
            uploadPath = string.Empty;
            
            var guidForTmpPath = Guid.NewGuid().ToString("N");
            
            if (webRootPath != null)
            {
                uploadPath = Path.Combine(webRootPath, adminSettings.UploadFolderName, guidForTmpPath);
            }
            else
            {
                uploadPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), adminSettings.WebRootFolderName),
                    adminSettings.UploadFolderName, guidForTmpPath);
            }
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            fullPath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (!uploadPath.EndsWith(Path.DirectorySeparatorChar))
                uploadPath += Path.DirectorySeparatorChar;
        }

        private void ExtractZipContent(IFormFile file, out List<JToken> templateOrder,
            out List<CuratedExperiencePackageTemplateModel> templateModel)
        {
            GetFilePath(file, out var fullPath, out var uploadPath);

            templateModel = new List<CuratedExperiencePackageTemplateModel>();
            templateOrder = new List<JToken>();

            using (var archive = ZipFile.OpenRead(fullPath))
            {
                foreach (var entry in archive.Entries)
                {
                    string destinationPath;
                    if (entry.FullName.EndsWith(adminSettings.BaseTemplateFileFullName))
                    {
                        destinationPath = Path.GetFullPath(Path.Combine(uploadPath, entry.FullName));

                        if (destinationPath.StartsWith(uploadPath, StringComparison.Ordinal))
                        {
                            entry.ExtractToFile(destinationPath);
                        }

                        using (var reader = new StreamReader(destinationPath))
                        {
                            var json = reader.ReadToEnd();
                            var parser = JObject.Parse(json);
                            var templates = parser.Properties()
                                .GetArrayValue(adminSettings.BaseTemplatePropertyForTemplateOrder)
                                .FirstOrDefault();
                            if (templates == null)
                            {
                                return;
                            }
                            templateOrder = templates.ToList();
                        }

                        //Delete uploaded json file
                        DeleteFile(destinationPath);
                    }

                    var match = Regex.Match(entry.FullName,
                        $@"({adminSettings.TemplateFileName}([0-9\-]+)\.json$)|({adminSettings.GuideTemplateFileName}\.json)",
                        RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        continue;
                    }

                    destinationPath = Path.GetFullPath(Path.Combine(uploadPath, Path.GetFileName(entry.FullName)));

                    if (destinationPath.StartsWith(uploadPath, StringComparison.Ordinal))
                    {
                        entry.ExtractToFile(destinationPath);
                    }

                    using (var reader = new StreamReader(destinationPath))
                    {
                        var json = reader.ReadToEnd();
                        var jObject = JObject.Parse(json);

                        if (destinationPath.EndsWith("Guide.json"))
                        {
                            FilterButtonLabels(jObject);
                        }

                        templateModel.Add(new CuratedExperiencePackageTemplateModel
                        {
                            TemplateName = Path.GetFileNameWithoutExtension(entry.FullName)
                                .Replace(adminSettings.TemplateFileName, string.Empty),
                            Template = jObject
                        });
                    }

                    //Delete uploaded json file
                    DeleteFile(destinationPath);

                }
            }

            //Delete uploaded zip file
            DeleteFile(fullPath);
            
            //Delete temp directory
            DeleteDirectory(uploadPath);
        }

        private void FilterButtonLabels(JObject jObject)
        {
            var pages = jObject["pages"].ToList();

            foreach (var page in pages)
            {
                var buttons = page.Children()["buttons"].ToList();
                foreach (var button in buttons)
                {
                    var labels = button.Children()["label"].ToList();
                    foreach (var label in labels)
                    {
                        var jProperty = label.Parent as JProperty;
                        var stringLabel = jProperty.Value.ToString();
                        if (stringLabel.Contains("<br>"))
                        {
                            var resultString = stringLabel.Replace("<br>", string.Empty);

                            jProperty.Value = resultString;
                        }
                    }
                }
            }
        }

        private void DeleteDirectory(string uploadPath)
        {
            if (Directory.Exists(uploadPath))
            {
                Directory.Delete(uploadPath);
            }
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private long IncrementVersion(long version)
        {
            return version + 1;
        }
    }
}