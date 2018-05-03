using System;
using System.Collections.Generic;
using System.Linq;
using Access2Justice.CosmosDbService.Models;
namespace Access2Justice.CosmosDbService
{
    public class Class1
    {
        public List<TopicModel> GetTopicsList()
        {
            List<TopicModel> topics = new List<TopicModel>();
            topics.Add(new TopicModel { Id = 1, Title = "Family", Icon = "../assets/images/topics/topic11.png"});
            topics.Add(new TopicModel { Id = 2, Title = "Abuse&Harassment", Icon = "../assets/images/topics/topic12.png" });
            topics.Add(new TopicModel { Id = 3, Title = "Housing", Icon = "../assets/images/topics/topic13.png" });
            topics.Add(new TopicModel { Id = 4, Title = "PublicBenefits", Icon = "./assets/images/topics/topic14.png" });
            topics.Add(new TopicModel { Id = 5, Title = "Descrimination", Icon = "../assets/images/topics/topic15.png" });
            topics.Add(new TopicModel { Id = 6, Title = "Money&Debt", Icon = "../assets/images/topics/topic16.png" });
            topics.Add(new TopicModel { Id = 7, Title = "Individual Rights", Icon = "../assets/images/topics/topic17.png" });
            topics.Add(new TopicModel { Id = 8, Title = "Native [State] Rights", Icon = "../assets/images/topics/topic18.png" });
            topics.Add(new TopicModel { Id = 9, Title = "Seniors & Life Planning", Icon = "../assets/images/topics/topic19.png" });
            topics.Add(new TopicModel { Id = 10, Title = "Immigration", Icon = "../assets/images/topics/topic20.png" });
            topics.Add(new TopicModel { Id = 11, Title = "Government Records & Documents", Icon = "../assets/images/topics/topic21.png"});
            topics.Add(new TopicModel { Id = 12, Title = "Veterans", Icon = "../assets/images/topics/topic22.png"});
            return topics;
        }

        public SubjectModel GetContentsList(int id)
        {
            return contents().Where(x => x.Id == id).Select(x => new SubjectModel { Id = x.Id, Heading = x.Heading, Content = x.Content }).FirstOrDefault();
        }
        public List<SubjectModel> contents()
        {
            List<SubjectModel> content = new List<SubjectModel>();
            content.Add(new SubjectModel { Id = 1, Heading = "Family", Content = "Information related to Family"});
            content.Add(new SubjectModel { Id = 2, Heading = "Abuse&Harassment", Content = "Information related to Abuse&Harassment" });
            content.Add(new SubjectModel { Id = 3, Heading = "Housing", Content = "Information related to Housing" });
            content.Add(new SubjectModel { Id = 4, Heading = "PublicBenefits", Content = "Information related to PublicBenefits" });
            content.Add(new SubjectModel { Id = 5, Heading = "Discrimination", Content = "Information related to Descrimination" });
            content.Add(new SubjectModel { Id = 6, Heading = "Money&Debt", Content = "Information related to Money&Debt" });
            content.Add(new SubjectModel { Id = 7, Heading = "Individual Rights", Content = "Information related to Individual Rights" });
            content.Add(new SubjectModel { Id = 8, Heading = "Native [State] Rights", Content = "Information related to Native [State] Rights" });
            content.Add(new SubjectModel { Id = 9, Heading = "Seniors & Life Planning", Content = "Information related to Seniors & Life Planning" });
            content.Add(new SubjectModel { Id = 10, Heading = "Immigration", Content = "Information related to Immigration" });
            content.Add(new SubjectModel { Id = 11, Heading = "Government Records & Documents", Content = "Information related to Government Records & Documents" });
            content.Add(new SubjectModel { Id = 12, Heading = "Veterans", Content = "Information related to Veterans" });
            return content;
        }

    }
}
