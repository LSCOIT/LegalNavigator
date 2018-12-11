using Access2Justice.Integration.Interfaces;
using Access2Justice.Integration.Models;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace Access2Justice.Integration.Partners.Rtm
{
    public class RtmServiceProviderAdapter : IServiceProviderAdapter
    {
        public RtmServiceProviderAdapter()
        {
        }

        public async Task<List<string>> GetServiceProviders(string topicName)
        {
            var rtmSettings = GetRTMConfiguration();
            var providerIds = new List<string>();
            if (rtmSettings != null && !string.IsNullOrEmpty(topicName))
            {
                var httpClient = new HttpClientService();
                string rtmSessionUrl = string.Format(CultureInfo.InvariantCulture, rtmSettings.SessionURL.OriginalString, rtmSettings.ApiKey);
                string sessionId = await GetRTMSession(rtmSessionUrl, httpClient).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(sessionId))
                {
                    string rtmProviderUrl = string.Format(CultureInfo.InvariantCulture, rtmSettings.ServiceProviderURL.OriginalString, rtmSettings.ApiKey, topicName, sessionId);                    
                    var serviceProviderResponse = await GetRTMProvider(rtmProviderUrl, httpClient).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(serviceProviderResponse))
                    {
                        dynamic serviceProviderObject = JsonConvert.DeserializeObject(serviceProviderResponse);
                        foreach (var site in serviceProviderObject.Sites)
                        {
                            providerIds.Add(topicName + "|" + site.Key);
                        }
                    }
                }
            }
            return providerIds;
        }

        public async Task<ServiceProvider> GetServiceProviderDetails(string providerId)
        {
            var rtmSettings = GetRTMConfiguration();
            var serviceProvider = new ServiceProvider();
            if (rtmSettings != null && !string.IsNullOrEmpty(providerId))
            {
                var providerDetail = providerId.Split("|");
                var httpClient = new HttpClientService();
                string rtmSessionUrl = string.Format(CultureInfo.InvariantCulture, rtmSettings.SessionURL.OriginalString, rtmSettings.ApiKey);
                string sessionId = await GetRTMSession(rtmSessionUrl, httpClient).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(sessionId) && providerDetail.Count() > 0)
                {
                    string rtmProviderUrl = string.Format(CultureInfo.InvariantCulture, rtmSettings.ServiceProviderURL.OriginalString, rtmSettings.ApiKey, providerDetail[0], sessionId);
                    var serviceProviderResponse = await GetRTMProvider(rtmProviderUrl, httpClient).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(serviceProviderResponse))
                    {
                        dynamic serviceProviderObject = JsonConvert.DeserializeObject(serviceProviderResponse);
                        foreach (var site in serviceProviderObject.Sites)
                        {
                            if (site.Key == providerDetail[1].Trim())
                            {
                                var spDetailsResponse = await GetRTMProviderDetails(site, sessionId, rtmSettings.ServiceProviderDetailURL.OriginalString, httpClient);
                                serviceProvider = ProcessRTMData(serviceProviderObject, spDetailsResponse);
                            }
                        }
                    }
                }
            }
            return serviceProvider;
        }

        #pragma warning disable CA1822 // Mark members as static
        private async Task<string> GetRTMSession(string rtmSessionUrl, IHttpClientService httpClient)
        {
            string sessionId = string.Empty;
            var sResponse = await httpClient.GetAsync(new Uri(rtmSessionUrl)).ConfigureAwait(false);
            if (sResponse.StatusCode == HttpStatusCode.OK)
            {
                var session = sResponse.Content.ReadAsStringAsync().Result;
                dynamic sessionJson = JsonConvert.DeserializeObject(session);
                sessionId = sessionJson[0][Constants.RTMSessionId];
            }
            return sessionId;
        }

        private async Task<dynamic> GetRTMProvider(string rtmProviderUrl, IHttpClientService httpClient)
        {
            dynamic serviceProviderResponse = null;
            var response = await httpClient.GetAsync(new Uri(rtmProviderUrl)).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                serviceProviderResponse = response.Content.ReadAsStringAsync().Result;
            }
            return serviceProviderResponse;
        }

        private async Task<dynamic> GetRTMProviderDetails(dynamic site, string sessionId,string rtmProviderDetailUrl, IHttpClientService httpClient)
        {
            dynamic spDetails = null;
            string siteID = site?.Sites[0]["ID"];
            string serviceGroupID = site?.Sites[0]["ServiceGroup"][0]["ID"];
            string serviceSiteID = site?.Sites[0]["ServiceGroup"][0]["ServiceSites"][0]["ID"];            
            string servicedetailURL = string.Format(CultureInfo.InvariantCulture, rtmProviderDetailUrl, siteID, serviceGroupID, serviceSiteID, sessionId);
            var response = await httpClient.GetAsync(new Uri(servicedetailURL)).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var spDetailResponse = response.Content.ReadAsStringAsync().Result;
                spDetails = JsonConvert.DeserializeObject(spDetailResponse);
            }
            return spDetails;
        }
        #pragma warning restore CA1822 // Mark members as static

        /// <summary>
        /// Reads RTM configuration from json file.
        /// </summary>
        /// <returns>RTM configuration</returns>
        private static RtmSettings GetRTMConfiguration()
        {
            try
            {
                RtmSettings rtmSettings = null;
                var currentDirectory = Directory.GetCurrentDirectory();
                using (StreamReader r = new StreamReader(currentDirectory + "\\RtmSettings.json"))
                {
                    string json = r.ReadToEnd();
                    rtmSettings = JsonConvert.DeserializeObject<RtmSettings>(json);                    
                }
                return rtmSettings;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }

        /// <summary>
        /// Converts RTM Service Provider data into Service Provider Model.
        /// </summary>
        /// <param name="serviceProviderObject">RTM serviceProvider Object</param>
        /// <returns></returns>
        private ServiceProvider ProcessRTMData(dynamic serviceProviderObject, dynamic spDetails)
        {
            var site = serviceProviderObject.Sites[0];
            string siteId = site.ID.ToString();
            return ConvertToServiceProvider(site, spDetails);
        }

        /// <summary>
        /// Converts RTM data to ServiceProvider Model.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="spDetails"></param>
        /// <returns></returns>
        private dynamic ConvertToServiceProvider(dynamic site, dynamic spDetails)
        {
            return new ServiceProvider()
            {
                ExternalId = site.ID,
                Email = site.Email,
                //Availability = GetAvailability(site.availability),
                AcceptanceCriteria = GetAcceptanceCriteria(site.acceptanceCriteria),
                OnboardingInfo = GetOnboardingInfo(site.onboardingInfo),
                Name = site.Name,
                Description = GetDescription(spDetails),
                ResourceCategory = site.resourceCategory,
                ResourceType = Constants.ServiceProviderResourceType,
                Url = site.URL,
                OrganizationalUnit = GetServiceProviderOrgUnit(site.Address),
                Location = GetServiceProviderLocation(site.Address),
                CreatedBy = Constants.IntegrationAPI,
                ModifiedBy = Constants.IntegrationAPI,
                Address = GetServiceProviderAddress(site.Address),
                Telephone = GetServiceProviderPhone(site.Phones),
                Overview = site.overview,
                Specialties = site.specialties,
                EligibilityInformation = site.eligibilityInformation,
                BusinessHours = GetBusinessHours(site.businessHours),
                Qualifications = site.qualifications
            };
        }

        /// <summary>
        /// returns service provider address
        /// </summary>
        /// <param name="siteAddress"></param>
        /// <returns></returns>
        private dynamic GetServiceProviderAddress(dynamic siteAddress)
        {
            var serviceProviderAddresses = string.Empty;
            foreach (var address in siteAddress)
            {
                string fullAddress = FormAddress(address.Line1.ToString()) + FormAddress(address.Line2.ToString()) +
                    FormAddress(address.Line3.ToString()) + FormAddress(address.City.ToString()) + FormAddress(address.County.ToString()) +
                    FormAddress(address.State.ToString()) + FormAddress(address.Zip.ToString());
                fullAddress = fullAddress.Remove(fullAddress.Length - 2);
                serviceProviderAddresses = serviceProviderAddresses + fullAddress;
                serviceProviderAddresses = serviceProviderAddresses + " | ";
            }
            return serviceProviderAddresses.Remove(serviceProviderAddresses.Length - 3);
        }

        /// <summary>
        /// returns formatted address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static string FormAddress(string address)
        {
            var formedAddress = string.Empty;
            if (!string.IsNullOrEmpty(address))
            {
                formedAddress = address + ", ";
            }

            return formedAddress;
        }

        /// <summary>
        /// returns location
        /// </summary>
        /// <param name="siteAddress"></param>
        /// <returns></returns>
        private dynamic GetServiceProviderLocation(dynamic siteAddress)
        {
            List<Location> locations = new List<Location>();
            Location location = new Location();
            foreach (var address in siteAddress)
            {
                location = new Location
                {
                    State = address.State.ToString(),
                    County = address.County.ToString(),
                    City = address.City.ToString(),
                    ZipCode = address.Zip.ToString()
                };
                bool alreadyExists = locations.Any(x => x.State == location.State &&
                                                   x.County == location.County && x.City == location.City &&
                                                   x.ZipCode == location.ZipCode);
                if (!alreadyExists)
                {
                    locations.Add(location);
                }
            }

            return locations;
        }

        /// <summary>
        /// returns organizational unit
        /// </summary>
        /// <param name="siteAddress"></param>
        /// <returns></returns>
        private static dynamic GetServiceProviderOrgUnit(dynamic siteAddress)
        {
            var serviceProviderOrgUnit = string.Empty;
            foreach (var address in siteAddress)
            {
                string stateName = address.State.ToString();
                if (!serviceProviderOrgUnit.Contains(stateName))
                {
                    serviceProviderOrgUnit = serviceProviderOrgUnit + stateName + " | ";
                }
            }

            return serviceProviderOrgUnit.Remove(serviceProviderOrgUnit.Length - 3);
        }

        /// <summary>
        /// returns phone numbers
        /// </summary>
        /// <param name="sitePhone"></param>
        /// <returns></returns>
        private static dynamic GetServiceProviderPhone(dynamic sitePhone)
        {
            var serviceProviderTelephones = string.Empty;
            var telephone = string.Empty;
            foreach (var phone in sitePhone)
            {
                if (phone.Type == "st")
                {
                    telephone = phone.Phone.ToString();
                }
                serviceProviderTelephones = serviceProviderTelephones + telephone;
                serviceProviderTelephones = serviceProviderTelephones + " | ";
            }

            return serviceProviderTelephones.Remove(serviceProviderTelephones.Length - 3);
        }

        /// <summary>
        /// returns availability details
        /// </summary>
        /// <param name="availabilityValues"></param>
        /// <returns></returns>
        private dynamic GetAvailability(dynamic availabilityValues)
        {
            var availability = new Availability();
            if (availabilityValues != null)
            {
                availability = new Availability
                {
                    RegularBusinessHours = availabilityValues?.regularBusinessHours?.Count > 0 ? GetBusinessHours(availabilityValues.regularBusinessHours) : null,
                    HolidayBusinessHours = availabilityValues?.holidayBusinessHours?.Count > 0 ? GetBusinessHours(availabilityValues.holidayBusinessHours) : null,
                    WaitTime = TimeSpan.Parse(availabilityValues.waitTime.ToString(), CultureInfo.InvariantCulture),
                };
            }

            return availability;
        }

        /// <summary>
        /// returns open time and close time schedule
        /// </summary>
        /// <param name="businessHours"></param>
        /// <returns></returns>
        private dynamic GetBusinessHours(dynamic businessHours)
        {
            var schedules = new List<Schedule>();
            var schedule = new Schedule();
            foreach (var businessHour in businessHours)
            {
                TimeSpan openTime = TimeSpan.Parse(businessHour.opensAt.ToString(), CultureInfo.InvariantCulture);
                TimeSpan closeTime = TimeSpan.Parse(businessHour.closesAt.ToString(), CultureInfo.InvariantCulture);
                schedule = (new Schedule { Day = businessHour.day, OpensAt = openTime, ClosesAt = closeTime });
                schedules.Add(schedule);
            }

            return schedules;
        }

        /// <summary>
        /// returns evaluated requirements
        /// </summary>
        /// <param name="evaluatedRequirements"></param>
        /// <returns></returns>
        private dynamic GetEvaluatedRequirements(dynamic evaluatedRequirements)
        {
            var expressions = new List<Expression>();
            if (evaluatedRequirements != null)
            {
                foreach (var evaluatedRequirement in evaluatedRequirements)
                {
                    Shared.Models.Integration.Condition condition = new Shared.Models.Integration.Condition
                    {
                        DisplayLabel = evaluatedRequirement.condition.displayLabel,
                        Data = evaluatedRequirement.condition.data,
                        DataType = evaluatedRequirement.condition.dataType
                    };
                    expressions.Add(new Expression
                    {
                        Condition = condition,
                        Operator = evaluatedRequirement.operatorName,
                        Variable = evaluatedRequirement.variable
                    });
                }
            }

            return expressions;
        }

        /// <summary>
        /// returns acceptance criteria
        /// </summary>
        /// <param name="acceptanceCriteriaValues"></param>
        /// <returns></returns>
        private dynamic GetAcceptanceCriteria(dynamic acceptanceCriteriaValues)
        {
            var acceptanceCriteria = new AcceptanceCriteria();
            if (acceptanceCriteriaValues != null)
            {
                acceptanceCriteria = new AcceptanceCriteria
                {
                    Description = acceptanceCriteriaValues.description.ToString(),
                    EvaluatedRequirements = GetEvaluatedRequirements(acceptanceCriteriaValues.evaluatedRequirements)
                };
            }

            return acceptanceCriteria;
        }

        /// <summary>
        /// returns onborading info
        /// </summary>
        /// <param name="onboardingInfoValues"></param>
        /// <returns></returns>
        private dynamic GetOnboardingInfo(dynamic onboardingInfoValues)
        {
            var onboardingInfo = new OnboardingInfo();
            if (onboardingInfoValues != null)
            {
                List<UserField> userField = new List<UserField>();
                foreach (var onboardingInfoValue in onboardingInfoValues.userFields)
                {
                    var name = onboardingInfoValue.name;
                    var value = onboardingInfoValue.value;
                    userField.Add(new UserField { Name = name, Value = value });
                }
                onboardingInfo = new OnboardingInfo { UserFields = userField };
            }

            return onboardingInfo;
        }

        /// <summary>
        /// returns description for resource.
        /// </summary>
        /// <param name="spDetails"></param>
        /// <returns></returns>
        private static string GetDescription(dynamic spDetails)
        {
            string description = string.Empty;
            if (spDetails?.Count > 0)
            {
                foreach (var item in spDetails)
                {
                    foreach (var detailText in item[0]?.DetailText)
                    {
                        if (detailText.Label == "SERVICE DESCRIPTION")
                        {
                            description = detailText.Text;
                            break;
                        }
                    }
                }
            }
            return description;
        }
    }
}

