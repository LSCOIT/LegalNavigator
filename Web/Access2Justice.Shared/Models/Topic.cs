using Microsoft.AspNetCore.JsonPatch.Operations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Globalization;

namespace Access2Justice.Shared.Models
{
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            //if (operation.OperationId.ToLower() == "apitopicscreatetopicuploadpost")
            if (operation.OperationId.ToLower(CultureInfo.CurrentCulture) == "apitopicscreatetopicuploadpost")
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Upload File",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }

    public class Topic
    {
        
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "parentTopicID")]
        public IEnumerable<ParentTopicID> ParentTopicID { get; set; }

        [Required]
        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "jsonContent")]
        public string JsonContent { get; set; }

        [Required]
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

    }

    public class ParentTopicID
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic ParentTopicId { get; set; }
    }

    public class Location
    {
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }
    }
}