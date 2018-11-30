using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Models.Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class OnboardingInfoBusinessLogic : IOnboardingInfoBusinessLogic
    {
        public OnboardingInfoBusinessLogic()
        {

        }

        public OnboardingInfo GetOnboardingInfo(string organizationType)
        {
            var onboardingInfo = new OnboardingInfo();

            if (organizationType.ToUpperInvariant() == "ORG1")
            {
                var fields = new List<Field>
                {
                    new Field()
                    {
                        Type = "TextBox",
                        Label = "First Name",
                        Name = "First Name",
                        Value = new List<string>(),
                        IsRequired = true,
                        MinLength = "3",
                        MaxLength ="20"
                    },
                    new Field()
                    {
                        Type = "TextBox",
                        Label = "Last Name",
                        Name = "Last Name",
                        Value = new List<string>(),
                        IsRequired = true,
                        MinLength = "3",
                        MaxLength ="20"
                    },
                    new Field()
                    {
                        Type = "ListBox",
                        Label = "Gender",
                        Name = "Gender",
                        Value = new List<string>(){ "Male","Female","Others"},
                        IsRequired = true
                    }
                };

                onboardingInfo = new OnboardingInfo()
                {
                    UserFields = fields,
                    DeliveryDestination = new Uri("http://example.com/onboarding-submission/"),
                    DeliveryMethod = DeliveryMethod.AvianCarrier
                };
            }
            else if (organizationType.ToUpperInvariant() == "ORG2")
            {
                var fields = new List<Field>
                {
                    new Field()
                    {
                        Type = "TextBox",
                        Label = "First Name",
                        Name = "First Name",
                        Value =new List<string>(),
                        IsRequired = true,
                        MinLength = "3",
                        MaxLength ="20"
                    },
                    new Field()
                    {
                        Type = "TextBox",
                        Label = "Last Name",
                        Name = "Last Name",
                        Value = new List<string>(),
                        IsRequired = true,
                        MinLength = "3",
                        MaxLength ="20"
                    },
                    new Field()
                    {
                        Type = "ListBox",
                        Label = "Age",
                        Name = "Age",
                        Value = new List<string>(),
                        IsRequired = true
                    }
                };

                onboardingInfo = new OnboardingInfo()
                {
                    UserFields = fields,
                    DeliveryDestination = new Uri("http://example.com/onboarding-submission/"),
                    DeliveryMethod = DeliveryMethod.AvianCarrier
                };
            }
            else
            {
                var fields = new List<Field>
                {
                    new Field()
                    {
                        Type = "TextBox",
                        Label = "Name",
                        Name = "Name",
                        Value =new List<string>(),
                        IsRequired = true,
                        MinLength = "3",
                        MaxLength ="20"
                    },
                    new Field()
                    {
                        Type = "Calendar",
                        Label = "DateOfBirth",
                        Name = "DateOfBirth",
                        Value = new List<string>(),
                        IsRequired = true
                    },
                    new Field()
                    {
                        Type = "TextBox",
                        Label = "Telephone",
                        Name = "Telephone",
                        Value = new List<string>(),
                        IsRequired = true,
                        MinLength = "10",
                        MaxLength ="10"
                    }
                };

                onboardingInfo = new OnboardingInfo()
                {
                    UserFields = fields,
                    DeliveryDestination = new Uri("http://example.com/onboarding-submission/"),
                    DeliveryMethod = DeliveryMethod.AvianCarrier
                };
            }
            return onboardingInfo;
        }
    }
}
