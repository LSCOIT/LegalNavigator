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

        public AdminBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService, IHostingEnvironment hostingEnvironment,
            ICuratedExperienceConvertor a2jAuthorBuisnessLogic,IAdminSettings adminSettings)
        {
            this.dynamicQueries = dynamicQueries;
            this.cosmosDbSettings = cosmosDbSettings;
            this.backendDatabaseService = backendDatabaseService;
            this.hostingEnvironment = hostingEnvironment;
            this.a2jAuthorBuisnessLogic = a2jAuthorBuisnessLogic;
            this.adminSettings = adminSettings;
        }

        public async Task<object> UploadCuratedContentPackage(List<IFormFile> files)
        {
            try
            {
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
                                    @"([0-9\-]+)\.json$)|(" + adminSettings.GuideTemplateFileName  + @"\.json)",
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

                        foreach (var model in templateModel)
                        {
                            if (model.TemplateName == parentTemplateId)
                            {
                                mainTemplate = model.Template;
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
                            var curatedExprienceJson = a2jAuthorBuisnessLogic.ConvertA2JAuthorToCuratedExperience(GuideTemplate);
                            curatedExprienceJson.A2jPersonalizedPlanId = newTemplateId;
                            await backendDatabaseService.UpdateItemAsync(curatedExprienceJson.CuratedExperienceId.ToString(), 
                                JObject.FromObject(curatedExprienceJson), cosmosDbSettings.CuratedExperienceCollectionId);
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
    }
}