using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;

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

        public async Task<object> UploadCuratedContentPackage(CuratedTemplate curatedTemplate)
        {
            try
            {
                if (curatedTemplate.TemplateFile.Count() == 0)
                    return "Please select the file to upload!";

                List<IFormFile> files = curatedTemplate.TemplateFile;
                string webRootPath = hostingEnvironment.WebRootPath;
                string uploadPath = string.Empty;
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
                foreach (IFormFile file in files)
                {
                    if (file.Length > 0)
                    {
                        string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        string fullPath = Path.Combine(uploadPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        if (!uploadPath.EndsWith(Path.DirectorySeparatorChar))
                            uploadPath += Path.DirectorySeparatorChar;

                        var templateOrder = new List<JToken>();
                        List<CuratedExperiencePackageTemplateModel> templateModel = new
                            List<CuratedExperiencePackageTemplateModel>();

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

                        var parentTemplateId = templateOrder.FirstOrDefault().ToString();
                        JObject mainTemplate = null;
                        JArray mainTemplateChildren = null;
                        JObject GuideTemplate = null;
                        string resourceTitle = string.Empty;

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
                                GuideTemplate = model.Template;
                            }
                        }
                        var newTemplateId = Guid.NewGuid();
                        mainTemplate.AddFirst(new JProperty(Constants.Id, newTemplateId));
                        var response = await backendDatabaseService.CreateItemAsync(mainTemplate,
                            cosmosDbSettings.A2JAuthorTemplatesCollectionId);
                        if (response != null && response.Id != null)
                        {
                            var resourceDetails = await topicsResourcesBusinessLogic.GetResourceDetailAsync(resourceTitle, Constants.GuidedAssistant);
                            List<GuidedAssistant> resources = JsonUtilities.DeserializeDynamicObject<List<GuidedAssistant>>(resourceDetails);
                            float maxVersion = default(float);
                            foreach(var resource in resources)
                            {
                                var resourceDetail = JsonUtilities.DeserializeDynamicObject<GuidedAssistant>(resource);
                                if(resourceDetail.Version == default(float))
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
                                    JObject.FromObject(resourceDetail), cosmosDbSettings.ResourceCollectionId);
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
                                
                                var curatedExprienceJson = a2jAuthorBuisnessLogic.ConvertA2JAuthorToCuratedExperience(GuideTemplate, true);
                                curatedExprienceJson.A2jPersonalizedPlanId = newTemplateId;

                                guidedAssistantResource.ResourceId = Guid.NewGuid();
                                guidedAssistantResource.Name = curatedTemplate.Name;
                                guidedAssistantResource.Description = curatedTemplate.Description;
                                guidedAssistantResource.TopicTags = topicTags;
                                guidedAssistantResource.Location = locations;
                                guidedAssistantResource.Version = IncrementMajorVersion(maxVersion);
                                guidedAssistantResource.IsActive = true;
                                guidedAssistantResource.ResourceType = Constants.GuidedAssistant;
                                guidedAssistantResource.CuratedExperienceId = curatedExprienceJson.CuratedExperienceId.ToString();

                                await backendDatabaseService.UpdateItemAsync(curatedExprienceJson.CuratedExperienceId.ToString(),
                                    JObject.FromObject(curatedExprienceJson), cosmosDbSettings.CuratedExperienceCollectionId);

                                await backendDatabaseService.CreateItemAsync(guidedAssistantResource, cosmosDbSettings.ResourceCollectionId);
                            }
                            return string.Format(CultureInfo.InvariantCulture, "Import failed: {0} Topic document is not available in the system. Please create/import the topic document before importing curated experience template.",resourceTitle);
                        }
                    }
                    else
                    {
                        return "Provide the file to upload.";
                    }
                }
                return "Upload Successful.";
            }
            catch (System.Exception ex)
            {
                return "Upload Failed: " + ex.Message;
            }
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