
using ContentDataAccess.StateBasedContents;
using ContentDataAccess.DataContextFactory;
using System.Collections.Generic;
using System.Linq;
using CrawledContentDataAccess.CuratedExperienceContents;
using ContentDataAccess.PlatformCoreSettingContents;

namespace ContentDataAccess
{
    public class CrowledContentDataRepository : ICrowledContentDataRepository
    {
        private ICrowledContentDataContextFactory iCrowledContentDataRepository;

        public CrowledContentDataRepository(ICrowledContentDataContextFactory iCrowledContentDataRepository)
        {
            this.iCrowledContentDataRepository = iCrowledContentDataRepository;
        }
       
        public string GetRelevantContent(string subTopic, string title, string connectionString)
        {
            // using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))
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
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))
            {
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
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))
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
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))
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
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))
            {

                var relevantContetnt = cntx.Topics.FirstOrDefault(topic => topic.topicId == id);


                return relevantContetnt;
            }
        }
       
        public Topic GetTopic(string name, string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))
            {

                var relevantContetnt = cntx.Topics.FirstOrDefault(topic => topic.Name == name);


                return relevantContetnt;
            }
        }

        public List<Topic> GetTopics(string connectionString)
        {
            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))

            {

                var topics = cntx.Topics?.ToList();


                return   topics ;
            }
        }

       public void Save(Topic topic, string connectionString)
        {

            //using (CrowledContentDataContext cntx = new CrowledContentDataContext())
            using (CrowledContentDataContextBase cntx = iCrowledContentDataRepository.GetCrowledContentDataContext(connectionString))
            {

                cntx.Topics.Add(topic);
                cntx.SaveChanges();
            }
        }

        public NSMIContent GetNSMIContent(string nsmiCode, string connectionString)
        {
            using (CuratedExperienceDataContext curatedCntx = iCrowledContentDataRepository.GetCuratedExperienceDataContext())

            {
                using (PlatformCoreDataContext plaformCntx = iCrowledContentDataRepository.GetPlatformCoreDataContext())
                {

                    var LawCategory = curatedCntx.LawCategories.FirstOrDefault(lawCat => nsmiCode.ToLower().Contains(lawCat.NSMICode.ToLower()));
                    var resource = LawCategory != null ? curatedCntx.Resources.Where(res => res.LawCategory.LCID == LawCategory.LCID) : null;
                    var process = LawCategory != null ? curatedCntx.Processes.Where(prcs => prcs.LawCategory.LCID == LawCategory.LCID) : null;
                    var nsmiResult = new NSMIContent { Description = LawCategory?.Description, Resources = resource?.ToList(), Processes = process?.ToList() };

                    return nsmiResult;
                }
            }
        }
    }
}
