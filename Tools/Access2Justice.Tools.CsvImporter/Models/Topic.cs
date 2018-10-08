using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Access2Justice.Tools.Models
{
    public class Topic
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [Required(ErrorMessage = "Topic_Name is a required field.")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "quickLinks")]
        public IEnumerable<QuickLinks> QuickLinks { get; set; }

        [JsonProperty(PropertyName = "parentTopicId")]
        public IEnumerable<ParentTopicID> ParentTopicId { get; set; }
        
        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [Required(ErrorMessage = "Organizational Unit is a required field.")]
        [JsonProperty(PropertyName = "organizationalUnit")]
        public string OrganizationalUnit { get; set; }

        [JsonProperty(PropertyName = "location")]
        public IEnumerable<Locations> Location { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

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
                    throw new ValidationException(sbrErrors.ToString());
                }
            }
        }
    }

    public class ParentTopicID
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ParentTopicId { get; set; }
        //public ParentTopicID()
        //{
        //    ParentTopicId = JsonConvert.DeserializeObject("[]");
        //}
    }

    public class QuickLinks
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Urls { get; set; }
    }
}