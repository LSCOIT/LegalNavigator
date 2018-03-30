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
        LawCategory GetLawCategory(int lcId, string connectionString);
        List<CrawledContentDataAccess.Scenario> GetScenarios(string connectionString);
        CrawledContentDataAccess.Scenario GetScenario(int id, string connectionString);
        Topic GetTopic(string name, string connectionString);
        void Save(Topic topic, string connectionString);
        void Save(Intent intent, string connectionString);
        string GetRelevantContent(string subTopic, string title, string connectionString);
        string GetRelevantContentTopDown(string intent, string title, string connectionString);
        List<RelevantTopic> GetRelevantTopicsDataAsPivot(string sentence, string connectionString);
        List<RelevantTopic> GetRelevantTopicsSentenceAsPivot(string sentence, string connectionString);
        void Save(LawCategory lawCategory, string connectionString);
        CuratedContent GetCuratedContent(string intent, string connectionString);
        CuratedContentForAScenario GetCuratedContent(int scenarioId, string connectionString);
        void Save(User user);
        void Save(PlatformCoreSettingContents.State state);
        CrawledContentDataAccess.State GetStateByName(string stateName);
        void Save(CrawledContentDataAccess.StateBasedContents.Scenario scenario, string connectionString);
    }
}
