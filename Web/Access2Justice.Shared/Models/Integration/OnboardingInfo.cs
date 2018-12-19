using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Access2Justice.Shared.Models.Integration
{
    /// <summary>
    /// Represents onboarding information that service provider can accept.
    /// </summary>
    public class OnboardingInfo
    {
        public OnboardingInfo()
        {
            UserFields = new List<UserField>();
        }

        /// <summary>
        /// Name / value pairs of data that end user can provide when
        /// onboarding with service provider
        /// </summary>
        /// <example>
        /// Age: 18-35
        /// Gender: Female
        /// </example>
        [JsonProperty(PropertyName = "userFields")]
        public IEnumerable<UserField> UserFields { get; set; }

        /// <summary>
        /// Template for paylod to be delivered to service provider.
        /// </summary>
        [JsonProperty(PropertyName = "payloadTemplate")]
        public string PayloadTemplate { get; set; }

        /// <summary>
        /// Delivery methods that service provider supports 
        /// for accepting onboarding information.
        /// </summary>
        [JsonProperty(PropertyName = "deliveryMethod")]
        public DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Destination for sending onboarding info
        /// </summary>
        /// <example>
        /// http://example.com/onboarding-submission/ (web service submissions)
        /// mailto:inbox@example.com (email submissions)
        /// tel:123-456-7890 (fax submissions)
        /// </example>
        [JsonProperty(PropertyName = "deliveryDestination")]
        public Uri DeliveryDestination { get; set; }
    }
}

public enum DeliveryMethod
{
    WebApi,
    Email,
    Fax,
    AvianCarrier
}

