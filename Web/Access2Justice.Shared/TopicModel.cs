using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared
{
    public class TopicModel
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string lang { get; set; }
        public string overview { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string essentialReading { get; set; }
        public string actionPlanHeading { get; set; }
        public actionPlan[] actionPlan { get; set; }
        public Organization[] organization { get; set; }
        public Video[] video { get; set; }
        public Article[] article { get; set; }
        public Form[] form { get; set; }
        public string createdTimeStamp { get; set; }      
        public string modifiedTimeStamp { get; set; }
    }
    public class actionPlan
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string subHead { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string content { get; set; }
        public string createdBy { get; set; }
        public string createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedTimeStamp { get; set; }
        public string status { get; set; }

    }
    public class Organization
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string alternateTitle { get; set; }
        public string subTitle { get; set; }
        public string overview { get; set; }
        public string description { get; set; }
        public string lang { get; set; }
        public string createdBy { get; set; }
        public string createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedTimeStamp { get; set; }
        public string status { get; set; }
        public string keywords { get; set; }
       // public carouselBanner[] carouselBanner { get; set; }
       // public Address[] address { get; set; }
        public string telephone { get; set; }
        public string faxNumber { get; set; }
        public string url { get; set; }
        public string email { get; set; }
       // public businessHours[] businessHours { get; set; }
     //   public eligibilityInformation[] eligibilityInformation { get; set; }
      //  public memberReview[] memberReview { get; set; }
    }
    public class article
    {

        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string subHead { get; set; }
        public string lang { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
       // public content[] content { get; set; }
        public string createdBy { get; set; }
        public string createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedTimeStamp { get; set; }
        public string status { get; set; }

    }
    public class Video
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string subHead { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string url { get; set; }
        public string createdBy { get; set; }
        public string createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedTimeStamp { get; set; }
        public string status { get; set; }

    }
    public class Form
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string subHead { get; set; }
        public string keywords { get; set; }
        public string url { get; set; }
        public string createdBy { get; set; }
        public string createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedTimeStamp { get; set; }
        public string status { get; set; }

    }
    public class Article
    {
        public string id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string subHead { get; set; }
        public string description { get; set; }
        public string keywords { get; set; }
        public string content { get; set; }
        public string createdBy { get; set; }
        public string createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedTimeStamp { get; set; }
        public string status { get; set; }

    }
}
   

