using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class Topic: Resource
    {        
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        //[Required(ErrorMessage = "Name is a required field.")]
        //[JsonProperty(PropertyName = "name")]
        //public string Name { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "parentTopicId")]
        public IEnumerable<ParentTopicId> ParentTopicId { get; set; }

        //[JsonProperty(PropertyName = "resourceType")]
        //public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }
        
        //[Required(ErrorMessage = "Organizational Unit is a required field.")]
        //[JsonProperty(PropertyName = "organizationalUnit")]
        //public string OrganizationalUnit { get; set; }

        //[JsonProperty(PropertyName = "location")]
        //public IEnumerable<Location> Location { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }        

        //[JsonProperty(PropertyName = "createdBy")]
        //public string CreatedBy { get; set; }

        //[JsonProperty(PropertyName = "createdTimeStamp")]
        //public DateTime? CreatedTimeStamp { get; set; } = DateTime.UtcNow;

        //[JsonProperty(PropertyName = "modifiedBy")]
        //public string ModifiedBy { get; set; }

        //[JsonProperty(PropertyName = "modifiedTimeStamp")]
        //public DateTime? ModifiedTimeStamp { get; set; } = DateTime.UtcNow;

        //[JsonProperty("nsmiCode")]
        //public string NsmiCode { get; set; }

        //public void Validate()
        //{

        //    ValidationContext context = new ValidationContext(this, serviceProvider: null, items: null);
        //    List<ValidationResult> results = new List<ValidationResult>();
        //    bool isValid = Validator.TryValidateObject(this, context, results, true);

        //    if (isValid == false)
        //    {
        //        StringBuilder sbrErrors = new StringBuilder();
        //        {
        //            foreach (var validationResult in results)
        //            {
        //                sbrErrors.AppendLine(validationResult.ErrorMessage);
        //            }
        //            throw new ValidationException(sbrErrors.ToString());
        //        }
        //    }
        //}
    }

    public class ParentTopicId
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ParentTopicIds { get; set; }
    }

    public class PrintableTopicView
    {
        public Topic Topic { get; set; }
        public List<Organization> Organizations { get; set; }
        public List<ActionPlan> ActionPlans { get; set; }
        public List<Article> Articles { get; set; }
        public List<Video> Videos { get; set; }
        public List<Form> Forms { get; set; }
        public List<RelatedLink> RelatedLinks { get; set; }
        public List<AdditionalReading> AdditionalReadings { get; set; }
    }
}