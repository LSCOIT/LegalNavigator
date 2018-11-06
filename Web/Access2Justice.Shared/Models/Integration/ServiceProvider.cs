using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models.Integration
{

    public class ServiceProvider : Organization
    {
        public Availability Availability { get; set; }

        public AcceptanceCriteria AcceptanceCriteria { get; set; }

        public OnboardingInfo OnboardingInfo { get; set; }
    }
}