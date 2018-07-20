using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

 // Todo:@Alaa Remain properties after mapping is complete.


//namespace Access2Justice.Api.ViewModels
//{

//    public class CuratedExperiencePlanViewModel
//    {
//        public string id { get; set; }
//        public string type { get; set; }
//        public List<PlanTag> planTags { get; set; }
//    }

//    public class TopicTag
//    {
//        public string id { get; set; }
//    }

//    public class Location
//    {
//        public string state { get; set; }
//        public string city { get; set; }
//        public string zipCode { get; set; }
//    }

//    public class Id
//    {
//        public string id { get; set; }
//        public string name { get; set; }
//        public string description { get; set; }
//        public string resourceType { get; set; }
//        public string externalUrl { get; set; }
//        public string url { get; set; }
//        public List<TopicTag> topicTags { get; set; }
//        public List<Location> location { get; set; }
//        public string icon { get; set; }
//        public string overview { get; set; }
//        public string isRecommended { get; set; }
//        public string createdBy { get; set; }
//        public string createdTimeStamp { get; set; }
//        public string modifiedBy { get; set; }
//        public DateTime modifiedTimeStamp { get; set; }
//        public string _rid { get; set; }
//        public string _self { get; set; }
//        public string _etag { get; set; }
//        public string _attachments { get; set; }
//        public int _ts { get; set; }
//        public string type { get; set; }
//        public string address { get; set; }
//        public string telephone { get; set; }
//        public string eligibilityInformation { get; set; }
//        public string reviewedByCommunityMember { get; set; }
//        public string reviewerFullName { get; set; }
//        public string reviewerTitle { get; set; }
//        public string reviewerImage { get; set; }
//    }

//    public class ResourceTag
//    {
//        public Id id { get; set; }
//    }

//    public class StepTag
//    {
//        public string id { get; set; }
//        public string type { get; set; }
//        public string title { get; set; }
//        public string description { get; set; }
//        public List<ResourceTag> resourceTags { get; set; }
//        public string _rid { get; set; }
//        public string _self { get; set; }
//        public string _etag { get; set; }
//        public string _attachments { get; set; }
//        public int _ts { get; set; }
//        public int order { get; set; }
//        public bool markCompleted { get; set; }
//    }

//    public class PlanTag
//    {
//        public string topicId { get; set; }
//        public List<StepTag> stepTags { get; set; }
//    }



//    //schema v2
//    //public class TopicTag
//    //{
//    //    public string id { get; set; } // this what was "id", I want to remove it altoghehr
//    //}

//    //public class Location
//    //{
//    //    public string state { get; set; }
//    //    public string city { get; set; }
//    //    public string zipCode { get; set; }
//    //}

//    //public class Id
//    //{
//    //    public string id { get; set; }
//    //    public string name { get; set; }
//    //    public string description { get; set; }
//    //    public string resourceType { get; set; }
//    //    public string externalUrl { get; set; }
//    //    public string url { get; set; }
//    //    public List<TopicTag> topicTags { get; set; }
//    //    public List<Location> location { get; set; }
//    //    public string icon { get; set; }
//    //    public string overview { get; set; }
//    //    public string isRecommended { get; set; }
//    //    public string createdBy { get; set; }
//    //    public string createdTimeStamp { get; set; }
//    //    public string modifiedBy { get; set; }
//    //    public DateTime modifiedTimeStamp { get; set; }
//    //    public string _rid { get; set; }
//    //    public string _self { get; set; }
//    //    public string _etag { get; set; }
//    //    public string _attachments { get; set; }
//    //    public int _ts { get; set; }
//    //    public string type { get; set; }
//    //    public string address { get; set; }
//    //    public string telephone { get; set; }
//    //    public string eligibilityInformation { get; set; }
//    //    public string reviewedByCommunityMember { get; set; }
//    //    public string reviewerFullName { get; set; }
//    //    public string reviewerTitle { get; set; }
//    //    public string reviewerImage { get; set; }
//    //}

//    //public class ResourceTag
//    //{
//    //    public Id id { get; set; }
//    //}

//    //public class StepDetails
//    //{
//    //    public string id { get; set; }
//    //    public string type { get; set; }
//    //    public string title { get; set; }
//    //    public string description { get; set; }
//    //    public List<ResourceTag> resourceTags { get; set; }
//    //    public string _rid { get; set; }
//    //    public string _self { get; set; }
//    //    public string _etag { get; set; }
//    //    public string _attachments { get; set; }
//    //    public int _ts { get; set; }
//    //}

//    //public class StepTag
//    //{
//    //    public StepDetails stepDetails { get; set; }
//    //    public int order { get; set; }
//    //    public bool markCompleted { get; set; }
//    //}

//    //public class PlanTag
//    //{
//    //    public string topicId { get; set; }
//    //    public List<StepTag> stepTags { get; set; }
//    //}

//    //public class RootObject
//    //{
//    //    public string id { get; set; }
//    //    public string type { get; set; }
//    //    public List<PlanTag> planTags { get; set; }
//    //}












//    // schema v1
//    //public class CuratedExperiencePlanViewModel
//    //    {
//    //        public string Id { get; set; }
//    //        public string Type { get; set; }
//    //        public string Title { get; set; }
//    //        public string Description { get; set; }
//    //        public List<Guid> ResourceTags { get; set; }
//    //    }

//    //    public class StepTag
//    //    {
//    //        public CuratedExperiencePlanViewModel Id { get; set; } // what is one?
//    //        public int Order { get; set; }
//    //        public bool MarkCompleted { get; set; }
//    //    }

//    //    public class PlanTag
//    //    {
//    //        public string topicId { get; set; }
//    //        public List<StepTag> StepTags { get; set; }
//    //    }

//    //    public class RootObject
//    //    {
//    //        public string Id { get; set; }
//    //        public string Type { get; set; }
//    //        public List<PlanTag> PlanTags { get; set; }
//    //    }
//}
