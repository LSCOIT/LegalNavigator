using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class Resource
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ResourceId { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "resourceCategory")]
        public string ResourceCategory { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ResourceType is a required field.")]
        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "topicTags")]
        public List<TopicTag> TopicTags { get; set; }

        [Required(ErrorMessage = "Organizational Unit is a required field.")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }

        //[EnsureOneElementAttribute(ErrorMessage = "At least one location is required")]
        [JsonProperty(PropertyName = "location")]
        public List<Location> Location { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public DateTime? CreatedTimeStamp { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public DateTime? ModifiedTimeStamp { get; set; } = DateTime.UtcNow;

        public void Validate()
        {            
            ValidationContext context = new ValidationContext(this, serviceProvider: null, items: null);
            List<ValidationResult> results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, context, results, true);

            if (isValid == false)
            {
                StringBuilder sbrErrors = new StringBuilder();
                {
                    foreach (var validationResult in results)
                    {
                        sbrErrors.AppendLine(validationResult.ErrorMessage);
                    }
                    throw new ValidationException(sbrErrors.ToString());//TO DO - excpetions to be logged
                }
                //TO DO log errors
            }
        }
    }

    public class TopicTag
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic TopicTags { get; set; }
    }

    public class ActionPlan : Resource
    {
        [JsonProperty(PropertyName = "conditions")]
        public List<Conditions> Conditions { get; set; }
    }

    public class Conditions
    {
        [JsonProperty(PropertyName = "condition")]
        public List<Condition> ConditionDetail { get; set; }
    }

    public class Condition
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string ConditionDescription { get; set; }
    }

    public class EssentialReading : Resource
    {
        //for now there are no unique properties to essential reading.
    }

    public class Article : Resource
    {
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "contents")]
        public List<ArticleContent> Contents { get; set; }
    }

    public class ArticleContent
    {
        [JsonProperty(PropertyName = "headline")]
        public string Headline { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }

    public class Video: Resource
    {
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }
    }

    public class Organization: Resource
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "telephone")]
        public string Telephone { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "eligibilityInformation")]
        public string EligibilityInformation { get; set; }

        [JsonProperty(PropertyName = "reviewer")]
        public List<OrganizationReviewer> Reviewer { get; set; }

        [JsonProperty(PropertyName = "specialties")]
        public string Specialties { get; set; }

        [JsonProperty(PropertyName = "qualifications")]
        public string Qualifications { get; set; }

        [JsonProperty(PropertyName = "businessHours")]
        public string BusinessHours { get; set; }
    }


    public class OrganizationReviewer
    {
        [JsonProperty(PropertyName = "reviewerFullName")]
        public string ReviewerFullName { get; set; }

        [JsonProperty(PropertyName = "reviewerTitle")]
        public string ReviewerTitle { get; set; }

        [JsonProperty(PropertyName = "reviewText")]
        public string ReviewText { get; set; }

        [JsonProperty(PropertyName = "reviewerImage")]
        public string ReviewerImage { get; set; }
    }

    public class Form: Resource
    {
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "fullDescription")]
        public string FullDescription { get; set; }
    }

    public class RelatedLink : Resource
    {
        //for now there are no unique properties to related links
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class EnsureOneElementAttribute : ValidationAttribute
    {        
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (!(list is null))
            {
                return list.Count > 0;
            }
            return false;
        }
    }

    public class GuidedAssistant : Resource
    {
        [JsonProperty(PropertyName = "curatedExperienceId")]
        public string CuratedExperienceId { get; set; }
    }
}