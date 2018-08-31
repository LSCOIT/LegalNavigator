using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Access2Justice.Tools.Models;
using System.ComponentModel.DataAnnotations;

namespace Access2Justice.Tools.Models
{
    public class Resources
    {
        public string Id { get; set; }
        public IEnumerable<Resource> ResourcesList { get; set; }
    }

    public class Resource
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ResourceId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "externalUrl")]
        public string ExternalUrls { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Urls { get; set; }

        [JsonProperty(PropertyName = "referenceTags")]
        public IEnumerable<ReferenceTag> ReferenceTags { get; set; }

        [JsonProperty(PropertyName = "location")]
        public IEnumerable<Locations> Location { get; set; }

        [JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [JsonProperty(PropertyName = "conditions")]
        public IEnumerable<Conditions> Condition { get; set; }

        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "headLine1")]
        public string HeadLine1 { get; set; }

        [JsonProperty(PropertyName = "headLine2")]
        public string HeadLine2 { get; set; }

        [JsonProperty(PropertyName = "headLine3")]
        public string HeadLine3 { get; set; }

        [JsonProperty(PropertyName = "isRecommended")]
        public string IsRecommended { get; set; }

        [JsonProperty(PropertyName = "subService")]
        public string SubService { get; set; }

        [JsonProperty(PropertyName = "street")]
        public string Street { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "telephone")]
        public string Telephone { get; set; }

        [JsonProperty(PropertyName = "eligibilityInformation")]
        public string EligibilityInformation { get; set; }

        [JsonProperty(PropertyName = "reviewedByCommunityMember")]
        public string ReviewedByCommunityMember { get; set; }

        [JsonProperty(PropertyName = "reviewerFullName")]
        public string ReviewerFullName { get; set; }

        [JsonProperty(PropertyName = "reviewerTitle")]
        public string ReviewerTitle { get; set; }

        [JsonProperty(PropertyName = "reviewerImage")]
        public string ReviewerImage { get; set; }

        [JsonProperty(PropertyName = "fullDescription")]
        public string FullDescription { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public DateTime? CreatedTimeStamp { get; set; } = DateTime.UtcNow;

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public DateTime? ModifiedTimeStamp { get; set; } = DateTime.UtcNow;
    }

    public class Locations
    {
        [Required(ErrorMessage = "Location_State is a required field.")]
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }
    }

    public class ReferenceTag
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ReferenceTags { get; set; }
    }

    public class Conditions
    {
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }
    }
}
