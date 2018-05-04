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
            topics.Add(new TopicModel { Id = 1, Title = "Family", Icon = "../assets/images/topics/topic11.png" });
            topics.Add(new TopicModel { Id = 2, Title = "Abuse and Harassment", Icon = "../assets/images/topics/topic12.png" });
            topics.Add(new TopicModel { Id = 3, Title = "Housing", Icon = "../assets/images/topics/topic13.png" });
            topics.Add(new TopicModel { Id = 4, Title = "PublicBenefits", Icon = "./assets/images/topics/topic14.png" });
            topics.Add(new TopicModel { Id = 5, Title = "Discrimination", Icon = "../assets/images/topics/topic15.png" });
            topics.Add(new TopicModel { Id = 6, Title = "MoneyDebt", Icon = "../assets/images/topics/topic16.png" });
            topics.Add(new TopicModel { Id = 7, Title = "Individual Rights", Icon = "../assets/images/topics/topic17.png" });
            topics.Add(new TopicModel { Id = 8, Title = "NativeRights", Icon = "../assets/images/topics/topic18.png" });
            topics.Add(new TopicModel { Id = 9, Title = "Seniors Life Planning", Icon = "../assets/images/topics/topic19.png" });
            topics.Add(new TopicModel { Id = 10, Title = "Immigration", Icon = "../assets/images/topics/topic20.png" });
            topics.Add(new TopicModel { Id = 11, Title = "Government Records Documents", Icon = "../assets/images/topics/topic21.png" });
            topics.Add(new TopicModel { Id = 12, Title = "Veterans", Icon = "../assets/images/topics/topic22.png" });
            return topics;
        }

        //public Overview GetContentsList(int id)
        //{
        //    return contents().Where(x => x.Id == id).Select(x => new Overview { Id = x.Id, Heading = x.Heading, Content = x.Content, Articles = x.Articles, Videos = x.Videos, Forms = x.Forms }).FirstOrDefault();
        //}
        public SubjectModel GetContentsList(string name)
        {
            return contents().Where(x => x.Heading == name).Select(x => new SubjectModel { Id = x.Id, Heading = x.Heading, Content = x.Content, Articles = x.Articles, Videos = x.Videos, Forms = x.Forms }).FirstOrDefault();
        }
        public List<SubjectModel> contents()
        {
            List<SubjectModel> content = new List<SubjectModel>();
            content.Add(new SubjectModel { Id = 1, Heading = "Family", Content = "Overview-Family", Articles = "Articles-Family", Videos = "Videos-Family", Forms = "Forms-Family" });
            content.Add(new SubjectModel { Id = 2, Heading = "Abuse and Harassment", Content = "Overview-Abuse&Harassment", Articles = "Articles-Abuse&Harassment", Videos = "Videos-Abuse&Harassment", Forms = "Forms-Abuse&Harassment" });
            content.Add(new SubjectModel { Id = 3, Heading = "Housing", Content = "Overview-Housing", Articles = "Articles-Housing", Videos = "Videos-Housing", Forms = "Forms-Housing" });
            content.Add(new SubjectModel { Id = 4, Heading = "PublicBenefits", Content = "Overview-PublicBenefits", Articles = "Articles-PublicBenefits", Videos = "Videos-PublicBenefits", Forms = "Forms-PublicBenefits" });
            content.Add(new SubjectModel { Id = 5, Heading = "Discrimination", Content = "Overview-Descrimination", Articles = "Articles-Descrimination", Videos = "Videos-Descrimination", Forms = "Forms-Descrimination" });
            content.Add(new SubjectModel { Id = 6, Heading = "MoneyDebt", Content = "Overview-Money&Debt", Articles = "Articles-Money&Debt", Videos = "Videos-Money&Debt", Forms = "Forms-Money&Debt" });
            content.Add(new SubjectModel { Id = 7, Heading = "Individual Rights", Content = "Overview-Individual Rights", Articles = "Articles-Individual Rights", Videos = "Videos-Individual Rights", Forms = "Forms-Individual Rights" });
            content.Add(new SubjectModel { Id = 8, Heading = "NativeRights", Content = "Overview-Native Rights", Articles = "Articles-Native Rights", Videos = "Videos-Native Rights", Forms = "Forms-Native Rights" });
            content.Add(new SubjectModel { Id = 9, Heading = "Seniors Life Planning", Content = "Overview-Seniors & Life Planning", Articles = "Articles-Seniors & Life Planning", Videos = "Videos-Seniors & Life Planning", Forms = "Forms-Seniors & Life Planning" });
            content.Add(new SubjectModel { Id = 10, Heading = "Immigration", Content = "Overview-Immigration", Articles = "Articles-Immigration", Videos = "Videos-Immigration", Forms = "Forms-Immigration" });
            content.Add(new SubjectModel { Id = 11, Heading = "Government Records Documents", Content = "Overview-Government Records & Documents", Articles = "Articles-Government Records & Documents", Videos = "Videos-Government Records & Documents", Forms = "Forms-Government Records & Documents" });
            content.Add(new SubjectModel { Id = 12, Heading = "Veterans", Content = "Overview-Veterans", Articles = "Articles-Veterans", Videos = "Videos-Veterans", Forms = "Forms-Veterans" });
            return content;
        }

    }
}
