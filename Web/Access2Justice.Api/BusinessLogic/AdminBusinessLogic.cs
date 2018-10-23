using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IHostingEnvironment hostingEnvironment;

        public AdminBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService, IHostingEnvironment webHostingEnvironment)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            hostingEnvironment = webHostingEnvironment;
        }

        public async Task<object> UploadCuratedContentPackage(IFormFile file)
        {
            try
            {
                string folderName = "Upload";
                string webRootPath = hostingEnvironment.WebRootPath;
                string newPath = string.Empty;
                if (webRootPath != null)
                {
                    newPath = Path.Combine(webRootPath, folderName);
                }
                else
                {
                    newPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), folderName);
                }
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    if (!newPath.EndsWith(Path.DirectorySeparatorChar))
                        newPath += Path.DirectorySeparatorChar;

                    var templateOrder = new List<JToken>();
                    List<CuratedExperiencePackageTemplateModel> templateModel = new
                        List<CuratedExperiencePackageTemplateModel>();

                    using (ZipArchive archive = ZipFile.OpenRead(fullPath))
                    {
                        string destinationPath = string.Empty;
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.FullName.Equals("templates.json"))
                            {
                                destinationPath = Path.GetFullPath(Path.Combine(newPath, entry.FullName));

                                if (destinationPath.StartsWith(newPath, StringComparison.Ordinal))
                                    entry.ExtractToFile(destinationPath);

                                using (StreamReader reader = new StreamReader(destinationPath))
                                {
                                    string json = reader.ReadToEnd();
                                    var parser = JObject.Parse(json);
                                    templateOrder = parser.Properties().GetArrayValue("templateIds")
                                           .FirstOrDefault().ToList();
                                }
                                //Delete uploaded json file
                                DeleteFile(destinationPath);
                            }

                            Match match = Regex.Match(entry.FullName, @"template([0-9\-]+)\.json$",
                                        RegexOptions.IgnoreCase);

                            if (match.Success)
                            {
                                destinationPath = Path.GetFullPath(Path.Combine(newPath, entry.FullName));

                                if (destinationPath.StartsWith(newPath, StringComparison.Ordinal))
                                    entry.ExtractToFile(destinationPath);

                                using (StreamReader reader = new StreamReader(destinationPath))
                                {
                                    string json = reader.ReadToEnd();
                                    templateModel.Add(new CuratedExperiencePackageTemplateModel
                                    {
                                        TemplateName = Path.GetFileNameWithoutExtension(entry.FullName).Replace("template", ""),
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

                    var parentTemplateId = templateModel.FirstOrDefault().TemplateName;
                    JObject mainTemplate = null;
                    JArray mainTemplateChildren = null;

                    foreach (var model in templateModel)
                    {
                        if (model.TemplateName == parentTemplateId)
                        {
                            mainTemplate = model.Template;
                            JObject rootNode = (JObject)mainTemplate["rootNode"];
                            mainTemplateChildren = (JArray)rootNode["children"];
                        }
                        else if (model.TemplateName != parentTemplateId)
                        {
                            JObject childTemplate = model.Template;
                            JObject childRootNode = (JObject)childTemplate["rootNode"];
                            JArray childrenNode = (JArray)childRootNode["children"];
                            foreach (var child in childrenNode)
                            {
                                mainTemplateChildren.Add(child);
                            }
                        }
                    }
                    var newTemplateId = Guid.NewGuid();
                    mainTemplate.AddFirst(new JProperty("id", newTemplateId));
                    await dbService.CreateItemAsync(mainTemplate, dbSettings.A2JAuthorTemplatesCollectionId);
                    return "Upload Successful.";
                }
                return "Provide the file to upload.";
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