using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowledContentDataAccess
{
    public interface ICrowledContentDataRepository
    {        
        List<Topic> GetTopics(string connectionString);       
        Topic GetTopic(int id, string connectionString);
        Topic GetTopic(string name, string connectionString);
        void Save(Topic topic, string connectionString);
        string GetRelevantContent(string subTopic, string title, string connectionString);
        string GetRelevantContentTopDown(string intent, string title, string connectionString);
        List<RelevantTopic> GetRelevantTopicsDataAsPivot(string sentence, string connectionString);
        List<RelevantTopic> GetRelevantTopicsSentenceAsPivot(string sentence, string connectionString);
        
    }
}
