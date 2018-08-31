﻿using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;
using System.Text;
using System.Collections;

namespace Access2Justice.Shared.Models
{
    public class Resource
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ResourceId { get; set; }

        [Required(ErrorMessage = "Name is a required field.")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is a required field.")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ResourceType is a required field.")]
        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "externalUrl")]
        public string ExternalUrls { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Urls { get; set; }

        [JsonProperty(PropertyName = "topicTags")]
        public IEnumerable<TopicTag> TopicTags { get; set; }

        //[EnsureOneElementAttribute(ErrorMessage = "At least one location is required")]
        [JsonProperty(PropertyName = "location")]
        public IEnumerable<Location> Location { get; set; }

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
        public IEnumerable<Conditions> Conditions { get; set; }
    }

    public class Conditions
    {
        [JsonProperty(PropertyName = "condition")]
        public IEnumerable<Condition> ConditionDetail { get; set; }
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

    public class Article: Resource
    {
       [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "headline1")]
        public string HeadLine1 { get; set; }

        [JsonProperty(PropertyName = "headline2")]
        public string HeadLine2 { get; set; }

        [JsonProperty(PropertyName = "headline3")]
        public string HeadLine3 { get; set; }        
    }

    public class Video: Resource
    {
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "isRecommended")]
        public string IsRecommended { get; set; }

        [JsonProperty(PropertyName = "videoUrl")]
        public string VideoUrls { get; set; }
    }

    public class Organization: Resource
    {
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

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
    }

    public class Form: Resource
    {
        [Required(ErrorMessage = "Overview is a required field.")]
        [JsonProperty(PropertyName = "overview")]
        public string Overview { get; set; }

        [JsonProperty(PropertyName = "fullDescription")]
        public string FullDescription { get; set; }                
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
}