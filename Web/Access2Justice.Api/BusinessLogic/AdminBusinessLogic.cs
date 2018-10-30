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

namespace Access2Justice.Api.BusinessLogic
{
    public class AdminBusinessLogic : IAdminBusinessLogic
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ICuratedExperienceConvertor a2jAuthorBuisnessLogic;
        private readonly IAdminSettings adminSettings;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;

        public AdminBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService, IHostingEnvironment hostingEnvironment,
            ICuratedExperienceConvertor a2jAuthorBuisnessLogic, IAdminSettings adminSettings,
            ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic)
        {
            this.dynamicQueries = dynamicQueries;
            this.cosmosDbSettings = cosmosDbSettings;
            this.backendDatabaseService = backendDatabaseService;
            this.hostingEnvironment = hostingEnvironment;
            this.a2jAuthorBuisnessLogic = a2jAuthorBuisnessLogic;
            this.adminSettings = adminSettings;
            this.topicsResourcesBusinessLogic = topicsResourcesBusinessLogic;
        }

        public async Task<dynamic> UploadCuratedContentPackage(CuratedTemplate curatedTemplate)
        {
            IFormFile file = curatedTemplate.TemplateFile.First();
            if (file.Length > 0 && Path.GetExtension(file.FileName) == Constants.A2JTemplateFileExtension)
            {
                var templateOrder = new List<JToken>();
                List<CuratedExperiencePackageTemplateModel> templateModel = new
                    List<CuratedExperiencePackageTemplateModel>();

                ExtractZipContent(file, out templateOrder, out templateModel);

                var newTemplateId = Guid.NewGuid();
                string resourceTitle = string.Empty;

                GetTemplates(templateOrder, templateModel, out JObject mainTemplate, out JObject guideTemplate, out resourceTitle);

                if (resourceTitle.ToUpperInvariant() == curatedTemplate.Name.ToUpperInvariant())
                {
                    var response = await ImportA2JTemplate(mainTemplate, newTemplateId);
                    if (response != null && response.Id != null)
                    {
                        var curatedDocumentResponse = await ImportCuratedTemplate(guideTemplate, newTemplateId);
                        if (curatedDocumentResponse != null && curatedDocumentResponse.Id != null)
                        {
                            return await CreateGuidedAssistantResource(resourceTitle, curatedTemplate, curatedDocumentResponse.Id);
                        }
                    }
                }
            }
            return null;
        }

        private void GetTemplates(List<JToken> templateOrder, List<CuratedExperiencePackageTemplateModel> templateModel,
            out JObject mainTemplate, out JObject guideTemplate, out string resourceTitle)
        {
            var parentTemplateId = templateOrder.FirstOrDefault().ToString();
            mainTemplate = null;
            guideTemplate = null;
            JArray mainTemplateChildren = null;
            resourceTitle = string.Empty;

            foreach (var model in templateModel)
            {
                if (model.TemplateName == parentTemplateId)
                {
                    mainTemplate = model.Template;
                    resourceTitle = mainTemplate.GetValue("title").ToString();
                    JObject rootNode = (JObject)mainTemplate[adminSettings.RootNode];
                    mainTemplateChildren = (JArray)rootNode[adminSettings.ChildrenNode];
                }
                else if (model.TemplateName != parentTemplateId && model.TemplateName.ToUpperInvariant() != adminSettings.GuideTemplateFileName.ToUpperInvariant())
                {
                    JObject childTemplate = model.Template;
                    JObject childRootNode = (JObject)childTemplate[adminSettings.RootNode];
                    JArray childrenNode = (JArray)childRootNode[adminSettings.ChildrenNode];
                    foreach (var child in childrenNode)
                    {
                        mainTemplateChildren.Add(child);
                    }
                }
                else if (model.TemplateName.ToUpperInvariant() == adminSettings.GuideTemplateFileName.ToUpperInvariant())
                {
                    guideTemplate = model.Template;
                }
            }
        }

        public async Task<Document> ImportA2JTemplate(JObject mainTemplate, Guid newTemplateId)
        {
            mainTemplate.AddFirst(new JProperty(Constants.Id, newTemplateId));

            return await backendDatabaseService.CreateItemAsync(mainTemplate,
                cosmosDbSettings.A2JAuthorDocsCollectionId);
        }

        public async Task<Document> ImportCuratedTemplate(JObject guideTemplate, Guid newTemplateId)
        {
            var curatedExprienceJson = a2jAuthorBuisnessLogic.ConvertA2JAuthorToCuratedExperience(guideTemplate, true);

            curatedExprienceJson.A2jPersonalizedPlanId = newTemplateId;

            return await backendDatabaseService.UpdateItemAsync(curatedExprienceJson.CuratedExperienceId.ToString(),
                JObject.FromObject(curatedExprienceJson), cosmosDbSettings.CuratedExperiencesCollectionId);
        }

        public async Task<object> CreateGuidedAssistantResource(string resourceTitle, CuratedTemplate curatedTemplate, string curatedExperienceId)
        {
            var resourceDetails = await topicsResourcesBusinessLogic.GetResourceDetailAsync(resourceTitle, Constants.GuidedAssistant);
            List<GuidedAssistant> resources = JsonUtilities.DeserializeDynamicObject<List<GuidedAssistant>>(resourceDetails);
            float maxVersion = default(float);
            foreach (var resource in resources)
            {
                var resourceDetail = JsonUtilities.DeserializeDynamicObject<GuidedAssistant>(resource);
                if (resourceDetail.Version == default(float))
                {
                    resourceDetail.Version = IncrementMajorVersion(default(float));
                }
                else
                {
                    resourceDetail.Version = IncrementMinorVersion(resourceDetail.Version);
                }
                if (maxVersion.CompareTo(resourceDetail.Version) < 0)
                    maxVersion = resourceDetail.Version;
                resourceDetail.IsActive = false;

                await backendDatabaseService.UpdateItemAsync(resourceDetail.ResourceId.ToString(),
                    JObject.FromObject(resourceDetail), cosmosDbSettings.ResourcesCollectionId);
            }

            var topicDetails = await topicsResourcesBusinessLogic.GetTopicDetailsAsync(resourceTitle);
            List<Topic> topics = JsonUtilities.DeserializeDynamicObject<List<Topic>>(topicDetails);
            if (topics.Count > 0)
            {
                var guidedAssistantResource = new GuidedAssistant();
                List<TopicTag> topicTags = new List<TopicTag>();
                List<Location> locations = new List<Location>();

                foreach (var topic in topics)
                {
                    topicTags.Add(new TopicTag { TopicTags = topic.Id });
                    foreach (var location in topic.Location)
                    {
                        locations.Add(location);
                    }
                }

                guidedAssistantResource.ResourceId = Guid.NewGuid();
                guidedAssistantResource.Name = curatedTemplate.Name;
                guidedAssistantResource.Description = curatedTemplate.Description;
                guidedAssistantResource.TopicTags = topicTags;
                guidedAssistantResource.Location = locations;
                guidedAssistantResource.Version = IncrementMajorVersion(maxVersion);
                guidedAssistantResource.IsActive = true;
                guidedAssistantResource.ResourceType = Constants.GuidedAssistant;
                guidedAssistantResource.CuratedExperienceId = curatedExperienceId;

                return await backendDatabaseService.CreateItemAsync(guidedAssistantResource, cosmosDbSettings.ResourcesCollectionId);
            }
            return string.Format(CultureInfo.InvariantCulture, "Import failed: {0} Topic document is not available in the system. " +
                "Please create/import the topic document before importing curated experience template.", resourceTitle);
        }

        private void GetFilePath(IFormFile file, out string fullPath, out string uploadPath)
        {
            string webRootPath = hostingEnvironment.WebRootPath;
            uploadPath = string.Empty;
            if (webRootPath != null)
            {
                uploadPath = Path.Combine(webRootPath, adminSettings.UploadFolderName);
            }
            else
            {
                uploadPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), adminSettings.WebRootFolderName),
                    adminSettings.UploadFolderName);
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
            string fullPath = string.Empty;
            string uploadPath = string.Empty;

            GetFilePath(file, out fullPath, out uploadPath);

            templateModel = new List<CuratedExperiencePackageTemplateModel>();
            templateOrder = new List<JToken>();

            using (ZipArchive archive = ZipFile.OpenRead(fullPath))
            {
                string destinationPath = string.Empty;
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.Equals(adminSettings.BaseTemplateFileFullName))
                    {
                        destinationPath = Path.GetFullPath(Path.Combine(uploadPath, entry.FullName));

                        if (destinationPath.StartsWith(uploadPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath);

                        using (StreamReader reader = new StreamReader(destinationPath))
                        {
                            string json = reader.ReadToEnd();
                            var parser = JObject.Parse(json);
                            templateOrder = parser.Properties().GetArrayValue(adminSettings.BaseTemplatePropertyForTemplateOrder)
                                   .FirstOrDefault().ToList();
                        }
                        //Delete uploaded json file
                        DeleteFile(destinationPath);
                    }

                    Match match = Regex.Match(entry.FullName, @"(" + adminSettings.TemplateFileName +
                        @"([0-9\-]+)\.json$)|(" + adminSettings.GuideTemplateFileName + @"\.json)",
                        RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        destinationPath = Path.GetFullPath(Path.Combine(uploadPath, entry.FullName));

                        if (destinationPath.StartsWith(uploadPath, StringComparison.Ordinal))
                            entry.ExtractToFile(destinationPath);

                        using (StreamReader reader = new StreamReader(destinationPath))
                        {
                            string json = reader.ReadToEnd();
                            templateModel.Add(new CuratedExperiencePackageTemplateModel
                            {
                                TemplateName = Path.GetFileNameWithoutExtension(entry.FullName).
                                Replace(adminSettings.TemplateFileName, ""),
                                Template = JObject.Parse(json)
                            });
                        }
                        //Delete uploaded json file
                        DeleteFile(destinationPath);
                    }
                }
            }

            //Delete uploaded zip file
            DeleteFile(fullPath);
        }

        private void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private float IncrementMajorVersion(float version)
        {
            return version + 1.0F;
        }

        private float IncrementMinorVersion(float version)
        {
            return version + 0.1F;
        }
    }
}