using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Models.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class OnboardingInfoBusinessLogic : IOnboardingInfoBusinessLogic
    {
        private IHttpClientService httpClientService;
        public OnboardingInfoBusinessLogic(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public Task<object> PostOnboardingInfo(OnboardingInfo onboardingInfo)
        {
            //Generate a template from the form data
            var updatedTemplate = UpdatePayLoadTemplate(onboardingInfo);

            //Send the information based on delivery method
            return SendInformation(onboardingInfo, updatedTemplate);
        }

        public async Task<dynamic> SendInformation(OnboardingInfo onboardingInfo, string updatedTemplate)
        {
            //Check organization delivery method
            switch (onboardingInfo.DeliveryMethod)
            {
                case DeliveryMethod.AvianCarrier:
                    break;
                case DeliveryMethod.Email:
                    break;
                case DeliveryMethod.Fax:
                    break;
                case DeliveryMethod.WebApi:
                    var stringContent = new StringContent(JsonConvert.SerializeObject(updatedTemplate), Encoding.UTF8, "application/json");
                    var response = await httpClientService.PostAsync(onboardingInfo.DeliveryDestination, stringContent);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                default:
                    //If delivery method not available in form data, need to fetch the details explicitly from the organization
                    getOrganizationDeliveryMethod();
                    break;
            }
            return null;
        }

        public string UpdatePayLoadTemplate(OnboardingInfo onboardingInfo)
        {
            JObject template = JObject.Parse(onboardingInfo.PayloadTemplate);

            foreach (var field in onboardingInfo.UserFields)
            {
                template.Property(field.Name).Value = field.Value.FirstOrDefault();
            }

            return template.ToString();
        }

        private DeliveryMethod getOrganizationDeliveryMethod()
        {
            //ToDo - Need to fetch the delivery method details from the backend database based on the organization name.
            return DeliveryMethod.WebApi;
        }

        private string GetPayloadTemplate()
        {
            return @"{
                'First Name': '',
                'Last Name': '',
                'Age': '',
                'Gender': '',
                'Name':'',
                'DateOfBirth':'',
                'Telephone':''
            }";

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
                    DeliveryDestination = new Uri("http://localhost:4200//api/OnboardingInfo/submission"),
                    DeliveryMethod = DeliveryMethod.WebApi,
                    PayloadTemplate = GetPayloadTemplate()
                };
            }
            return onboardingInfo;
        }
    }
}
