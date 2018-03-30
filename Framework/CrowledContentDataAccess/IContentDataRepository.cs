using ContentDataAccess.PlatformCoreSettingContents;
using ContentDataAccess.StateBasedContents;
using CrawledContentDataAccess;
using CrawledContentDataAccess.StateBasedContents;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentDataAccess
{
    public interface IContentDataRepository
    {        
        List<Topic> GetTopics(string connectionString);
        List<LawCategory> GetLawCategories(string connectionString);
        List<LawCategory> GetLawCategories(int migrationId, string connectionString);
        LawCategory GetLawCategory(int lcId, string connectionString);
        List<CrawledContentDataAccess.Scenario> GetScenarios(string connectionString);
        List<CrawledContentDataAccess.Video> GetVideos(string intent, string connectionString);
        List<CrawledContentDataAccess.QuestionsAndAnswers> GetQuestionsAndAnswers(string intent, string connectionString);
        List<CrawledContentDataAccess.Scenario> GetScenarios(int migrationId, string connectionString);
        CrawledContentDataAccess.Scenario GetScenario(int id, string connectionString);
        Topic GetTopic(string name, string connectionString);       
        string GetRelevantContent(string subTopic, string title, string connectionString);
        string GetRelevantContentTopDown(string intent, string title, string connectionString);
        List<RelevantTopic> GetRelevantTopicsDataAsPivot(string sentence, string connectionString);
        List<RelevantTopic> GetRelevantTopicsSentenceAsPivot(string sentence, string connectionString);        
        CuratedContent GetCuratedContent(string intent, string connectionString, string translateTo);
        CuratedContentForAScenario GetCuratedContent(int scenarioId, string connectionString, string translateTo);
        CrawledContentDataAccess.State GetStateByName(string stateName);
        List<CrawledContentDataAccess.Process> GetRelevantProcesses(int migrationId, string connectionString);
        List<CrawledContentDataAccess.Resource> GetRelevantResources(int migrationId, string connectionString);
        List<CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers> GetQuestionsAndAnswers(int batchId, string connectionString);


        bool IsDatabseExists(string connectionString);
        void CreateDatabaseIfNotExists(string connectionString);
        //Localization
        List<LanguageSupport> GetSupportedLanguages(string connectionString);
        UsageTranslation GetCurrentTranslationUsage(string connectionString);
        int GetRecentBatchId(string connectionString);
        IntentToNSMICode GetIntentToNSMICode(string nsmiCode, string connectionString);
        List<IntentToNSMICode> GetIntentToNSMICodes( string connectionString);

        //Localization
        void Update(UsageTranslation usageTranslation, string connectionString);

        int FindLCID(int batchId, string nsmiCode, string scenarioDesciption, string connectionString);

        void Save(IntentToNSMICode intentToNSMICode, string connectionString);
        void Save(Topic topic, string connectionString);
        void Save(Intent intent, string connectionString);
        void Save(User user);
        void Save(PlatformCoreSettingContents.State state);
        int Save(LawCategory lawCategory, string connectionString);
        int Save(CrawledContentDataAccess.StateBasedContents.Scenario scenario, string connectionString);
        int Save(UploadBatchReference uploadBatchReference, string connectionString);
        void Save(List<CrawledContentDataAccess.StateBasedContents.Process> processes, string connectionString);
        void Save(List<CrawledContentDataAccess.StateBasedContents.Resource> resources, string connectionString);      
        void Save(LawCategoryToScenario lawCategoryToScenario, string connectionString);
        void Save(CrawledContentDataAccess.StateBasedContents.Video video, string connectionString);
        void Save(CrawledContentDataAccess.StateBasedContents.QuestionsAndAnswers questionsAndAnswers, string connectionString);

        //Localization
        void Save(UsageTranslation usageTranslation, string connectionString);

        bool RollbackMigration(int migrationId, string connectionString);
        List<CrawledContentDataAccess.StateBasedContents.Video> GetVideos(int migrationId, string connectionString);
        //Localization
        /*void Save(LawCategory_fr_FR LawCategories_fr_FR, string connectionString);
        void Save(LawCategory_es_MX LawCategories_es_MX, string connectionString);
        void Save(Scenario_fr_FR Scenarios_fr_FR, string connectionString);
        void Save(Scenario_es_MX Scenarios_es_MX, string connectionString);
        void Save(Process_fr_FR Processes_fr_FR, string connectionString);
        void Save(Process_es_MX Processes_es_MX, string connectionString);
        void Save(Resource_fr_FR Resources_fr_FR, string connectionString);
        void Save(Resource_es_MX Resources_es_MX, string connectionString);*/
    }
}
