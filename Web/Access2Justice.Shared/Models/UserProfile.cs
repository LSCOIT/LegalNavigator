using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public class UserProfile
    {
        [JsonProperty(PropertyName = "id")]
        public dynamic Id { get; set; }

        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "eMail")]
        public string EMail { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public string IsActive { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdTimeStamp")]
        public string CreatedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty(PropertyName = "modifiedTimeStamp")]
        public string ModifiedTimeStamp { get; set; }

        [JsonProperty(PropertyName = "sharedResourceId")]
        public Guid SharedResourceId { get; set; }

        [JsonProperty(PropertyName = "personalizedActionPlanId")]
        public Guid PersonalizedActionPlanId { get; set; }

        [JsonProperty(PropertyName = "curatedExperienceAnswersId")]
        public Guid CuratedExperienceAnswersId { get; set; }

        [JsonProperty(PropertyName = "savedResourcesId")]
        public Guid SavedResourcesId { get; set; }

        [JsonProperty(PropertyName = "roleInformationId")]
        public List<Guid> RoleInformationId { get; set; }

        public UserProfile()
        {
            RoleInformationId = new List<Guid>();
        }
    }

    public class UserProfileViewModel
    {
        [JsonProperty(PropertyName = "oId")]
        public string OId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "eMail")]
        public string EMail { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public string IsActive { get; set; }

        [JsonProperty(PropertyName = "sharedResourceId")]
        public Guid SharedResourceId { get; set; }

        [JsonProperty(PropertyName = "personalizedActionPlanId")]
        public Guid PersonalizedActionPlanId { get; set; }

        [JsonProperty(PropertyName = "curatedExperienceAnswersId")]
        public Guid CuratedExperienceAnswersId { get; set; }

        [JsonProperty(PropertyName = "savedResourcesId")]
        public Guid SavedResourcesId { get; set; }        

        [JsonProperty(PropertyName = "roleInformationId")]        
        public List<string> RoleInformationId { get; set; }

        [JsonProperty(PropertyName = "roleInformation")]
        public List<RoleViewModel> RoleInformation { get; set; }        

        public UserProfileViewModel()
        {
            RoleInformationId = new List<string>();
            RoleInformation = new List<RoleViewModel>();
        }
    }

}
