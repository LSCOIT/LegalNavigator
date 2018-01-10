
using ContentDataAccess.StateBasedContents;
using ContentDataAccess.DataContextFactory;
using System.Collections.Generic;
using System.Linq;
using ContentDataAccess.PlatformCoreSettingContents;
using CrawledContentDataAccess.StateBasedContents;
using System;
using CrawledContentDataAccess;

namespace ContentDataAccess
{
    public class ContentDataRepository : IContentDataRepository
    {
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

        public  CuratedContent GetCuratedContent(string intent, string connectionString)
        {
            using (var curatedCntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {
                try
                {

                    var intentstoNSMICodes = curatedCntx?.IntentToNSMICodes?.Select(x => new IntentToNSMICode { Id = x.Id, Intent = x.Intent, NSMICode = x.NSMICode });
                    var intentToNSMICode_values = curatedCntx.IntentToNSMICodes.Where(intentToNsmi => intentToNsmi.Intent.ToLower().Contains(intent.ToLower())).ToList();
                    var intentToNSMICode = curatedCntx.IntentToNSMICodes.FirstOrDefault(intentToNsmi => intentToNsmi.Intent.ToLower().Contains(intent.ToLower()) || intent.ToLower().Contains(intentToNsmi.Intent.ToLower()));

                    var LawCategory = intentToNSMICode?.NSMICode != null ? curatedCntx.LawCategories.OrderBy(lawCat=> lawCat.LCID).FirstOrDefault(lawCat => lawCat.NSMICode==intentToNSMICode.NSMICode) : null;
                    var resource = LawCategory != null ? curatedCntx.Resources.Where(res => res.LawCategory.LCID == LawCategory.LCID).Select(res=> new CrawledContentDataAccess.Resource { Action= res.Action, ResourceId=res.ResourceId, ResourceJson=res.ResourceJson, ResourceType=res.ResourceType, Title=res.Title}) : null;
                    var processes = LawCategory != null ? curatedCntx.Processes.Where(prcs => prcs.LawCategory.LCID == LawCategory.LCID).OrderBy(prc=>prc.Id).Select(prc =>  new CrawledContentDataAccess.Process { ActionJson = prc.ActionJson, Description = prc.Description, Id = prc.Id, ParentId = prc.ParentId, stepType = prc.stepType, Title = prc.Title }) : null;
                    var childNodes = LawCategory != null ? curatedCntx.LawCategoryParentChildren.Where(lawCatPC => lawCatPC.ParentLawCategory != null && lawCatPC.ParentLawCategory.LCID == LawCategory.LCID).Select(lawCatPC => lawCatPC.ChildLawCategory) : null;
                    var siblings = LawCategory != null ? curatedCntx.LawCategorySiblings.Where(lawCatSib => lawCatSib.LawCategory != null && lawCatSib.LawCategory.LCID == LawCategory.LCID).Select(lawCatSib => lawCatSib.SiblingLawCategory) : null;
                    var relatedCategoriesNSMICode = childNodes.Union(siblings)?.Select(relCat => relCat.NSMICode)?.ToList();
                    var relatedIntents = relatedCategoriesNSMICode != null ? curatedCntx.IntentToNSMICodes.Where(intentToNsmi => relatedCategoriesNSMICode.Contains(intentToNsmi.NSMICode)).Select(intentToNsmi => intentToNsmi.Intent)?.ToList() : null;
                    var scenariosIds = LawCategory != null ? curatedCntx.LawCategoryToScenarios.Where(lctoscen => lctoscen.LCID == LawCategory.LCID).Select(lctoscen=> lctoscen.ScenarioId)?.ToList(): null;
                    var scenarios= scenariosIds!=null && scenariosIds.Count > 0? curatedCntx.Scenarios.Where(scen=> scenariosIds.Contains(scen.ScenarioId)).Select(scn=> new CrawledContentDataAccess.Scenario { Description= scn.Description, LC_ID= scn.LC_ID, ScenarioId=scn.ScenarioId }) : null;
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
                    var nsmiResult = new CuratedContent { Description = LawCategory?.Description, Resources = resource?.ToList(), Scenarios= scenarios?.ToList(),Processes = processList, RelatedIntents = relatedIntents };

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
        public void Save(LawCategory lawCategory, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                cntx.LawCategories.Add(lawCategory);
                cntx.SaveChanges();
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

        public void Save(CrawledContentDataAccess.StateBasedContents.Scenario scenario, string connectionString)
        {
            using (var cntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))
            {

                cntx.Scenarios.Add(scenario);
                cntx.SaveChanges();
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

        public CrawledContentDataAccess.Scenario GetScenario(int id, string connectionString)
        {
            throw new NotImplementedException();
        }

        public CuratedContent GetCuratedContent(int scenarioId, string connectionString)
        {
            using (var curatedCntx = iCrowledContentDataContextFactory.GetStateBasedContentDataContext(connectionString))

            {
                try
                {
                  
                    var scenarioObj = curatedCntx.Scenarios.FirstOrDefault(scenario => scenario.ScenarioId == scenarioId);

                    var LawCategory = scenarioObj != null ? curatedCntx.LawCategories.FirstOrDefault(lawCat => lawCat.LCID == scenarioObj.LC_ID) : null;
                    var resource = LawCategory != null ? curatedCntx.Resources.Where(res => res.LawCategory.LCID == LawCategory.LCID).Select(res=> new CrawledContentDataAccess.Resource { Action=res.Action, ResourceId=res.ResourceId, ResourceJson = res.ResourceJson, ResourceType = res.ResourceType , Title = res.Title }) : null;
                    var processes = LawCategory != null ? curatedCntx.Processes.Where(prcs => prcs.LawCategory.LCID == LawCategory.LCID).OrderBy(prc => prc.Id).Select(prc=> new CrawledContentDataAccess.Process { ActionJson=prc.ActionJson, Description= prc.Description, Id= prc.Id, ParentId= prc.ParentId, stepType = prc.stepType, Title = prc.Title }) : null;
                    var childNodes = LawCategory != null ? curatedCntx.LawCategoryParentChildren.Where(lawCatPC => lawCatPC.ParentLawCategory != null && lawCatPC.ParentLawCategory.LCID == LawCategory.LCID).Select(lawCatPC => lawCatPC.ChildLawCategory) : null;
                    var siblings = LawCategory != null ? curatedCntx.LawCategorySiblings.Where(lawCatSib => lawCatSib.LawCategory != null && lawCatSib.LawCategory.LCID == LawCategory.LCID).Select(lawCatSib => lawCatSib.SiblingLawCategory) : null;
                    var relatedCategoriesNSMICode = childNodes.Union(siblings)?.Select(relCat => relCat.NSMICode)?.ToList();
                    var relatedIntents = relatedCategoriesNSMICode != null ? curatedCntx.IntentToNSMICodes.Where(intentToNsmi => relatedCategoriesNSMICode.Contains(intentToNsmi.NSMICode)).Select(intentToNsmi => intentToNsmi.Intent)?.ToList() : null;
                    // var scenarios = LawCategory != null ? curatedCntx.Scenarios.Where(scen => scen.LawCategory != null && scen.LawCategory.LCID == LawCategory.LCID).Select(scn=> new CrawledContentDataAccess.Scenario { Description = scn.Description, LC_ID = scn.LC_ID, ScenarioId= scn.ScenarioId }) : null;
                    // var scenariosIds = LawCategory != null ? curatedCntx.LawCategoryToScenarios.Where(lctoscen => lctoscen.LCID == LawCategory.LCID).Select(lctoscen => lctoscen.ScenarioId)?.ToList() : null;
                    // var scenarios = scenariosIds != null && scenariosIds.Count > 0 ? curatedCntx.Scenarios.Where(scen => scenariosIds.Contains(scen.ScenarioId)).Select(scn => new CrawledContentDataAccess.Scenario { Description = scn.Description, LC_ID = scn.LC_ID, ScenarioId = scn.ScenarioId }) : null;
                    var scenarios = new List<CrawledContentDataAccess.Scenario>();// Since we are deriving curation  for a single scenario , we don't care about related scenarios
                    //List<CrawledContentDataAccess.Process> processList = null;
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
                    var nsmiResult = new CuratedContent { Description = LawCategory?.Description, Resources = resource?.ToList(), Scenarios = scenarios?.ToList(), Processes = processList, RelatedIntents = relatedIntents };

                    return nsmiResult;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
            return null;
        }
    }
}
