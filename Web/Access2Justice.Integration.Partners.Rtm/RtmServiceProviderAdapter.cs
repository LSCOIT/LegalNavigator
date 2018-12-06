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
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Partners.Rtm
{
    public class RtmServiceProviderAdapter : IServiceProviderAdapter
    {
        public RtmServiceProviderAdapter()
        {
        }

        public async Task<List<string>> GetServiceProviders(string topicName)
        {
            return new List<string>() { "Family Based Services", "Family Violence Prevention", "Family Support Services" };
        }

        public async Task<ServiceProvider> GetServiceProviderDetails(string providerId)
        {
            var httpClient = new HttpClientService();
            var sessionUrl = "https://www.referweb.net/pubres/api/GetSessionID/?ip={{apikey:'{61GV7G4Y}'}}";
            var sResponse = await httpClient.GetAsync(new Uri(sessionUrl)).ConfigureAwait(false);
            var session = sResponse.Content.ReadAsStringAsync().Result;

            dynamic sessionJson = JsonConvert.DeserializeObject(session);
            string sessionId = sessionJson[0][Constants.RTMSessionId];
            var serviceProvider = new ServiceProvider();
            if (!string.IsNullOrEmpty(sessionId))
            {
                string spUrl = "https://www.referweb.net/pubres/api/ServiceProviders/?ip={{apikey:'{61GV7G4Y}',st:'s',catid:'',sn:'{0}',zip:'',county:'',sid:'{1}'}}";
                var serviceProviderUrl = string.Format(CultureInfo.InvariantCulture, spUrl, providerId, sessionId);
                var response = await httpClient.GetAsync(new Uri(serviceProviderUrl)).ConfigureAwait(false);
                var serviceProviderResponse = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(serviceProviderResponse))
                {
                    var serviceProviderObject = JsonConvert.DeserializeObject(serviceProviderResponse);
                    serviceProvider = ProcessRTMData(serviceProviderObject);
                }
            }

            return serviceProvider;
        }

         // ## TODO : Need to check with Andrew on this to implement further.... 
        //private async Task<dynamic> ServiceProviderData(dynamic site, string sessionId, IHttpClientService httpClient)
        //{
        //    string siteID = site?.Sites[0]["ID"];
        //    string serviceGroupID = site?.Sites[0]["ServiceGroup"][0]["ID"];
        //    string serviceSiteID = site?.Sites[0]["ServiceGroup"][0]["ServiceSites"][0]["ID"];
        //    string serviceDetailLink = "https://www.referweb.net/pubres/api/ProviderDetail/?ip={{apikey:'{0}',locid: '{1}',svid:'{2}',ssid:'{3}',sid:'{4}'}}";
        //    string servicedetailURL = string.Format(CultureInfo.InvariantCulture, serviceDetailLink, siteID, serviceGroupID, serviceSiteID, sessionId);
        //    var response = await httpClient.GetAsync(new Uri(servicedetailURL)).ConfigureAwait(false);
        //    var spDetailResponse = response.Content.ReadAsStringAsync().Result;

        //}

        /// <summary>
        /// Converts RTM Service Provider data into Service Provider Model.
        /// </summary>
        /// <param name="serviceProviderObject">RTM serviceProvider Object</param>
        /// <returns></returns>
        private ServiceProvider ProcessRTMData(dynamic serviceProviderObject)
        {
            var site = serviceProviderObject.Sites[0];
            string siteId = site.ID.ToString();
            return ConvertToServiceProvider(site);
        }

        private dynamic ConvertToServiceProvider(dynamic site)
        {
            return new ServiceProvider()
            {
                ExternalId = site.ID,
                Email = site.Email,
                //Availability = GetAvailability(site.availability),
                AcceptanceCriteria = GetAcceptanceCriteria(site.acceptanceCriteria),
                OnboardingInfo = GetOnboardingInfo(site.onboardingInfo),
                Name = site.Name,
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
                //BusinessHours = GetBusinessHours(site.businessHours),
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
        /// return location
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
        /// retruns acceptance criteria
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
    }
}

