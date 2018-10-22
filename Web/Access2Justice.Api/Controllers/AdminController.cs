using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Access2Justice.Api.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public AdminController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("upload-curated-experience-template")]
        public ActionResult UploadCuratedExperienceTemplate()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
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

                    using (ZipArchive archive = ZipFile.OpenRead(fullPath))
                    {
                        string destinationPath = string.Empty;
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            Match match = Regex.Match(entry.FullName, @"template([0-9\-]+)\.json$",
                                            RegexOptions.IgnoreCase);

                            if (match.Success)
                            {
                                destinationPath = Path.GetFullPath(Path.Combine(newPath, entry.FullName));

                                if (destinationPath.StartsWith(newPath, StringComparison.Ordinal))
                                    entry.ExtractToFile(destinationPath);
                            }
                            //ToDo - Extract child node Json content from Template2,3... and add to template1.json

                        }
                    }
                    //Delete uploaded files
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }
    }
}