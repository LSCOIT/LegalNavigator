using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Models.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Access2Justice.Api.BusinessLogic
{
    public class OnboardingInfoBusinessLogic : IOnboardingInfoBusinessLogic
    {
        private IHttpClientService httpClientService;
        private IOnboardingInfoSettings onboardingInfoSettings;
        public OnboardingInfoBusinessLogic(IHttpClientService httpClientService, IOnboardingInfoSettings onboardingInfoSettings)
        {
            this.httpClientService = httpClientService;
            this.onboardingInfoSettings = onboardingInfoSettings;
        }

        public Task<object> PostOnboardingInfo(OnboardingInfo onboardingInfo)
        {
            //Send the information based on delivery method
            return SendInformation(onboardingInfo);
        }

        public async Task<dynamic> SendInformation(OnboardingInfo onboardingInfo)
        {
            //Check organization delivery method
            switch (onboardingInfo.DeliveryMethod)
            {
                case DeliveryMethod.AvianCarrier:
                    break;

                case DeliveryMethod.Email:
                    string body = onboardingInfo.PayloadTemplate;
                    ListDictionary replacements = new ListDictionary();
                    foreach (var field in onboardingInfo.UserFields)
                    {
                        body = body.Replace("@" + field.Name + "@",field.Value.FirstOrDefault());
                    }

                    SmtpClient client = new SmtpClient(onboardingInfoSettings.HostAddress, Convert.ToInt16(onboardingInfoSettings.PortNumber,CultureInfo.InvariantCulture))
                    {
                        Credentials = new NetworkCredential(onboardingInfoSettings.UserName, onboardingInfoSettings.Password),
                        EnableSsl = false,
                    };

                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(onboardingInfoSettings.FromAddress);
                    var toAddress = Convert.ToString(onboardingInfo.DeliveryDestination, CultureInfo.InvariantCulture);
                    if (toAddress.IsValidEmailAddress()) {
                        mailMessage.To.Add(toAddress);
                    }
                    else if(onboardingInfoSettings.FallbackToAddress.IsValidEmailAddress())
                    {
                        mailMessage.To.Add(onboardingInfoSettings.FallbackToAddress);
                    }

                    mailMessage.Subject = onboardingInfoSettings.Subject;
                    mailMessage.Body = !string.IsNullOrEmpty(body) ? body : onboardingInfoSettings.FallbackBodyMessage;
                    mailMessage.IsBodyHtml = true;
                    client.Send(mailMessage);
                    break;

                case DeliveryMethod.Fax:
                    break;

                case DeliveryMethod.WebApi:
                    
                    //Generate a template from the form data
                    var updatedTemplate = UpdatePayLoadTemplate(onboardingInfo);

                    var stringContent = new StringContent(JsonConvert.SerializeObject(updatedTemplate), Encoding.UTF8, "application/json");
                    var response = await httpClientService.PostAsync(onboardingInfo.DeliveryDestination, stringContent);
                    response.EnsureSuccessStatusCode();
                    //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject(JsonConvert.DeserializeObject(content).ToString());

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

        private string GetPayloadTemplate(DeliveryMethod deliveryMethod)
        {
            switch (deliveryMethod)
            {
                case DeliveryMethod.Email:
                    return @"<html>
                            <body>
                                <h1>Hello @First Name@,</h1>
                                <p>
                                    Welcome to Organization Onboarding process. 
                                </p>
                                <ul>
                                    Here are the details submitted as part of the Onboarding process.
                                    <li>First Name: @First Name@</li>
                                    <li>Last Name: @Last Name@</li>
                                    <li>Gender: @Gender@</li>
                                    <li>Age: @Age@</li>
                                </ul>
                            </body>
                            </html>";
                case DeliveryMethod.WebApi:
                    return @"{
                            'First Name': '',
                            'Last Name': '',
                            'Age': '',
                            'Gender': '', 
                            'Name':'',
                            'DateOfBirth':'',
                            'Telephone':''
                            }";
                default:
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
                    DeliveryMethod = DeliveryMethod.Email,
                    PayloadTemplate = GetPayloadTemplate(DeliveryMethod.Email)
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
                    PayloadTemplate = GetPayloadTemplate(DeliveryMethod.WebApi)
                };
            }
            return onboardingInfo;
        }
    }
}
