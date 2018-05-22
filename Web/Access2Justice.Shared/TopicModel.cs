using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared
{
    public class TopicModel : CommonFields
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "parentid")]
        public string ParentId { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string keywords { get; set; }

        [JsonProperty(PropertyName = "essentialreading")]
        public string EssentialReading { get; set; }

        [JsonProperty(PropertyName = "actionplanheading")]
        public string ActionPlanHeading { get; set; }

        public IEnumerable<ActionPlan> ActionPlan { get; set; }
        public IEnumerable<Organization> Organization { get; set; }
        public IEnumerable<Video> Video { get; set; }
        public IEnumerable<Article> Article { get; set; }
        public IEnumerable<Form> Form { get; set; }

    }
    public class ActionPlan : CommonFields
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

    }
    public class Organization : CommonFields
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "alternatetitle")]
        public string AlternateTitle { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "telephone")]
        public string Telephone { get; set; }

        [JsonProperty(PropertyName = "faxnumber")]
        public string FaxNumber { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        //public CarouselBanner[] CarouselBanner { get; set; }
        //public Address[] Address { get; set; }
        //public BusinessHours[] BusinessHours { get; set; }
        //public EligibilityInformation[] EligibilityInformation { get; set; }
        //public MemberReview[] memberReview { get; set; }
    }
    public class Article : CommonFields
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        //public content[] content { get; set; }      

    }
    public class Video : CommonFields
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

    }
    public class Form : CommonFields
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
    public class CommonFields
    {
        [JsonProperty(PropertyName = "createdby")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdtimestamp")]
        public string CreatedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "modifiedby")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedtimestamp")]
        public string ModifiedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; }
    }

    public class SearchResults
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }        
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }        
    }

}