using System;
using System.Linq;
using ContentDataAccess;
using System.IO;
//using System.Data;
using ContentUploader.Model;
using OfficeOpenXml;
using System.Data;
using System.Web;
using CrawledContentDataAccess.StateBasedContents;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;

namespace ContentsUploader.ContentUploadManager
{
    public class ExcelUploadManager : IExcelUploadManager
    {

        private readonly IContentDataRepository _iContentDataRepository;
        private readonly ExcelFile _excelFileContent;
        //private Dictionary<string, HashSet<WeightedChildNode>> _adjencyListofCsvContent;
       // private List<string> _backcycleFormingVerticesNames;
        private readonly char[] _separationCharacters = { ',', '\r', '\n' };

        public ExcelUploadManager(IContentDataRepository iContentDataRepository)
        {
            this._iContentDataRepository = iContentDataRepository;
            this._excelFileContent = new ExcelFile();
        }

      
    

       

     

        
        public int PersistUploadedExcelContentForCuraredExperience(HttpPostedFileBase fileBase, List<string> errorMessages)
        {
          
            //ToDo: To be refactored
            if (string.IsNullOrEmpty(fileBase?.FileName))
            {
                throw new Exception("Please Select Excel file to Upload Curated Content");
            }
            if (Path.GetExtension(fileBase.FileName) == ".xlsx")
            {
                using (var excel = new ExcelPackage(fileBase.InputStream))
                {
                    UploadBatchReference uploadBatchReference = new UploadBatchReference
                    {
                        Name = fileBase.FileName + " Batch Upload for Curated Content",
                        TimeStamp = DateTime.Now
                    };
                    var batch_id = _iContentDataRepository.Save(uploadBatchReference, "ContentsDb_AK");
                    foreach (var ws in excel.Workbook.Worksheets)
                    {
                        
                        // var ws = excel.Workbook.Worksheets.First();
                        var hasHeader = true;  // adjust accordingly
                                               // add DataColumns to DataTable
                                               //foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                               //    tbl.Columns.Add(hasHeader ? firstRowCell.Text
                                               //        : String.Format("Column {0}", firstRowCell.Start.Column));
                        try
                        {
                            MapCellsToEntities(ws, batch_id,errorMessages);
                        }
                        catch(Exception e)
                        {
                            throw;
                        }
                    }
                    return batch_id;
                }
            }
            else
            {
                throw new Exception("Please Select only Excel file to Upload");
            }
            return -1;
        }



        private void MapCellsToEntities(ExcelWorksheet ws, int batch_id, List<string> errorMessages)
        {
           Dictionary<string, LawCategory> lawCategories = new Dictionary<string, LawCategory>();

            try
            {
                for (int rowNum = 2; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    if (wsRow.All(x => string.IsNullOrEmpty(x.Text)))
                    {
                        continue;
                    }




                    var processXml = wsRow["G" + rowNum].Text;

                    List<Process> processes = new List<Process>();

                    //Parse process steps to list of Process entities

                    ParseToProcessEntities(ws.Name, rowNum,'G', processXml, processes,errorMessages);



                    var relatedResourceXml = wsRow["J" + rowNum].Text;
                    List<Resource> resources = new List<Resource>();

                    //Parse related resources to list of Resource entities
                    ParseToResourceEntities(ws.Name,rowNum,'J',relatedResourceXml, resources,errorMessages);

                    //Todo: remove hardcode 
                    int currentColLen = 11;
                    var columnLetter = 'J';
                    while (currentColLen < ws.Dimension.End.Column)
                    {

                        columnLetter++;
                        relatedResourceXml = wsRow[columnLetter.ToString() + rowNum].Text;
                        if (!string.IsNullOrEmpty(relatedResourceXml))
                        { 
                            //Parse related resources to list of Resource entities
                            ParseToResourceEntities(ws.Name, rowNum, columnLetter, relatedResourceXml, resources, errorMessages);
                        }

                        currentColLen++;
                    }


                    //Persist Process steps
                    // _iContentDataRepository.Save(processes, "ContentsDb_AK_Test");

                    //Persist Process steps
                    // _iContentDataRepository.Save(resources, "ContentsDb_AK_Test");


                    var lawCategory = new LawCategory()
                    {
                        NSMICode = CleanUp(wsRow["C" + rowNum].Text),
                        Description = wsRow["D" + rowNum].Text,
                        StateDeviation = wsRow["E" + rowNum].Text,
                        Batch_Id = batch_id,
                        Processes = processes,
                        Resources = resources
                    };
                    var lc_id = _iContentDataRepository.Save(lawCategory, "ContentsDb_AK");
                    if(!lawCategories.ContainsKey(lawCategory.NSMICode))
                    {
                        lawCategories.Add(lawCategory.NSMICode, lawCategory);
                    }
                    var scenaio = new Scenario()
                    {
                        Description = wsRow["A" + rowNum].Text,
                        LC_ID = lc_id,
                        Outcome = wsRow["F" + rowNum].Text,
                    };

                    //Persist Scenario
                    var scenarioId=_iContentDataRepository.Save(scenaio, "ContentsDb_AK");
                    LawCategoryToScenario lawCategoryToScenario = new LawCategoryToScenario
                    {
                        LCID = lawCategories[lawCategory.NSMICode].LCID,
                        ScenarioId = scenarioId
                    };
                    _iContentDataRepository.Save(lawCategoryToScenario, "ContentsDb_AK");

                }
            }
            catch(Exception e)
            {
                //Log e
                throw;
            }
        }


