using ContentDataAccess;
using ContentsTranslator.Utilities;
using CrawledContentDataAccess.StateBasedContents;
using CrawledContentsBusinessLayer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ContentsTranslator
{
    class TranslationManager
    {
        /// <summary>
        /// connection string prefix 
        /// </summary>
        private const string prefix = "ContentsDb";

        /// <summary>
        /// 
        /// </summary>
        private IContentDataRepository _contentDataRepository { get; set; }// = new ContentDataRepository(new CrowledContentDataContextFactory());

        /// <summary>
        /// 
        /// </summary>
        private ILogger _logger { get; set; }// = new ContentDataRepository(new CrowledContentDataContextFactory());

        /// <summary>
        /// 
        /// </summary>
        private string dbSourceConnectionStringName = ConfigurationManager.AppSettings["SourceConnectionString"];

        /// <summary>
        /// 
        /// </summary>
        private string[] separators = { ",", ".", "!", "?", ";", ":", " " };

        /// <summary>
        /// 
        /// </summary>
       // private List<string> listOfWords = new List<string>();

        private int numberOfTranslateCharacters {get;set;}

        private int TotalNumberOfTranslatedCharacters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentDataRepository"></param>
        /// <param name="logger"></param> 
        public TranslationManager(IContentDataRepository contentDataRepository, ILogger logger)
        {
            _contentDataRepository = contentDataRepository;
            _logger = logger;
        }


        public void TranslateSentences(int batchId, string state, string targetLanguageCode)
        {

            _logger.Information($"Entering into  TranslateSentences Method. ");

            //Reset number of characters to be translated
            
            //Dictionary to Manage LawCategoristToScenario logical attachemene
            Dictionary<string, LawCategory> lawCategoriesToScenarios = new Dictionary<string, LawCategory>();

            // Dictionary<string, string> UniqueSentencesToBeTranslated = new Dictionary<string, string>();
            var targetConnectionStringName = StateToConnectionStringMapper.ToConnectionString(prefix, state, targetLanguageCode);
            //Create DB if not existed yet
            _contentDataRepository.CreateDatabaseIfNotExists(targetConnectionStringName);
            
            var destinationSupportedLanguageName = _contentDataRepository.GetSupportedLanguages(targetConnectionStringName).Find(lang=> lang.LanguageCode == targetLanguageCode)?.LanguageName;

            var lawCategories = _contentDataRepository
                               .GetLawCategories(batchId, dbSourceConnectionStringName);
            var newBatch = new UploadBatchReference { Name = $"Translate from English to {destinationSupportedLanguageName}", TimeStamp = DateTime.Now };
            var batch_Id = _contentDataRepository.Save(newBatch, targetConnectionStringName);

            var intentToNsmiCodes = _contentDataRepository.GetIntentToNSMICodes( dbSourceConnectionStringName);
            var intentToNsmiCodesFromTagetDb = _contentDataRepository.GetIntentToNSMICodes(targetConnectionStringName);
            if (intentToNsmiCodesFromTagetDb== null || intentToNsmiCodesFromTagetDb.Count == 0)
            {
                intentToNsmiCodes.ForEach(
                 intentToNsmiCode =>
                 _contentDataRepository.Save(new IntentToNSMICode { Intent = intentToNsmiCode.Intent, NSMICode = intentToNsmiCode.NSMICode }, StateToConnectionStringMapper.ToConnectionString(prefix, state, targetLanguageCode))
                );
                _logger.Error($"Saved all the NSMI codes to target Db. ");
            }


            TotalNumberOfTranslatedCharacters = 0;
            foreach (var lawCategory in lawCategories)
            {
                numberOfTranslateCharacters = 0;
                //Translate Process Table
                List<Process> translatedProcesses = new List<Process>();
                foreach (var process in _contentDataRepository.GetRelevantProcesses(lawCategory.Batch_Id.Value, dbSourceConnectionStringName).FindAll(res=> res.LC_ID == lawCategory.LCID))
                {
                    var translatedDescription_prc = string.Empty;
                    if (!string.IsNullOrEmpty(process.Description) ) {
                        translatedDescription_prc = StripTAG(TextExtractionModule.TextTranslate(process.Description, "en", targetLanguageCode));
                        numberOfTranslateCharacters += process.Description.Length;
                            
                    }
                    var translatedTitle_prc = string.Empty;
                    if (!string.IsNullOrEmpty(process.Title))
                    {
                        translatedTitle_prc = StripTAG(TextExtractionModule.TextTranslate(process.Title, "en", targetLanguageCode));
                        numberOfTranslateCharacters += process.Title.Length;                           
                    }
                    var translatedProcess = new Process
                    {
                        ActionJson = !string.IsNullOrEmpty(process.ActionJson)?TranslateActoinJson(process.ActionJson, targetLanguageCode): null,
                        Description = translatedDescription_prc,
                        Title = translatedTitle_prc,
                        stepType = process.stepType
                    };
                    translatedProcesses.Add(translatedProcess);
                }

                //Translate Resource Table
                List<Resource> translatedResources = new List<Resource>();
                foreach (var resource in _contentDataRepository.GetRelevantResources(lawCategory.Batch_Id.Value, dbSourceConnectionStringName).FindAll(res=> res.LC_ID == lawCategory.LCID))
                {
                    var translatedTitle_res = string.Empty;
                    if (!string.IsNullOrEmpty(resource.Title))
                    {
                        translatedTitle_res = StripTAG(TextExtractionModule.TextTranslate(resource.Title, "en", targetLanguageCode));
                        numberOfTranslateCharacters += resource.Title.Length;
                    }

                    var translatedAnchor = string.Empty;
                    
                    //ToDo: Remove hardocoded constant
                    if (resource.Action == "Url")
                    {
                        if (!string.IsNullOrEmpty(resource.ResourceJson))
                        {
                            translatedAnchor = TranslateAnchor(resource.ResourceJson, targetLanguageCode);
                            numberOfTranslateCharacters += resource.ResourceJson.Length;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(resource.ResourceJson))
                        {
                            translatedAnchor = StripTAG(TextExtractionModule.TextTranslate(resource.ResourceJson, "en", targetLanguageCode));
                            //Keeping track of total nuber of characters
                            numberOfTranslateCharacters += resource.ResourceJson.Length;
                        }
                    }
                    var translatedResource = new Resource
                    {
                        ResourceJson = translatedAnchor,
                        Title = translatedTitle_res,
                        ResourceType = resource.ResourceType
                    };
                    translatedResources.Add(translatedResource);
                }

                var translatedDescription_lawCat = string.Empty;
                //Translate LawCategory Table
                if (!string.IsNullOrEmpty(lawCategory.Description))
                {
                    translatedDescription_lawCat = StripTAG(TextExtractionModule.TextTranslate(lawCategory.Description, "en", targetLanguageCode));

                    numberOfTranslateCharacters += lawCategory.Description.Length;
                }
                //item.Description = translatedText;
                var translatedStateDeviation_lawCat = string.Empty;
                if (!string.IsNullOrEmpty(lawCategory.Description))
                {
                    translatedStateDeviation_lawCat = StripTAG(TextExtractionModule.TextTranslate(lawCategory.StateDeviation, "en", targetLanguageCode));
                    numberOfTranslateCharacters += lawCategory.StateDeviation.Length;
                }
                // var lawCategories= new LawCategories { }
                var translatedLawCategory = new LawCategory
                {
                    Batch_Id = batch_Id,
                    Description = translatedDescription_lawCat,
                    StateDeviation = translatedStateDeviation_lawCat,
                    NSMICode = lawCategory?.NSMICode,
                    Processes = translatedProcesses,
                    Resources = translatedResources
                };

                var translatedLcId = _contentDataRepository.Save(translatedLawCategory, StateToConnectionStringMapper.ToConnectionString(prefix, state, targetLanguageCode));
              

                if (!lawCategoriesToScenarios.ContainsKey(translatedLawCategory.NSMICode))
                {
                    lawCategoriesToScenarios.Add(lawCategory.NSMICode, translatedLawCategory);
                }

                //Translate Scenarios Table
                foreach (var scenario in _contentDataRepository.GetScenarios(batchId, dbSourceConnectionStringName).FindAll(scn => scn.LC_ID == lawCategory.LCID))
                {
                    var translatedDescription_sen = string.Empty;
                    if (!string.IsNullOrEmpty(lawCategory.Description))
                    {
                        translatedDescription_sen = StripTAG(TextExtractionModule.TextTranslate(scenario.Description, "en", targetLanguageCode));

                        numberOfTranslateCharacters += scenario.Description.Length;

                    }
                    var translatedOutcome_sen = string.Empty;
                    if (!string.IsNullOrEmpty(lawCategory.Description))
                    {
                        translatedOutcome_sen = StripTAG(TextExtractionModule.TextTranslate(scenario.Outcome, "en", targetLanguageCode));

                        numberOfTranslateCharacters += scenario.Outcome.Length;
                    }

                    var translatedScenario = new Scenario
                    {
                        Description = translatedOutcome_sen,
                        Outcome = translatedOutcome_sen,
                        LC_ID = translatedLcId
                    };                   

                    //Persist Scenario
                    var translatedScenarioId= _contentDataRepository.Save(translatedScenario, StateToConnectionStringMapper.ToConnectionString(prefix, state, targetLanguageCode));

                    LawCategoryToScenario translatedLawCategoryToScenario = new LawCategoryToScenario
                    {
                        LCID = lawCategoriesToScenarios[translatedLawCategory.NSMICode].LCID,
                        ScenarioId = translatedScenarioId
                    };
                    _contentDataRepository.Save(translatedLawCategoryToScenario, StateToConnectionStringMapper.ToConnectionString(prefix, state, targetLanguageCode));
                    
                }
                var currentTranslationUsage = _contentDataRepository.GetCurrentTranslationUsage(dbSourceConnectionStringName);
                currentTranslationUsage.UsedTillNow = currentTranslationUsage.UsedTillNow + numberOfTranslateCharacters;
                currentTranslationUsage.LastUpdated = numberOfTranslateCharacters;
                currentTranslationUsage.LastRunTime = DateTime.Now;
                _contentDataRepository.Update(currentTranslationUsage, dbSourceConnectionStringName);
                //_contentDataRepository.Save(new UsageTranslation { LastRunTime = DateTime.Now, LastUpdated = numberOfTranslatedWords, Limit = currentTranslationUsage.Limit, UsedTillNow = currentTranslationUsage.UsedTillNow + numberOfTranslatedWords }, dbSourceConnectionStringName);
                TotalNumberOfTranslatedCharacters += numberOfTranslateCharacters;
            }
            _logger.Information($"Total Number of Characters tranlated  for the current job is: {numberOfTranslateCharacters }. ");
             _logger.Information($"Exiting from  TranslateSentences Method. ");



        }
        public bool CountCharactersToBeTranslatedWithInLimit(int batchId, string languageCode)
        {
            _logger.Information($"Entering from  CountCharactersToBeTranslatedWithInLimit Method. ");

            var currentTranslationUsage = _contentDataRepository.GetCurrentTranslationUsage("ContentsDb_AK");
            int minLimit;
            Int32.TryParse(ConfigurationManager.AppSettings["MIN_LIMIT_TRANSLATION_REMAINING"], out minLimit);

            var maxNumberOfCharactersToBeTranslated = currentTranslationUsage.Limit - currentTranslationUsage.UsedTillNow;
            
            if ( maxNumberOfCharactersToBeTranslated <= minLimit)
            {
              _logger.Error($"Remaining number of characters to be translated reached reached(or below ) minimum limit = {minLimit}");
                return false;
            }

            var categories = _contentDataRepository
                               .GetLawCategories(batchId, dbSourceConnectionStringName);
            int currentCount = 0;
           
            numberOfTranslateCharacters = 0;
            //Parse LawCategories
            categories.ForEach((x) =>
            {
               
                numberOfTranslateCharacters += x.Description.Length + x.StateDeviation.Length;
            }

                                     
                             );
            if(numberOfTranslateCharacters > maxNumberOfCharactersToBeTranslated)
            {
                _logger.Error($"Total Number of Characters ( {numberOfTranslateCharacters} ) in Categories table exceeds the remaining  number of characters ( {maxNumberOfCharactersToBeTranslated} ) to be translated. ");
                return false;
            }
            _logger.Information($"Total Number of Characters in Categories table for the batch id = {batchId } is {numberOfTranslateCharacters - currentCount}");
            currentCount = numberOfTranslateCharacters;

            var scenarios = _contentDataRepository
                               .GetScenarios(batchId, dbSourceConnectionStringName);


            //Parse Scenario
            scenarios.ForEach((x) =>
            {

                numberOfTranslateCharacters += x.Description.Length + x.Outcome.Length;
            }
            );
                

            if(numberOfTranslateCharacters > maxNumberOfCharactersToBeTranslated)
            {
               _logger.Error($"Total Number of Characters ( {numberOfTranslateCharacters} ) in Categories and Scenatio tables exceeds the remaining  number of characters ( {maxNumberOfCharactersToBeTranslated} ) to be translated. ");
              
                return false;
            }
            _logger.Information($"Total Number of Characters in Scenario  table for the batch id = {batchId} is {numberOfTranslateCharacters - currentCount}");
            currentCount = numberOfTranslateCharacters;



            var processes = _contentDataRepository
                               .GetRelevantProcesses(batchId, dbSourceConnectionStringName);

            // Parse Process
            processes.ForEach((x) =>
            {
                numberOfTranslateCharacters += x.Title.Length;


                //  Parse ActionJson content into words
                if (!string.IsNullOrEmpty(x.ActionJson))
                {

                    numberOfTranslateCharacters += ParseActoinJson(x.ActionJson).Length;
                }
                numberOfTranslateCharacters += x.Description.Length;
            });


                  
            if (numberOfTranslateCharacters > maxNumberOfCharactersToBeTranslated)
            {
                _logger.Error($"Total Number of Characters ( {numberOfTranslateCharacters} ) in Categories,Scenario and Processes tables exceeds the remaining  number of characters ( {maxNumberOfCharactersToBeTranslated} ) to be translated. ");
            
                return false;
            }
            _logger.Information($"Total Number of Characters in Process table for the batch id = {batchId} is {numberOfTranslateCharacters - currentCount}");
            currentCount = numberOfTranslateCharacters;
            
            
            var resources = _contentDataRepository
                               .GetRelevantResources(batchId, dbSourceConnectionStringName);
            // Parse Resource
            resources.ForEach((x) => {
                numberOfTranslateCharacters += x.Title.Length;
                

                //  Parse ActionJson content into characters
                var parsedResourceJson = string.Empty;

                //ToDo: Remove hardocoded constant
                 if (x.Action == "Url") {
                    if (!string.IsNullOrEmpty(x.ResourceJson))
                    {
                        parsedResourceJson = ParseAnchor(x.ResourceJson);
                    }
                 }
                else
                {
                    parsedResourceJson = x.ResourceJson;
                }
                numberOfTranslateCharacters += parsedResourceJson.Length;
                                
            }
            );
            if (numberOfTranslateCharacters > maxNumberOfCharactersToBeTranslated)
            {
                _logger.Error($"Total Number of Characters ( {numberOfTranslateCharacters} ) in Categories,Scenario, Processes and Resources tables exceeds the remaining  number of characters ( {maxNumberOfCharactersToBeTranslated} ) to be translated. ");
                _logger.Information($"Exiting from  TranslateSentences Method. ");

                return false;
            }
            _logger.Information($"Total Number of Characters in Resource table for the batch id = {batchId} is {numberOfTranslateCharacters - currentCount}");

            _logger.Information($"Exiting from  CountCharactersToBeTranslatedWithInLimit Method. ");

            return true;
        }

        private string ParseAnchor(string actionJson)
        {
            string begRightSig = ">";
            string endSig = "</a>";

            var indxBegin = actionJson.IndexOf(begRightSig);
            actionJson = actionJson.Remove(0, indxBegin + 1);

            var indxEnd = actionJson.IndexOf(endSig);
            return  actionJson.Substring(0, indxEnd); 
        }

       

        private string ParseActoinJson(string actionJson)
        {
            string begSig = "<li>";
            string endSig = "</li>";
            string anchorStart = "<a";
            if (actionJson.Contains(begSig))
            {
                StringBuilder sb = new StringBuilder();
                while (actionJson.Contains(begSig))
                {
                    var indxBegin = actionJson.IndexOf(begSig);
                    actionJson = actionJson.Remove(0, indxBegin + begSig.Length);
                    var indxEnd = actionJson.IndexOf(endSig);
                    var anchorElement = actionJson.Substring(0, indxEnd);
                    sb.Append(ParseAnchor(anchorElement) + " ");
                    actionJson = actionJson.Remove(0, indxEnd + endSig.Length);
                }
                return sb.ToString();
            }
            else if (actionJson.TrimStart().StartsWith(anchorStart))
            {
                return ParseAnchor(actionJson);
            }
            return null;
        }

        private string TranslateAnchor(string actionJson, string targetLanguageCode)
        {
            string begRightSig = ">";
            string endSig = "</a>";

            string translatedAnchorJsonContent = string.Empty;

            var indxBegin = actionJson.IndexOf(begRightSig);
            //actionJson = actionJson.Remove(0, indxBegin + 1);

            var indxEnd = actionJson.IndexOf(endSig);
            var anchorContent = actionJson.Substring(indxBegin + begRightSig.Length, indxEnd - indxBegin - begRightSig.Length)?.Trim();
            if (!string.IsNullOrEmpty(anchorContent))
            {
                //Update total number of translated charactrers
                numberOfTranslateCharacters += actionJson.Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;

                translatedAnchorJsonContent = StripTAG(TextExtractionModule.TextTranslate(anchorContent, "en", targetLanguageCode));
                actionJson = actionJson.Remove(indxBegin + begRightSig.Length, indxEnd - indxBegin - begRightSig.Length);
                actionJson = actionJson.Insert(indxBegin + begRightSig.Length, translatedAnchorJsonContent);               
            }
            return HttpUtility.HtmlDecode(actionJson).ToString();
        }

        private static string StripTAG(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        private string TranslateActoinJson(string actionJson, string targetLanguageCode)
        {
            string begSig = "<li>";
            string endSig = "</li>";
            string anchorStart = "<a";
            int begPivotIndex = 0;
            int endPivotIndex = 0;
            if (actionJson.Contains(begSig))
            {
               
                while (actionJson.IndexOf(begSig,begPivotIndex ) != -1)
                {
                    string translatedActionJsonContent = string.Empty;
                    var indxBegin = actionJson.IndexOf(begSig, begPivotIndex);
                  
                    var indxEnd = actionJson.IndexOf(endSig,endPivotIndex);
                   
                    var actionJsonContent = actionJson.Substring(indxBegin + begSig.Length, indxEnd- indxBegin - begSig.Length)?.Trim();
                    if(!string.IsNullOrEmpty(actionJsonContent))
                    {
                        //Update total number of translated characters
                        numberOfTranslateCharacters += actionJsonContent.Split(separators, StringSplitOptions.RemoveEmptyEntries).Length;

                        translatedActionJsonContent = StripTAG(TextExtractionModule.TextTranslate(actionJsonContent, "en", targetLanguageCode));
                        actionJson = actionJson.Remove(indxBegin + begSig.Length, indxEnd - indxBegin - begSig.Length);
                        actionJson = actionJson.Insert(indxBegin + begSig.Length, translatedActionJsonContent);
                        indxEnd = actionJson.IndexOf(endSig, endPivotIndex);
                    }
                    
                    begPivotIndex = indxBegin + begSig.Length;
                    endPivotIndex = indxEnd + endSig.Length;                   
                }
                return HttpUtility.HtmlDecode(actionJson).ToString();
            }
            else if (actionJson.TrimStart().StartsWith(anchorStart))
            {
                //Simple Anchor
                return TranslateAnchor(actionJson, targetLanguageCode);
            }
            return null;
        }
    }
}
