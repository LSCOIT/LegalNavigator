
using ContentDataAccess.StateBasedContents;
using ContentDataAccess.DataContextFactory;
using System.Collections.Generic;
using System.Linq;
using ContentDataAccess.PlatformCoreSettingContents;
using CrawledContentDataAccess.StateBasedContents;
using System;
using CrawledContentDataAccess;
using System.Configuration;
using System.Data.Entity;

namespace ContentDataAccess
{
    public class ContentDataRepository : IContentDataRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private string dbSourceConnectionStringName = ConfigurationManager.AppSettings["SourceConnectionString"];


        private ICrowledContentDataContextFactory iCrowledContentDataContextFactory;

        public ContentDataRepository(ICrowledContentDataContextFactory iCrowledContentDataContextFactory)
        {
            this.iCrowledContentDataContextFactory = iCrowledContentDataContextFactory;
        }
       
        public string GetRelevantContent(string subTopic, string title, string connectionString)
        {
            // using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                //Most relevant
                var relevantDocFromDeepestLevelDocument = cntx.DocumentContents.Where(doc =>
                     doc.Document.Title.ToLower().Contains(subTopic.ToLower()) && doc.Title.ToLower().Contains(title.ToLower()));
                var result = relevantDocFromDeepestLevelDocument != null ? string.Join("<br/>", relevantDocFromDeepestLevelDocument.Select(docContent => docContent.Content).Distinct()) : string.Empty;
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }

                //Second most relevant
                var relevantDocFromRemainingDeepestLevelDocument = cntx.DocumentContents.Where(doc =>
                     !doc.Document.Title.ToLower().Contains(subTopic.ToLower()) && doc.Title.ToLower().Contains(title.ToLower()));

