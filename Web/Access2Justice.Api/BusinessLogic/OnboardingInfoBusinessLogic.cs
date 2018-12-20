using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Extensions;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class OnboardingInfoBusinessLogic : IOnboardingInfoBusinessLogic
    {
        private IHttpClientService httpClientService;
        private IOnboardingInfoSettings onboardingInfoSettings;
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings dbSettings;
        public OnboardingInfoBusinessLogic(IHttpClientService httpClientService, IOnboardingInfoSettings onboardingInfoSettings,
            IDynamicQueries dynamicQueries, ICosmosDbSettings dbSettings)
        {
            this.httpClientService = httpClientService;
            this.onboardingInfoSettings = onboardingInfoSettings;
            this.dynamicQueries = dynamicQueries;
            this.dbSettings = dbSettings;
        }

        public async Task<OnboardingInfo> GetOnboardingInfo(string organizationId)
        {
            var response = await dynamicQueries.FindItemWhereAsync<JObject>(dbSettings.ResourcesCollectionId, Constants.Id, organizationId);
            if (response == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<OnboardingInfo>(JsonConvert.SerializeObject(response.GetValue(Constants.EFormFormFields)));
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
                case DeliveryMethod.Email:
                    SendMail(onboardingInfo);
                    break;
                case DeliveryMethod.Fax:
                    break;
                case DeliveryMethod.WebApi:
                    return await PostData(onboardingInfo);
            }

            return null;
        }

        private void SendMail(OnboardingInfo onboardingInfo)
        {
            string body = onboardingInfo.PayloadTemplate;
            ListDictionary replacements = new ListDictionary();
            foreach (var field in onboardingInfo.UserFields)
            {
                body = body.Replace("@" + field.Name + "@", field.Value);
            }

            SmtpClient client = new SmtpClient(onboardingInfoSettings.HostAddress, Convert.ToInt16(onboardingInfoSettings.PortNumber, CultureInfo.InvariantCulture))
            {
                Credentials = new NetworkCredential(onboardingInfoSettings.UserName, onboardingInfoSettings.Password),
                EnableSsl = false,
            };

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(onboardingInfoSettings.FromAddress);
            var toAddress = Convert.ToString(onboardingInfo.DeliveryDestination, CultureInfo.InvariantCulture);
            if (toAddress.IsValidEmailAddress())
            {
                mailMessage.To.Add(toAddress);
            }
            else if (onboardingInfoSettings.FallbackToAddress.IsValidEmailAddress())
            {
                mailMessage.To.Add(onboardingInfoSettings.FallbackToAddress);
            }

            mailMessage.Subject = onboardingInfoSettings.Subject;
            mailMessage.Body = !string.IsNullOrEmpty(body) ? body : onboardingInfoSettings.FallbackBodyMessage;
            mailMessage.IsBodyHtml = true;
            client.Send(mailMessage);
        }

        private async Task<dynamic> PostData(OnboardingInfo onboardingInfo)
        {
            //Generate a template from the form data
            var updatedTemplate = UpdatePayLoadTemplate(onboardingInfo);
            var stringContent = new StringContent(JsonConvert.SerializeObject(updatedTemplate), Encoding.UTF8, "application/json");

            var response = await httpClientService.PostAsync(onboardingInfo.DeliveryDestination, stringContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(JsonConvert.DeserializeObject(content).ToString());
        }

        private string UpdatePayLoadTemplate(OnboardingInfo onboardingInfo)
        {
            JObject template = onboardingInfo.PayloadTemplate;

            foreach (var field in onboardingInfo.UserFields)
            {
                template.Property(field.Name).Value = field.Value;
            }

            return template.ToString();
        }
    }
}
