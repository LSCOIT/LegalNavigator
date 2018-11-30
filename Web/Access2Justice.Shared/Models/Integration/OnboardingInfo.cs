using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models.Integration
{
    /// <summary>
    /// Represents onboarding information that service provider can accept.
    /// </summary>
    public class OnboardingInfo
    {
        public OnboardingInfo()
        {
            UserFields = new List<Field>();
        }

        /// <summary>
        /// Name / value pairs of data that end user can provide when
        /// onboarding with service provider
        /// </summary>
        /// <example>
        /// Age: 18-35
        /// Gender: Female
        /// </example>
        public IEnumerable<Field> UserFields { get; set; }

        /// <summary>
        /// Template for paylod to be delivered to service provider.
        /// </summary>
        public string PayloadTemplate { get; set; }

        /// <summary>
        /// Delivery methods that service provider supports 
        /// for accepting onboarding information.
        /// </summary>
        public DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Destination for sending onboarding info
        /// </summary>
        /// <example>
        /// http://example.com/onboarding-submission/ (web service submissions)
        /// mailto:inbox@example.com (email submissions)
        /// tel:123-456-7890 (fax submissions)
        /// </example>
        public Uri DeliveryDestination { get; set; }

    }

    public class Field
    {        
        public string Type { get; set; }

        public string Label { get; set; }
        
        public string Name { get; set; }
        
        public List<string> Value { get; set; }

        public bool IsRequired { get; set; }

        public string MinLength { get; set; }

        public string MaxLength { get; set; }
    }

    public enum DeliveryMethod
    {
        WebApi,
        Email,
        Fax,
        AvianCarrier
    }
}