        private void ParseToResourceEntities(string worksheetName, int rowNum, char colLetter, string relatedResourceXml, List<Resource> resources, List<string> errorMessages)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(relatedResourceXml);

                string title;

                var mailTitleNodes = xmlDoc.GetElementsByTagName("main_title");
                string resourceJson;
                foreach (XmlNode mainTitleNode in mailTitleNodes)
                {
                    resourceJson = null;


                    title = mainTitleNode.Attributes[0].Value;

                    if (mainTitleNode.SelectNodes("description").Count > 0)
                    {
                        foreach (XmlNode descriptionNode in mainTitleNode.SelectNodes("description"))
                        {
                            resourceJson = descriptionNode.InnerText;
                            resources.Add(new Resource { Action = "Title", Title = title, ResourceJson = resourceJson, ResourceType = "Related" });

                        }


                    }

                    // titleNode.Attributes.Item.title
                    var titleNodes = mainTitleNode.SelectNodes("title");



                    if (titleNodes.Count > 0)
                    {

                        if (titleNodes.Count == 1)
                        {
                            var hrefText = titleNodes[0].InnerText;
                            var hrefUrl = titleNodes[0].NextSibling.InnerText;
                            var mediaType = titleNodes[0].NextSibling.Attributes[0].InnerText;
                            resourceJson = "<a href=\"" + hrefUrl + "\" class =\"" + mediaType + "\" >" + hrefText + "</a>";
                            
                        }
                        else
                        {
                            resourceJson = "<ul> ";
                            foreach (XmlNode actionNode in titleNodes)
                            {
                                var hrefText = actionNode.InnerText;
                                var hrefUrl = actionNode.NextSibling.InnerText;
                                var mediaType = actionNode.NextSibling.Attributes[0].InnerText;
                                resourceJson += "<li> <a href =\"" + hrefUrl + "\" class =\"" + mediaType + "\">" + hrefText + "</a></li>";
                            }
                            resourceJson += "</ul>";
                        }

                        resources.Add(new Resource { Action = "Url", Title = title, ResourceJson = resourceJson, ResourceType = "Related" });
                    }




                    var phoneNodes = mainTitleNode.SelectNodes("phone");

                    foreach (XmlNode phoneNode in phoneNodes)
                    {
                        resourceJson = phoneNode.InnerText;
                        resources.Add(new Resource { Action = "Phone", Title = title, ResourceJson = resourceJson, ResourceType = "Related" });
                    }
                }
            }
            catch (Exception e)
            {
                errorMessages.Add($"Resource cards column content, under worksheet name =<b style=\"color:blue\">\"{worksheetName}\"</b> at cell Position <b style=\"color:green\">{colLetter}{rowNum}</b>,  has the following  issue:<span style=\"color:red\">{e.Message}</span> Please correct it and try again!");
            }
        }
       // private void ParseToResourceEntities(string relatedResourceXml, List<Resource> resources)
        private void ParseToProcessEntities(string worksheetName, int rowNum, char colLetter, string processXml, List<Process> processes, List<string> errorMessages)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(processXml);
            
            string title;

            var titleNodes = xmlDoc.GetElementsByTagName("title1");
            string description, actionJson;
            foreach (XmlNode titleNode in titleNodes)
            {
                    title = string.Empty;
                    description = actionJson = null;
                    if (titleNode != null && (titleNode.Attributes != null && titleNode.Attributes.Count > 0)){
                        title = titleNode.Attributes[0].Value;
                    }
                    else if (titleNode != null && (titleNode.Attributes != null && titleNode.Attributes.Count == 0))
                    {
                        title = titleNode.Name;
                    }
               // titleNode.Attributes.Item.title
                var stepNodes = titleNode.SelectNodes("step");
                if(stepNodes.Count == 0)
                {
                    if (titleNode.SelectNodes("substep").Count > 0)
                    {
                        stepNodes = titleNode.SelectNodes("substep");
                    }
                }
                    foreach (XmlNode stepNode in stepNodes)
                    {
                        if (stepNode.Attributes != null && stepNode.Attributes.Count > 0)
                        {
                            description = stepNode.Attributes[0].Value;
                        }
                        else
                        {
                                for (int i = 0; i < stepNode.ChildNodes.Count; i++)
                                {
                                    if (stepNode.ChildNodes[i].Attributes != null && stepNode.ChildNodes[i].Attributes.Count > 0)
                                    {
                                        description = stepNode.ChildNodes[i].Attributes[0].Value;
                                    }
                                    else
                                    {
                                        description = stepNode.ChildNodes[i].InnerText;
                                    }
                                    if (stepNode.ChildNodes[i].SelectNodes("title") != null && stepNode.ChildNodes[i].SelectNodes("title").Count > 0)
                                    {                                        
                                        actionJson = actionJson = createActionJson(stepNode.ChildNodes[i]);
                                    }
                                    processes.Add(new Process { ActionJson = actionJson, Description = description, stepType = StepType.Description, Title = title });
                                    //Reset them for each steps
                                    actionJson = description = string.Empty;
                                }

                                continue;
                            
                        }

                        if (stepNode.SelectNodes("title").Count > 0)
                        {
                            actionJson = createActionJson(stepNode);
                        }
                        processes.Add(new Process { ActionJson = actionJson, Description = description, stepType = StepType.Description, Title = title });
                            //Reset them for each steps
                            actionJson = description = string.Empty;
                        }
                    }
            }
            catch (Exception e)
            {
                errorMessages.Add($"Court Process column content, under worksheet name =<b style=\"color:blue\">\"{worksheetName}\"</b> at cell Position <b style=\"color:green\">{colLetter}{rowNum}</b>, has the following  issue:<span style=\"color:red\">{e.Message}</span> Please correct it and try again!");
            }
        }

        private static string createActionJson(XmlNode stepNode)
        {
            string actionJson;
            if (stepNode.SelectNodes("title").Count == 1)
            {
                var hrefText = stepNode.SelectNodes("title")[0].InnerText?.Trim();
                var hrefUrl = stepNode.SelectNodes("action")[0].InnerText?.Trim();
                var nextSibling = stepNode.SelectNodes("title")[0].NextSibling;
                if (nextSibling != null && nextSibling.InnerText.Trim() == string.Empty)
                {
                    nextSibling = nextSibling.NextSibling;
                    while (nextSibling != null && nextSibling.InnerText.Trim() == string.Empty)
                    {
                        nextSibling = nextSibling.NextSibling;
                    }

                }
                if (nextSibling != null)
                {
                    var mediaType = nextSibling.Attributes[0].InnerText?.Trim();
                    actionJson = "<a href=\"" + hrefUrl + "\" class =\"" + mediaType + "\">" + hrefText + "</a>";
                }
                else
                {
                    actionJson = "<a href=\"" + hrefUrl + "\">" + hrefText + "</a>";

                }

            }
            else
            {
                actionJson = "<ul> ";
                foreach (XmlNode actionNode in stepNode.SelectNodes("title"))
                {
                    var hrefText = actionNode.InnerText?.Trim();
                    var hrefUrl = actionNode.NextSibling.InnerText?.Trim();
                    // var mediaType = actionNode.NextSibling.Attributes[0].InnerText;
                    var nextSibling = stepNode.SelectNodes("title")[0].NextSibling;
                    if (nextSibling != null && nextSibling.InnerText.Trim() == string.Empty)
                    {
                        nextSibling = nextSibling.NextSibling;
                        while (nextSibling != null && nextSibling.InnerText.Trim() == string.Empty)
                        {
                            nextSibling = nextSibling.NextSibling;
                        }

                    }
                    if (nextSibling != null)
                    {
                        hrefUrl = nextSibling.InnerText?.Trim();
                        var mediaType = nextSibling.Attributes[0]?.InnerText?.Trim();
                        actionJson += "<li><a href=\"" + hrefUrl + "\" class =\"" + mediaType + "\">" + hrefText + "</a></li>";
                    }
                    else
                    {
                        actionJson += "<li><a href=\"" + hrefUrl + "\">" + hrefText + "</a></li>";

                    }

                    // actionJson += "<li> <a href =\"" + hrefUrl + "\" class =\"" + mediaType + "\">" + hrefText + "</a></li>";
                }
                actionJson += "</ul>";
            }

            return actionJson;
        }

        private string CleanUp(string input)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            return digitsOnly.Replace(input, "");
        }

        public bool RollbackMigration(int migrationId, string connectionString)
        {
            return _iContentDataRepository.RollbackMigration(migrationId, connectionString);
        }

        public int PersistUploadedExcelContentForVideo(HttpPostedFileBase fileBase, List<string> errorMessages)
        {
            //ToDo: To be refactored
            if (string.IsNullOrEmpty(fileBase?.FileName))
            {
                throw new Exception("Please Select Excel file to Upload Video Content");
            }
            if (Path.GetExtension(fileBase.FileName) == ".xlsx")
            {
                using (var excel = new ExcelPackage(fileBase.InputStream))
                {
                   
                    var batch_id = _iContentDataRepository.GetRecentBatchId("ContentsDb_AK");
                    foreach (var ws in excel.Workbook.Worksheets)
                    {

                        try
                        {
                            MapCellsToEntitiesForVideo(ws, batch_id, errorMessages);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                    return batch_id;
                }
            }
            else
            {
                throw new Exception("Please Select only Excel file to Upload");
            }
            
        }

        public int PersistUploadedExcelContentForQA(HttpPostedFileBase fileBase, List<string> errorMessages)
        {
            //ToDo: To be refactored
            if (string.IsNullOrEmpty(fileBase?.FileName))
            {
                throw new Exception("Please Select Excel file to Upload Questions and Answers Contents");
            }
            if (Path.GetExtension(fileBase.FileName) == ".xlsx")
            {
                using (var excel = new ExcelPackage(fileBase.InputStream))
                {

                    var batch_id = _iContentDataRepository.GetRecentBatchId("ContentsDb_AK");
                    foreach (var ws in excel.Workbook.Worksheets)
                    {

                        try
                        {
                            MapCellsToEntitiesForQA(ws, batch_id, errorMessages);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }
                    }
                    return batch_id;
                }
            }
            else
            {
                throw new Exception("Please Select only Excel file to Upload");
            }

        }

        private void MapCellsToEntitiesForQA(ExcelWorksheet ws, int batch_id, List<string> errorMessages)
        {
           // Dictionary<string, LawCategory> lawCategories = new Dictionary<string, LawCategory>();

            try
            {
                for (int rowNum = 3; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    if (wsRow.All(x => string.IsNullOrEmpty(x.Text)))
                    {
                        continue;
                    }

                   
                    var intent = wsRow["A" + rowNum].Text;
                    var nsmiCode = wsRow["B" + rowNum].Text;
                    var question = wsRow["C" + rowNum].Text;
                    var answer = wsRow["D" + rowNum].Text;
                    var questionAndAnswer = new CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers
                    {
                        Intent = intent,
                        NsmiCode = nsmiCode,
                        Question = question,
                        Answer = answer,
                        Batch_Id =batch_id
                    };

                    var qaByIntents=_iContentDataRepository.GetQuestionsAndAnswers(intent, "ContentsDb_AK");
                    var isQA_Exists_Already = qaByIntents.Any(qa => qa.NsmiCode == questionAndAnswer.NsmiCode && qa.Question == question && qa.Answer == answer);

                    if (!isQA_Exists_Already)
                    {
                        _iContentDataRepository.Save(questionAndAnswer, "ContentsDb_AK");
                    }


                }
            }
            catch (Exception e)
            {
                //Log e
                throw;
            }
        }

        private void MapCellsToEntitiesForVideo(ExcelWorksheet ws, int batch_id, List<string> errorMessages)
        {
            Dictionary<string, LawCategory> lawCategories = new Dictionary<string, LawCategory>();

            try
            {
                for (int rowNum = 3; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    if (wsRow.All(x => string.IsNullOrEmpty(x.Text)))
                    {
                        continue;
                    }
                    var scenarioDescription = wsRow["A" + rowNum].Text;
                    var nsmiCode = wsRow["F" + rowNum].Text;
                    Video video = new Video
                    {
                        Title = wsRow["B" + rowNum].Text,
                        Url = wsRow["C" + rowNum].Text,
                        ResourceJson = wsRow["D" + rowNum].Text,
                        ActionType = wsRow["E" + rowNum].Text,
                        LCID = _iContentDataRepository.FindLCID(batch_id,nsmiCode,scenarioDescription, "ContentsDb_AK")
                    };

                    _iContentDataRepository.Save(video, "ContentsDb_AK");


                }
            }
            catch (Exception e)
            {
                //Log e
                throw;
            }
        }


       
    }
}