                result = relevantDocFromRemainingDeepestLevelDocument != null ? string.Join("<br/>", relevantDocFromRemainingDeepestLevelDocument.Select(docContent => docContent.Content).Distinct()) : string.Empty;

                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }

                //Third most relevant
                var relevantDocFromSecondDeepestLevelDocument = cntx.Documents.Where(doc =>
                     doc.Title.ToLower().Contains(title.ToLower()));

                result = relevantDocFromSecondDeepestLevelDocument != null ? string.Join("<br/>", relevantDocFromSecondDeepestLevelDocument.Select(docContent => docContent.Content).Distinct()) : string.Empty;

                return result;
            }
        }
        public string GetRelevantContentTopDown(string intent, string title, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                var relevantIntentstemp = cntx.SubTopics.ToList();
                    
                //Most relevant
                var relevantIntents = cntx.SubTopics.Where(subtopic =>
                     subtopic.Name.ToLower().Contains(intent.ToLower()));
                var relevantSubtopicsWithnoChildren = cntx.Documents.Where(doc => doc.Title.ToLower().Contains(title) && !doc.HasMoreContents).Distinct();

                var relevantDocFromDeepestLevelDocument = cntx.DocumentContents.Where(doc =>
                relevantIntents.Select(relInt=>relInt.SubTopicId).Contains(doc.Document.SubTopic.SubTopicId)  && doc.Title.ToLower().Contains(title.ToLower()));

             
                var result = relevantDocFromDeepestLevelDocument != null ? string.Join("<br/>", relevantDocFromDeepestLevelDocument.Select(docContent => docContent.Content).Distinct()) : string.Empty;
               
                if (string.IsNullOrEmpty(result))
                {
                    if (relevantSubtopicsWithnoChildren != null)
                    {
                        result = string.Empty; 
                        result.Union(string.Join("<br/>", relevantSubtopicsWithnoChildren.Select(docContent => docContent.Content).Distinct()));
                        
                    }
                    
                }
                else
                {
                    return result;
                }

                //Second most relevant
                var relevantDocFromNonRelevantDeepestLevelDocument = cntx.DocumentContents.Where(doc =>
             !relevantIntents.Select(relInt => relInt.SubTopicId).Contains(doc.Document.SubTopic.SubTopicId) && doc.Title.ToLower().Contains(title.ToLower()));

              
                result = relevantDocFromNonRelevantDeepestLevelDocument != null ? string.Join("<br/>", relevantDocFromNonRelevantDeepestLevelDocument.Select(docContent => docContent.Content).Distinct()) : string.Empty;

                if (!string.IsNullOrEmpty(result))
                {
                    if (relevantSubtopicsWithnoChildren != null)
                    {
                        result.Union(string.Join("<br/>", relevantSubtopicsWithnoChildren.Select(docContent => docContent.Content).Distinct()));
                    }
                    return result;
                }

               
                return result;
            }
        }
        public List<RelevantTopic> GetRelevantTopicsSentenceAsPivot(string sentence, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                var relevantSubtopic = cntx.SubTopics.FirstOrDefault(subtopic => sentence.ToLower().Contains(subtopic.Name.ToLower()));

                var reuslt = relevantSubtopic != null ? cntx.Documents.Where(doc => doc.SubTopic.SubTopicId == relevantSubtopic.SubTopicId).Select(doc => new RelevantTopic { Name = doc.Title, Url = doc.Url }).ToList() : null;
                if(reuslt == null)
                {
                   return GetRelevantTopicsDataAsPivot(sentence, connectionString);

                }
                return reuslt;
            }           
            
        }

        public List<RelevantTopic> GetRelevantTopicsDataAsPivot(string sentence, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                var relevantSubtopic = cntx.SubTopics.FirstOrDefault(subtopic => subtopic.Name.ToLower().Contains(sentence.ToLower()));
                var result = relevantSubtopic != null ? cntx.Documents.Where(doc => doc.SubTopic.SubTopicId == relevantSubtopic.SubTopicId).Select(doc => new RelevantTopic { Name = doc.Title, Url = doc.Url }).ToList() : null;
                if (result == null)
                {
                    var relevantDocument = cntx.Documents.FirstOrDefault(doc =>
                     doc.Title.ToLower().Contains(sentence.ToLower()));
                    if (relevantDocument!= null)
                    {
                       // relevantSubtopic = cntx.SubTopics.FirstOrDefault(SubTopic => SubTopic.SubTopicId == relevantDocument.SubTopic.SubTopicId);

                        relevantSubtopic = cntx.SubTopics.FirstOrDefault(SubTopic => SubTopic.Docs.Any(doc=>doc.DocumentId == relevantDocument.DocumentId));
                       return relevantSubtopic != null ? cntx.Documents.Where(doc => doc.SubTopic.SubTopicId == relevantSubtopic.SubTopicId).Select(doc => new RelevantTopic { Name = doc.Title, Url = doc.Url }).ToList() : null;
                       
                    }
                    else
                    {
                        var relevantDocFromDeepestLevelDocument = cntx.DocumentContents.FirstOrDefault(doc =>
                                 doc.Title.ToLower().Contains(sentence.ToLower()));
                        if (relevantDocFromDeepestLevelDocument != null)
                        {
                            relevantSubtopic = cntx.SubTopics.FirstOrDefault(SubTopic => SubTopic.Docs.Any(docs => docs.DocumentContents.Any(docContent => docContent.DocumentContentId == relevantDocFromDeepestLevelDocument.DocumentContentId)));
                            return  result = relevantSubtopic != null ? cntx.Documents.Where(doc => doc.SubTopic.SubTopicId == relevantSubtopic.SubTopicId).Select(doc => new RelevantTopic { Name = doc.Title, Url = doc.Url }).ToList() : null;
                            
                        }
                    }

                }
                return result;              

           }
        }

    public Topic GetTopic(int id, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                var relevantContetnt = cntx.Topics.FirstOrDefault(topic => topic.topicId == id);


                return relevantContetnt;
            }
        }
       
        public Topic GetTopic(string name, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                var relevantContetnt = cntx.Topics.FirstOrDefault(topic => topic.Name == name);


                return relevantContetnt;
            }
        }

        public List<Topic> GetTopics(string connectionString)
        {
            
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {

                var topics = cntx.Topics?.ToList();


                return   topics ;
            }
        }

       public void Save(Topic topic, string connectionString)
        {

            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                cntx.Topics.Add(topic);
                cntx.SaveChanges();
            }
        }

        public CuratedContent GetCuratedContent(string intent, string connectionString, string translateTo)
        {
            var targetTranslationConnectionStringName = connectionString;
            IntentToNSMICode intentToNSMICode = null; ;
            using (var curatedCntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(dbSourceConnectionStringName))
            {
                try
                {

                    var intentstoNSMICodes = curatedCntx?.IntentToNSMICodes?.Select(x => new IntentToNSMICode { Id = x.Id, Intent = x.Intent, NSMICode = x.NSMICode });
                    var intentToNSMICode_values = curatedCntx.IntentToNSMICodes.Where(intentToNsmi => intentToNsmi.Intent.ToLower().Contains(intent.ToLower())).ToList();
                    intentToNSMICode = curatedCntx.IntentToNSMICodes.FirstOrDefault(intentToNsmi => intentToNsmi.Intent.ToLower().Contains(intent.ToLower()) || intent.ToLower().Contains(intentToNsmi.Intent.ToLower()));

                }
                catch (Exception ex) { Console.WriteLine(ex?.Message); }
            }
            var targetTranslationConnectionString = string.Empty;
            if (translateTo?.ToLower() == "en")
            {
                targetTranslationConnectionString = dbSourceConnectionStringName;
            }
            else
            {
                targetTranslationConnectionString = connectionString;
            }

            using (var curatedCntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(targetTranslationConnectionString))
            {
                try
                {
                    var recentBatch = curatedCntx.UploadBatchReferences.ToList().LastOrDefault();
                    LawCategory LawCategory = null;
                    if (recentBatch != null)
                    {
                         LawCategory = intentToNSMICode?.NSMICode != null ? curatedCntx.LawCategories.Where(lc => lc.Batch_Id == recentBatch.Batch_Id).OrderBy(lawCat => lawCat.LCID).FirstOrDefault(lawCat => lawCat.NSMICode == intentToNSMICode.NSMICode) : null;

                    }
                    else
                    {
                         LawCategory = intentToNSMICode?.NSMICode != null ? curatedCntx.LawCategories.OrderBy(lawCat => lawCat.LCID).FirstOrDefault(lawCat => lawCat.NSMICode == intentToNSMICode.NSMICode) : null;

                    }
                    var resource = LawCategory != null ? curatedCntx.Resources.Where(res => res.LawCategory.LCID == LawCategory.LCID && res.ResourceType != ResourceType.Related.ToString()).Select(res=> new CrawledContentDataAccess.Resource { Action= res.Action, ResourceId=res.ResourceId, ResourceJson=res.ResourceJson, ResourceType=res.ResourceType, Title=res.Title}) : null;
                    var processes = LawCategory != null ? curatedCntx.Processes.Where(prcs => prcs.LawCategory.LCID == LawCategory.LCID).OrderBy(prc=>prc.Id).Select(prc =>  new CrawledContentDataAccess.Process { ActionJson = prc.ActionJson, Description = prc.Description, Id = prc.Id, ParentId = prc.ParentId, stepType = prc.stepType, Title = prc.Title }) : null;
                    var childNodes = LawCategory != null ? curatedCntx.LawCategoryParentChildren.Where(lawCatPC => lawCatPC.ParentLawCategory != null && lawCatPC.ParentLawCategory.LCID == LawCategory.LCID).Select(lawCatPC => lawCatPC.ChildLawCategory) : null;
                    var siblings = LawCategory != null ? curatedCntx.LawCategorySiblings.Where(lawCatSib => lawCatSib.LawCategory != null && lawCatSib.LawCategory.LCID == LawCategory.LCID).Select(lawCatSib => lawCatSib.SiblingLawCategory) : null;
                    var relatedCategoriesNSMICode = siblings != null?childNodes.Union(siblings)?.Select(relCat => relCat.NSMICode)?.ToList():null;
                    var relatedIntents = relatedCategoriesNSMICode != null ? curatedCntx.IntentToNSMICodes.Where(intentToNsmi => relatedCategoriesNSMICode.Contains(intentToNsmi.NSMICode)).Select(intentToNsmi => intentToNsmi.Intent)?.ToList() : null;
                    var scenariosIds = LawCategory != null ? curatedCntx.LawCategoryToScenarios.Where(lctoscen => lctoscen.LCID == LawCategory.LCID).Select(lctoscen=> lctoscen.ScenarioId)?.ToList(): null;
                    var scenarios= scenariosIds!=null && scenariosIds.Count > 0? curatedCntx.Scenarios.Where(scen=> scenariosIds.Contains(scen.ScenarioId)).Select(scn=> new CrawledContentDataAccess.Scenario { Description= scn.Description, LC_ID= scn.LC_ID, ScenarioId=scn.ScenarioId}) : null;
                    var processList = processes?.ToList(); 
                    if (processList != null)
                    {                       
                        int index = 0;
                        string  prevousTitle="";
                        
                        foreach (var process in processList)
                        {
                            string currentTitle = process.Title;
                            if (prevousTitle != currentTitle)
                            {
                                index++;
                            }
                            prevousTitle = currentTitle;
                            process.stepNumber = index.ToString();
                            
                        }
                    }
                    var nsmiResult = new CuratedContent { Description = LawCategory?.Description,StateDeviation=LawCategory?.StateDeviation, Resources = resource?.ToList(), Scenarios= scenarios?.ToList(),Processes = processList, RelatedIntents = relatedIntents,RelatedVideos=GetVideos(intent, connectionString), QuestionsAndAnswers=GetQuestionsAndAnswers(intent,connectionString) };

                    return nsmiResult;
                }
                catch (Exception ex) { Console.WriteLine(ex?.Message); }
            }
            return null;
        }

        public void Save(Intent intent, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                cntx.Intents.Add(intent);
                cntx.SaveChanges();
            }
        }
        public int Save(LawCategory lawCategory, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                cntx.LawCategories.Add(lawCategory);
                cntx.SaveChanges();
                return lawCategory.LCID;
            }
        }

        public void Save(User user)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetPlatformCoreDataContext())
            {

                cntx.Users.Add(user);
                cntx.SaveChanges();
            }
        }

        public int Save(CrawledContentDataAccess.StateBasedContents.Scenario scenario, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                cntx.Scenarios.Add(scenario);
                cntx.SaveChanges();

                return scenario.ScenarioId;
            }
        }
       

        public List<LawCategory> GetLawCategories(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {

                var lawCategories = cntx.LawCategories?.ToList();


                return lawCategories;
            }
        }

        public List<LawCategory> GetLawCategories(int migrationId, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
               return cntx.LawCategories.Where(lc => lc.Batch_Id == migrationId)?.ToList();
            }            
        }

        public LawCategory GetLawCategory(int lcId, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {

                var lawCategory = cntx.LawCategories?.FirstOrDefault(lwCat => lwCat.LCID == lcId);


                return lawCategory;
            }
        }

        public List<CrawledContentDataAccess.Scenario> GetScenarios(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {

                var topics = cntx.Scenarios?.Select(scn=> new CrawledContentDataAccess.Scenario { Description = scn.Description, LC_ID = scn.LC_ID, ScenarioId = scn.ScenarioId }).ToList();


                return topics;
            }
        }

        public List<CrawledContentDataAccess.Scenario> GetScenarios(int migrationId, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {
                
                var lcIds = cntx.LawCategories.Where(lc => lc.Batch_Id == migrationId).Select(lc=>lc.LCID);
                if (lcIds != null && lcIds.Count() > 0)
                {
                    return cntx.Scenarios.Where(scn => lcIds.Contains( scn.LC_ID ) )?.Select(scn => new CrawledContentDataAccess.Scenario { Description = scn.Description, LC_ID = scn.LC_ID, ScenarioId = scn.ScenarioId , Outcome= scn.Outcome}).ToList();
                }

                return null;
            }
        }

        public CrawledContentDataAccess.Scenario GetScenario(int id, string connectionString)
        {
            throw new NotImplementedException();
        }

        public CuratedContentForAScenario GetCuratedContent(int scenarioId, string connectionString, string translateTo)
        {
            using (var curatedCntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {
                try
                {
                  
                    var scenarioObj = curatedCntx.Scenarios.FirstOrDefault(scenario => scenario.ScenarioId == scenarioId);
                    var recentBatch = curatedCntx.UploadBatchReferences.ToList().LastOrDefault();
                    LawCategory LawCategory = null;
                    if (recentBatch != null)
                    {
                        LawCategory = scenarioObj != null ? curatedCntx.LawCategories.FirstOrDefault(lawCat => lawCat.LCID == scenarioObj.LC_ID) : null;

                    }
                    else
                    {
                         LawCategory = scenarioObj != null ? curatedCntx.LawCategories.FirstOrDefault(lawCat => lawCat.LCID == scenarioObj.LC_ID) : null;

                    }
                    var resources = LawCategory != null ? curatedCntx.Resources.Where(res => res.LawCategory.LCID == LawCategory.LCID && res.ResourceType == ResourceType.Related.ToString() && res.Action != ResourceType.Related.ToString()).OrderBy(res=> res.ResourceId).Select(res=> new CrawledContentDataAccess.Resource { Action=res.Action, ResourceId=res.ResourceId, ResourceJson = res.ResourceJson, ResourceType = res.ResourceType , Title = res.Title }) : null;
                    var processes = LawCategory != null ? curatedCntx.Processes.Where(prcs => prcs.LawCategory.LCID == LawCategory.LCID).OrderBy(prc => prc.Id).Select(prc=> new CrawledContentDataAccess.Process { ActionJson=prc.ActionJson, Description= prc.Description, Id= prc.Id, ParentId= prc.ParentId, stepType = prc.stepType, Title = prc.Title }) : null;
                    var childNodes = LawCategory != null ? curatedCntx.LawCategoryParentChildren.Where(lawCatPC => lawCatPC.ParentLawCategory != null && lawCatPC.ParentLawCategory.LCID == LawCategory.LCID).Select(lawCatPC => lawCatPC.ChildLawCategory) : null;
                    var siblings = LawCategory != null ? curatedCntx.LawCategorySiblings.Where(lawCatSib => lawCatSib.LawCategory != null && lawCatSib.LawCategory.LCID == LawCategory.LCID).Select(lawCatSib => lawCatSib.SiblingLawCategory) : null;
                    var relatedCategoriesNSMICode = siblings != null?childNodes.Union(siblings)?.Select(relCat => relCat.NSMICode)?.ToList(): null;
                    var relatedIntents = relatedCategoriesNSMICode != null ? curatedCntx.IntentToNSMICodes.Where(intentToNsmi => relatedCategoriesNSMICode.Contains(intentToNsmi.NSMICode)).Select(intentToNsmi => intentToNsmi.Intent)?.ToList() : null;
                    // var scenarios = LawCategory != null ? curatedCntx.Scenarios.Where(scen => scen.LawCategory != null && scen.LawCategory.LCID == LawCategory.LCID).Select(scn=>   new CrawledContentDataAccess.Scenario { Description = scn.Description, LC_ID = scn.LC_ID, ScenarioId= scn.ScenarioId }) : null;
                    // var scenariosIds = LawCategory != null ? curatedCntx.LawCategoryToScenarios.Where(lctoscen => lctoscen.LCID == LawCategory.LCID).Select(lctoscen => lctoscen.ScenarioId)?.ToList() : null;
                    // var scenarios = scenariosIds != null && scenariosIds.Count > 0 ? curatedCntx.Scenarios.Where(scen => scenariosIds.Contains(scen.ScenarioId)).Select(scn => new CrawledContentDataAccess.Scenario { Description = scn.Description, LC_ID = scn.LC_ID, ScenarioId = scn.ScenarioId }) : null;
                    var scenarios = new List<CrawledContentDataAccess.Scenario>();// Since we are deriving curation  for a single scenario , we don't care about related scenarios
                    //List<CrawledContentDataAccess.Process> processList = null;
                    var currentIntent = LawCategory != null ? curatedCntx.IntentToNSMICodes.FirstOrDefault(intentToNsmi => LawCategory.NSMICode == intentToNsmi.NSMICode):null;
                    var processList = processes?.ToList();
                    if (processList != null)
                    {
                        int index = 0;
                        string prevousTitle = "";

                        foreach (var process in processList)
                        {
                            string currentTitle = process.Title;
                            if (prevousTitle != currentTitle)
                            {
                                index++;
                            }
                            prevousTitle = currentTitle;
                            process.stepNumber = index.ToString();

                        }
                    }
                    var nsmiResult = new CuratedContentForAScenario
                    { 
                        ScenarioId= scenarioObj?.ScenarioId,
                        Description = LawCategory?.Description,
                        StateDeviation = LawCategory?.StateDeviation,
                        RelatedResources = resources?.ToList(),                         
                        Outcome= scenarioObj?.Outcome,                       
                        Processes = processList,
                        RelatedIntents = relatedIntents,
                        CurrentIntent = currentIntent?.Intent,
                        RelatedVideos = GetVideos(currentIntent?.Intent, connectionString),
                        QuestionsAndAnswers = GetQuestionsAndAnswers(currentIntent?.Intent, connectionString)
                    };

                    return nsmiResult;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            return null;
        }

        public void Save(PlatformCoreSettingContents.State state)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetPlatformCoreDataContext())
            {

                cntx.States.Add(state);
                cntx.SaveChanges();
            }
        }

        public CrawledContentDataAccess.State GetStateByName(string stateName)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetPlatformCoreDataContext())
            {            
                var result= cntx.States.FirstOrDefault(state => string.Compare(state.Name, stateName, true) == 0 || string.Compare(state.ShortName, stateName, true) == 0);
                if(result != null)
                {
                    return new CrawledContentDataAccess.State { Name = result.Name, ShortName = result.ShortName };
                }
                return null;
            }
        }

        public int Save(UploadBatchReference uploadBatchReference, string connectionString)
        {           
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                cntx.UploadBatchReferences.Add(uploadBatchReference);
                cntx.SaveChanges();
                return uploadBatchReference.Batch_Id;
            }
        }

        public void Save(List<CrawledContentDataAccess.StateBasedContents.Process> processes, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                cntx.Processes.AddRange(processes);
                cntx.SaveChanges();               
            }
        }

        public void Save(List<CrawledContentDataAccess.StateBasedContents.Resource> resources, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                cntx.Resources.AddRange(resources);
                cntx.SaveChanges();
            }
        }

        public List<CrawledContentDataAccess.Process> GetRelevantProcesses(int migrationId, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {

                var lcIds = cntx.LawCategories.Where(lc => lc.Batch_Id == migrationId).Select(lc => lc.LCID);
                if (lcIds != null && lcIds.Count() > 0)
                {
                    return cntx.Processes.Where(pr => lcIds.Contains(pr.LawCategory.LCID)).Select(
                          pr => new CrawledContentDataAccess.Process
                          {
                               Id= pr.Id,
                               Title = pr.Title,
                               Description = pr.Description,
                               ActionJson = pr.ActionJson,
                               LC_ID =pr.LawCategory.LCID
                          }
                        )
                        .ToList();
                }

                return null;
            }
        }

        public List<CrawledContentDataAccess.Resource> GetRelevantResources(int migrationId, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {

                var lcIds = cntx.LawCategories.Where(lc => lc.Batch_Id == migrationId).Select(lc => lc.LCID);
                if (lcIds != null && lcIds.Count() > 0)
                {
                    return cntx.Resources.Where(rsc => lcIds.Contains(rsc.LawCategory.LCID)).Select(rsc => new CrawledContentDataAccess.Resource { ResourceId = rsc.ResourceId, ResourceType = rsc.ResourceType, ResourceJson = rsc.ResourceJson, Title = rsc.Title, Action= rsc.Action, LC_ID=rsc.LawCategory.LCID}).ToList();
                }

                return null;
            }
        }

        public bool RollbackMigration(int migrationId, string connectionString)
        {
            try
            {
                using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
                {
                    var lcIds = cntx.LawCategories.Where(lc => lc.Batch_Id == migrationId).Select(lc => lc.LCID);
                    if (lcIds != null && lcIds.Count() > 0)
                    {
                        cntx.Scenarios.RemoveRange(cntx.Scenarios.Where(rsc => lcIds.Contains(rsc.LC_ID)));
                    }

                    cntx.Resources.RemoveRange(cntx.Resources.Where(rsc => rsc.LawCategory.Batch_Id == migrationId));
                    cntx.Processes.RemoveRange(cntx.Processes.Where(pr => pr.LawCategory.Batch_Id == migrationId));

                    cntx.LawCategories.RemoveRange(cntx.LawCategories.Where(lc => lc.Batch_Id == migrationId));

                    cntx.SaveChanges();

                    return true;
                }
            }
            catch
            {
               return false;
            }
        }

        public void Save(LawCategoryToScenario lawCategoryToScenario, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                cntx.LawCategoryToScenarios.Add(lawCategoryToScenario);
                cntx.SaveChanges();
            }
        }

        public List<CrawledContentDataAccess.Video> GetVideos(string intent, string connectionString)
        {
            try
            {
                using (var curatedCntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
                {
                    var intentToNSMICode = curatedCntx.IntentToNSMICodes.FirstOrDefault(intentToNsmi => intentToNsmi.Intent.ToLower().Contains(intent.ToLower()) || intent.ToLower().Contains(intentToNsmi.Intent.ToLower()));
                    var LawCategory = intentToNSMICode?.NSMICode != null ? curatedCntx.LawCategories.OrderBy(lawCat => lawCat.LCID).FirstOrDefault(lawCat => lawCat.NSMICode == intentToNSMICode.NSMICode) : null;
                    if(LawCategory != null)
                    {
                      return  curatedCntx.Videos.Where(v => v.LCID == LawCategory.LCID).Select(v=> new CrawledContentDataAccess.Video { ActionType=v.ActionType, ResourceJson=v.ResourceJson, Title=v.Title, Url= v.Url,VideoId= v.VideoId }).ToList();
                    }
                    
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public List<CrawledContentDataAccess.QuestionsAndAnswers> GetQuestionsAndAnswers(string intent, string connectionString)
        {
            try
            {
                using (var curatedCntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
                {
                    var intentToNSMICode = curatedCntx.IntentToNSMICodes.FirstOrDefault(intentToNsmi => intentToNsmi.Intent.ToLower().Contains(intent.ToLower()) || intent.ToLower().Contains(intentToNsmi.Intent.ToLower()));
                    var LawCategory = intentToNSMICode?.NSMICode != null ? curatedCntx.LawCategories.OrderBy(lawCat => lawCat.LCID).FirstOrDefault(lawCat => lawCat.NSMICode == intentToNSMICode.NSMICode) : null;
                    if (LawCategory != null)
                    {
                        return curatedCntx.QuestionsAndAnswers.Where(q => q.NsmiCode == LawCategory.NSMICode)
                               .Select(q=> new CrawledContentDataAccess.QuestionsAndAnswers {  Question= q.Question, Answer= q.Answer, Intent= q.Intent, NsmiCode = q.NsmiCode }).ToList();
                    }

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public void Save(CrawledContentDataAccess.StateBasedContents.Video video, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                cntx.Videos.Add(video);
                cntx.SaveChanges();
            }
        }

        public void Save(CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers questionsAndAnswers, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                cntx.QuestionsAndAnswers.Add(questionsAndAnswers);
                cntx.SaveChanges();
            }
        }

        public List<LanguageSupport> GetSupportedLanguages(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                var languageSupported = cntx.LanguageSupports?.ToList();


                return languageSupported;
            }
        }

        public UsageTranslation GetCurrentTranslationUsage(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                var languageSupported = cntx.UsageTranslations.OrderByDescending(ut=> ut.Id)?.FirstOrDefault();


                return languageSupported;
            }
        }

        public void Save(UsageTranslation usageTranslation, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                cntx.UsageTranslations.Add(usageTranslation);
                cntx.SaveChanges();
            }
        }

        public int GetRecentBatchId(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                return cntx.UploadBatchReferences.OrderByDescending(batch => batch.Batch_Id).FirstOrDefault().Batch_Id;
            }
        }

        public IntentToNSMICode GetIntentToNSMICode(string nsmiCode, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                return cntx.IntentToNSMICodes.FirstOrDefault(nsmi => nsmi.NSMICode == nsmiCode);
            }
        }

        public void Save(IntentToNSMICode intentToNSMICode, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                cntx.IntentToNSMICodes.Add(intentToNSMICode);
                cntx.SaveChanges();                
            }
        }

        public List<IntentToNSMICode> GetIntentToNSMICodes(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                return cntx?.IntentToNSMICodes?.ToList();
            }
        }

        public bool IsDatabseExists(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                return cntx.Database.Exists();
            }
           
        }

        public void CreateDatabaseIfNotExists(string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                if (!cntx.Database.Exists())
                {
                    cntx.Database.Create();
                }
                
            }
        }

        public void Update(UsageTranslation usageTranslation, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                var itemTobeUpdated=cntx.UsageTranslations.FirstOrDefault(ut=> ut.Id == usageTranslation.Id);
                if(itemTobeUpdated != null)
                {
                    itemTobeUpdated.LastRunTime = usageTranslation.LastRunTime;
                    itemTobeUpdated.LastUpdated = usageTranslation.LastUpdated;
                    itemTobeUpdated.Limit = usageTranslation.Limit;
                    itemTobeUpdated.UsedTillNow = usageTranslation.UsedTillNow;
                }
                cntx.SaveChanges();
            }           
        }

        public int FindLCID(int batchId, string nsmiCode, string scenarioDesciption, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {
                var lcIdsByScenarioDescrition = cntx.Scenarios.Where(sc => sc.Description.Trim() == scenarioDesciption.Trim()).Select(scn => scn.LC_ID);

                var lcIdsByBatch = cntx.LawCategories.Where(lc => lc.Batch_Id == batchId).Select(lc=>lc.LCID).ToList();

                var lawCategory = lcIdsByScenarioDescrition.Where(lcBySd => lcIdsByBatch.Contains(lcBySd))?.ToList().FirstOrDefault(); 


                return lawCategory ?? 0;
            }
        }

        public List<CrawledContentDataAccess.StateBasedContents.Video> GetVideos(int migrationId, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {
                var lcIdsByBatch = cntx.LawCategories.Where(lc => lc.Batch_Id == migrationId).Select(lc => lc.LCID).ToList();

                var videosByBatchId = cntx.Videos.Where(v => lcIdsByBatch.Contains(v.LCID)).ToList();

                return videosByBatchId;
            }
        }

        public List<CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers> GetQuestionsAndAnswers(int batchId, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {
                return cntx.QuestionsAndAnswers.Where(qa => qa.Batch_Id != null &&  qa.Batch_Id.Value== batchId).ToList();
            }
        }


        /* public void Save(LawCategory_fr_FR lawCategories_fr_FR, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.LawCategories_fr_FR.Add(lawCategories_fr_FR);
                  cntx.SaveChanges();
              }
          }

          public void Save(LawCategory_es_MX lawCategories_es_MX, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.LawCategories_es_MX.Add(lawCategories_es_MX);
                  cntx.SaveChanges();
              }
          }

          public void Save(Scenario_fr_FR scenarios_fr_FR, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.Scenarios_fr_FR.Add(scenarios_fr_FR);
                  cntx.SaveChanges();
              }
          }

          public void Save(Scenario_es_MX scenarios_es_MX, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.Scenarios_es_MX.Add(scenarios_es_MX);
                  cntx.SaveChanges();
              }
          }

          public void Save(Process_fr_FR processes_fr_FR, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.Processes_fr_FR.Add(processes_fr_FR);
                  cntx.SaveChanges();
              }
          }

          public void Save(Process_es_MX processes_es_MX, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.Processes_es_MX.Add(processes_es_MX);
                  cntx.SaveChanges();
              }
          }

          public void Save(Resource_fr_FR resources_fr_FR, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.Resources_fr_FR.Add(resources_fr_FR);
                  cntx.SaveChanges();
              }
          }

          public void Save(Resource_es_MX resources_es_MX, string connectionString)
          {
              using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
              {
                  cntx.Resources_es_MX.Add(resources_es_MX);
                  cntx.SaveChanges();
              }
          }*/
    }
}
