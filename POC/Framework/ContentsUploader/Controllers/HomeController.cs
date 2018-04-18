using ContentDataAccess;
using ContentsUploader.ContentUploadManager;
using ContentsUploader.Models;
using ContentUploader.Model;
using CrawledContentDataAccess;
using CrawledContentDataAccess.StateBasedContents;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentsUploader.Controllers
{
    public class HomeController : Controller
    {
        private string dbSourceConnectionStringName = ConfigurationManager.AppSettings["SourceConnectionString"];
        private string state = ConfigurationManager.AppSettings["State"];

        [Inject]
        private readonly IExcelUploadManager _iExcelUploadManager;

        private IContentDataRepository _iContentDataRepository;
        //[Inject]
        //  public HomeController() { }
        public HomeController(IExcelUploadManager iExcelUploadManager, IContentDataRepository iContentDataRepository)
        {
            this._iExcelUploadManager = iExcelUploadManager;
            this._iContentDataRepository = iContentDataRepository;
        }
        [HttpPost]
        public ActionResult RollbackMigration(int migrationId)
        {
            _iExcelUploadManager.RollbackMigration(migrationId, "ContentsDb_AK");
            ViewBag.SectionText = "Rollbacked Migrated data";
            return View("UploadedContents", migrationId);
        }

        public ActionResult Index()
        {
            var model = new LanguageTranslation
            {
                SupportedLanguages = _iContentDataRepository
                .GetSupportedLanguages(dbSourceConnectionStringName).OrderBy(lang => lang.LanguageName).Select(lang => new SelectListItem { Value = lang.LanguageCode, Text = lang.LanguageName }).ToList()
            };
            ViewBag.ContentUploadOptionModel = new ContentUploadOptions();
            ViewBag.LanguageTranslationModel = model;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Access to Justice related Content Extraction aimed for Curated Experience";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Content Extraction fot Access to Justice Application";

            return View();
        }

        [HttpPost]        
        public string Translate(string selectedLanguage)
        {
            var isSucceeded= TriggerWebJobForContentTransaltion(selectedLanguage);
            if (isSucceeded)
            {
                return "Tanslation Job is submitted successfully";
            }
            else
            {
                return "Translation Job is not submitted successfully";
            }            
        }

        private bool TriggerWebJobForContentTransaltion(string selectedLanguage)
        {
            try
            {
                //App Service Publish Profile Credentials 
                string userName = "$ContentsTranslator"; //userName 
                string userPassword = "kyNoTTR8DddeLdvlyg9tE3llsjg97s0PoXG2Kv4T7tHwuGoHZB37mCoScnSc"; //userPWD 

                //change webJobName to your WebJob name 
                string webJobName = "ContentsTranslator";

                var unEncodedString = String.Format($"{userName}:{userPassword}");
                var encodedString = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(unEncodedString));

                var batchId = _iContentDataRepository.GetRecentBatchId(dbSourceConnectionStringName);

                //Change this URL to your WebApp hosting the 
                //contenttranslator.scm.azurewebsites.net:443
                string URL = "https://contentstranslator.scm.azurewebsites.net:443/api/triggeredwebjobs/" + webJobName +"/run?arguments=" + batchId + " " + state + " " + selectedLanguage;
                System.Net.WebRequest request = System.Net.WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentLength = 0;
                request.Headers["Authorization"] = "Basic " + encodedString;
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream dataStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();               
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileUpload(HttpPostedFileBase fileBase, UploadOptios? SelectedOption)
        {
            
            List<string> errorMessages = new List<string>();
            int result = 0;
            try
            {
                var model = new LanguageTranslation
                {
                    SupportedLanguages = _iContentDataRepository
               .GetSupportedLanguages(dbSourceConnectionStringName).OrderBy(lang => lang.LanguageName).Select(lang => new SelectListItem { Value = lang.LanguageCode, Text = lang.LanguageName }).ToList()
                };
                ViewBag.ContentUploadOptionModel = new ContentUploadOptions();
                ViewBag.LanguageTranslationModel = model;

                if (SelectedOption == null)
                {
                   
                    ModelState.AddModelError("invalid-upload-option", "<span style=\"color:red\">You must select valid content type to Upload</span>");
                    return View("Index", ModelState);

                }

                if (SelectedOption == UploadOptios.CuratedExperience)
                {
                    result = _iExcelUploadManager.PersistUploadedExcelContentForCuraredExperience(fileBase, errorMessages);
                }
                else if (SelectedOption == UploadOptios.Video)
                {
                    result = _iExcelUploadManager.PersistUploadedExcelContentForVideo(fileBase, errorMessages);
                }
                else if (SelectedOption == UploadOptios.QA)
                {
                    result = _iExcelUploadManager.PersistUploadedExcelContentForQA(fileBase, errorMessages);
                }
                if (errorMessages.Count > 0)
                {
                    for (int i = 0; i < errorMessages.Count; i++)
                    {
                        ModelState.AddModelError((i + 1).ToString(), errorMessages[i]);
                    }
                    
                    return View("Index", ModelState);
                }

                // ValidateFileName(fileBase);

                /*  using (var reader = new BinaryReader(fileBase.InputStream))
                  {
                      var content = reader.ReadBytes(fileBase.ContentLength);
                      result = _iCsvUploadManager.PersistUploadedCsvContent(fileBase.FileName, content);
                  }*/


            }
           /* catch (CsvContentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }*/
            catch (Exception ex)
            {
                ModelState.AddModelError("file-upload-error", "<span style=\"color:red\">" + ex.Message + "</span>");
                return View("Index",ModelState);
            }
            //   return new ExtractedContentModel { Resource = "test Resouce", Summary = result.ExtractedText };
            //  return RedirectToAction("ViewUploadedContent", new { migrationId= result });
            if (SelectedOption == UploadOptios.CuratedExperience)
            {
                ViewBag.SectionText = "Successfully Uploaded  Excel contents for Curated Experience in to the target data store";
                return View("UploadedContents", result);
            }
            else if(SelectedOption == UploadOptios.Video)
            {
                ViewBag.SectionText = "Successfully Uploaded  Excel contents for Video in to the target data store";
                return View("UploadedVideoContents", result);
            }
            else if (SelectedOption == UploadOptios.QA)
            {
                ViewBag.SectionText = "Successfully Uploaded  Excel contents for Questions and Answers in to the target data store";
                return View("UploadedQAContents", result);
            }
            //if none of them matched
            return View("Index");
            
        }
        public ActionResult ViewUploadedContent(int migrationId)
        {
            return View("UploadedContents");
        }

        #region Get data method.

        /// <summary>
        /// GET: /Home/GetScenarioData
        /// </summary>
        /// <returns>Return data</returns>
        public ActionResult GetScenarioData(int migrationId)
        {
            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                List<CrawledContentDataAccess.Scenario> data = this.LoadScenarioData(migrationId);

                // Total record count.
                int totalRecords = data.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.ScenarioId.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.LC_ID.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Description.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Outcome.ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                data = this.SortScenarioByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count;

                // Apply pagination.
                data = data.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }


        /// <summary>
        /// GET: /Home/GetLawCategoryData
        /// </summary>
        /// <returns>Return data</returns>
        public ActionResult GetLawCategoryData(int migrationId)
        {
            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                List<LawCategory> data = this.LoadLawCategoryData(migrationId);

                // Total record count.
                int totalRecords = data.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.LCID.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.NSMICode.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Description.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.StateDeviation.ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                data = this.SortLawCategoryByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count;

                // Apply pagination.
                data = data.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }

        /// <summary>
        /// GET: /Home/GetProcessData
        /// </summary>
        /// <returns>Return data</returns>
        public ActionResult GetProcessData(int migrationId)
        {
            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                var data = this.LoadProcessData(migrationId);

                // Total record count.
                int totalRecords = data.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.Id.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Title.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Description.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.ActionJson.ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                data = this.SortProcessByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count;

                // Apply pagination.
                data = data.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }

        /// <summary>
        /// GET: /Home/GetResourceData
        /// </summary>
        /// <returns>Return data</returns>
        public ActionResult GetResourceData(int migrationId)
        {
            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                var data = this.LoadResourceData(migrationId);

                // Total record count.
                int totalRecords = data.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.ResourceId.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.ResourceType.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.ResourceJson.ToString().ToLower().Contains(search.ToLower()) ||                                           
                                           p.Title.ToLower().Contains(search.ToLower()) ||
                                           p.Action.ToLower().Contains(search.ToLower()) ||
                                           p.LC_ID.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                data = this.SortResourceByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count;

                // Apply pagination.
                data = data.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }


        /// <summary>
        /// GET: /Home/GetQAData
        /// </summary>
        /// <returns>Return data</returns>
        public ActionResult GetQAData(int migrationId)
        {

            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                var data = this.LoadQAData(migrationId);

                // Total record count.
                int totalRecords = data.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.QAId.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Question.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Answer.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.NsmiCode.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Intent.ToString().ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                data = this.SortQAByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count;

                // Apply pagination.
                data = data.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }

      



        /// <summary>
        /// GET: /Home/GetVideoData
        /// </summary>
        /// <returns>Return data</returns>
        public ActionResult GetVideoData(int migrationId)
        {

            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                var data = this.LoadVideoData(migrationId);

                // Total record count.
                int totalRecords = data.Count;

                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search
                    data = data.Where(p => p.VideoId.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Title.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Url.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.ResourceJson.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.ActionType.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.Url.ToLower().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                data = this.SortVideoByColumnWithOrder(order, orderDir, data);

                // Filter record count.
                int recFilter = data.Count;

                // Apply pagination.
                data = data.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }


            private List<CrawledContentDataAccess.Resource> LoadResourceData(int migrationId)
        {
            // Initialization.
            List<CrawledContentDataAccess.Resource> lst = new List<CrawledContentDataAccess.Resource>();

            try
            {
                lst = _iContentDataRepository.GetRelevantResources(migrationId, "ContentsDb_AK");
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        private List<CrawledContentDataAccess.Process> LoadProcessData(int migrationId)
        {
            // Initialization.
            List<CrawledContentDataAccess.Process> lst = new List<CrawledContentDataAccess.Process>();

            try
            {
                lst = _iContentDataRepository.GetRelevantProcesses(migrationId, "ContentsDb_AK");
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        private List<LawCategory> LoadLawCategoryData(int migrationId)
        {
            // Initialization.
            List< LawCategory> lst = new List<LawCategory>();

            try
            {
                lst = _iContentDataRepository.GetLawCategories(migrationId, "ContentsDb_AK");
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        #endregion


        #region Helpers

        #region Load Data

        /// <summary>
        /// Load data method.
        /// </summary>
        /// <returns>Returns - Data</returns>
        private List<CrawledContentDataAccess.Scenario> LoadScenarioData(int migrationId)
        {
            // Initialization.
            List<CrawledContentDataAccess.Scenario> lst = new List<CrawledContentDataAccess.Scenario>();

            try
            {
                lst = _iContentDataRepository.GetScenarios(migrationId,"ContentsDb_AK");
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }


        private List<CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers> LoadQAData(int migrationId)
        {
            // Initialization.
            var lst = new List<CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers>();

            try
            {
                lst = _iContentDataRepository.GetQuestionsAndAnswers(migrationId, "ContentsDb_AK");


            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        /// <summary>
        /// Load data method.
        /// </summary>
        /// <returns>Returns - Data</returns>
        private List<CrawledContentDataAccess.StateBasedContents.Video> LoadVideoData(int migrationId)
        {
            // Initialization.
            var lst = new List<CrawledContentDataAccess.StateBasedContents.Video>();

            try
            {
                lst = _iContentDataRepository.GetVideos(migrationId, "ContentsDb_AK");

               
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }
        #endregion

        #region Sort by column with order method

        /// <summary>
        /// Sort by column with order method.
        /// </summary>
        /// <param name="order">Order parameter</param>
        /// <param name="orderDir">Order direction parameter</param>
        /// <param name="data">Data parameter</param>
        /// <returns>Returns - Data</returns>
        private List<CrawledContentDataAccess.Scenario> SortScenarioByColumnWithOrder(string order, string orderDir, List<CrawledContentDataAccess.Scenario> data)
        {
            // Initialization.
            List<CrawledContentDataAccess.Scenario> lst = new List<CrawledContentDataAccess.Scenario>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScenarioId).ToList()
                                                                                                 : data.OrderBy(p => p.ScenarioId).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LC_ID).ToList()
                                                                                                 : data.OrderBy(p => p.LC_ID).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList()
                                                                                                 : data.OrderBy(p => p.Description).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Outcome).ToList()
                                                                                                 : data.OrderBy(p => p.Outcome).ToList();
                        break;

                   

                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ScenarioId).ToList()
                                                                                                 : data.OrderBy(p => p.ScenarioId).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        /// <summary>
        /// Sort by column with order method.
        /// </summary>
        /// <param name="order">Order parameter</param>
        /// <param name="orderDir">Order direction parameter</param>
        /// <param name="data">Data parameter</param>
        /// <returns>Returns - Data</returns>
        private List<LawCategory> SortLawCategoryByColumnWithOrder(string order, string orderDir, List<LawCategory> data)
        {
            // Initialization.
            List<LawCategory> lst = new List<LawCategory>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LCID).ToList()
                                                                                                 : data.OrderBy(p => p.LCID).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NSMICode).ToList()
                                                                                                 : data.OrderBy(p => p.NSMICode).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList()
                                                                                                 : data.OrderBy(p => p.Description).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StateDeviation).ToList()
                                                                                                 : data.OrderBy(p => p.StateDeviation).ToList();
                        break;



                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LCID).ToList()
                                                                                                 : data.OrderBy(p => p.LCID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        private List<CrawledContentDataAccess.Process> SortProcessByColumnWithOrder(string order, string orderDir, List<CrawledContentDataAccess.Process> data)
        {
            // Initialization.
            List<CrawledContentDataAccess.Process> lst = new List<CrawledContentDataAccess.Process>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Id).ToList()
                                                                                                 : data.OrderBy(p => p.Id).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Title).ToList()
                                                                                                 : data.OrderBy(p => p.Title).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList()
                                                                                                 : data.OrderBy(p => p.Description).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ActionJson).ToList()
                                                                                                 : data.OrderBy(p => p.ActionJson).ToList();
                        break;



                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Id).ToList()
                                                                                                 : data.OrderBy(p => p.Id).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        private List<CrawledContentDataAccess.Resource> SortResourceByColumnWithOrder(string order, string orderDir, List<CrawledContentDataAccess.Resource> data)
        {
            // Initialization.
            List<CrawledContentDataAccess.Resource> lst = new List<CrawledContentDataAccess.Resource>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResourceId).ToList()
                                                                                                 : data.OrderBy(p => p.ResourceId).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResourceType).ToList()
                                                                                                 : data.OrderBy(p => p.ResourceType).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResourceJson).ToList()
                                                                                                 : data.OrderBy(p => p.ResourceJson).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Title).ToList()
                                                                                                 : data.OrderBy(p => p.Title).ToList();
                        break;

                    case "4":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Action).ToList()
                                                                                                 : data.OrderBy(p => p.Action).ToList();
                        break;

                    case "5":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LC_ID).ToList()
                                                                                                 : data.OrderBy(p => p.LC_ID).ToList();
                        break;


                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResourceId).ToList()
                                                                                                 : data.OrderBy(p => p.ResourceId).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        private List<CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers> SortQAByColumnWithOrder(string order, string orderDir, List<CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers> data)
        {
            // Initialization.
            var lst = new List<CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QAId).ToList()
                                                                                                 : data.OrderBy(p => p.QAId).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Question).ToList()
                                                                                                 : data.OrderBy(p => p.Question).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Answer).ToList()
                                                                                                 : data.OrderBy(p => p.Answer).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NsmiCode).ToList()
                                                                                                 : data.OrderBy(p => p.NsmiCode).ToList();
                        break;

                    case "4":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Intent).ToList()
                                                                                                 : data.OrderBy(p => p.Intent).ToList();
                        break;




                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.QAId).ToList()
                                                                                                 : data.OrderBy(p => p.QAId).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }
        /// <summary>
        /// Sort by column with order method.
        /// </summary>
        /// <param name="order">Order parameter</param>
        /// <param name="orderDir">Order direction parameter</param>
        /// <param name="data">Data parameter</param>
        /// <returns>Returns - Data</returns>
        private List<CrawledContentDataAccess.StateBasedContents.Video> SortVideoByColumnWithOrder(string order, string orderDir, List<CrawledContentDataAccess.StateBasedContents.Video> data)
        {
            // Initialization.
            var lst = new List<CrawledContentDataAccess.StateBasedContents.Video>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VideoId).ToList()
                                                                                                 : data.OrderBy(p => p.VideoId).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Title).ToList()
                                                                                                 : data.OrderBy(p => p.Title).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Url).ToList()
                                                                                                 : data.OrderBy(p => p.Url).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ResourceJson).ToList()
                                                                                                 : data.OrderBy(p => p.ResourceJson).ToList();
                        break;

                    case "4":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ActionType).ToList()
                                                                                                 : data.OrderBy(p => p.ActionType).ToList();
                        break;

                    case "5":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LCID).ToList()
                                                                                                 : data.OrderBy(p => p.LCID).ToList();
                        break;



                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VideoId).ToList()
                                                                                                 : data.OrderBy(p => p.VideoId).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        #endregion

        #endregion
    }
}