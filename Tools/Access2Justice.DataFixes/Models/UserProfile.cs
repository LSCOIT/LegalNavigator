using System;
using Newtonsoft.Json;

namespace Access2Justice.DataFixes.Models
{
    public class UserProfile
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "eMail")]
        public string EMail { get; set; }

        [JsonProperty(PropertyName = "sharedResourceId")]
        public Guid SharedResourceId { get; set; }

        [JsonProperty(PropertyName = "incomingResourcesId")]
        public Guid IncomingResourcesId { get; set; }
        
        public string FullName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Name))
                {
                    return Name;
                }

                if (!string.IsNullOrWhiteSpace(FirstName) ||
                    !string.IsNullOrWhiteSpace(LastName))
                {
                    return $"{FirstName} {LastName}".Trim();
                }

                return EMail;
            }
        }
    }
}