using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Globalization;
using Access2Justice.Shared;
using Microsoft.Azure.Documents;
using Microsoft.AspNetCore.Http;

namespace Access2Justice.Api.BusinessLogic
{
    public class UserProfileBusinessLogic : IUserProfileBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public UserProfileBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<UserProfile> GetUserProfileDataAsync(string oId)
        {
            UserProfile userProfile = new UserProfile();
            var resultUserData = await dbClient.FindItemsWhereAsync(dbSettings.UserProfileCollectionId, Constants.OId, oId);
            userProfile = ConvertUserProfile(resultUserData);
            return userProfile;
        }
        public async Task<dynamic> GetUserResourceProfileDataAsync(string oId)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, Constants.OId, oId);
        }
        public async Task<int> CreateUserProfileDataAsync(UserProfile userProfile)
        {
            UserProfile resultUserProfile = await GetUserProfileDataAsync(userProfile.OId);
            if (!(resultUserProfile.OId == userProfile.OId))
            {
                var result = await dbService.CreateItemAsync(ResourceDeserialized(userProfile), dbSettings.UserProfileCollectionId);
                if (result.ToString().Contains(userProfile.OId)) return 1;
            }
            return 0;
        }

        public async Task<int> UpdateUserProfileDataAsync(UserProfile userProfile, string userIdGuid)
        {
            var resultUP = GetUserProfileDataAsync(userProfile.OId);
            var userprofileObjects = JsonConvert.SerializeObject(resultUP);
            if (userprofileObjects.Contains(userProfile.OId)) // condition to verify oId and update the details
            {
                userProfile.Id = userIdGuid; // guid id of the document
                var result = await dbService.UpdateItemAsync(userIdGuid, ResourceDeserialized(userProfile), dbSettings.UserProfileCollectionId);
                if (result.ToString().Contains(userProfile.OId)) return 1;
            }
            return 0;
        }

        private UserProfile ConvertUserProfile(dynamic convObj)
        {
            var serializedResult = JsonConvert.SerializeObject(convObj);
            List<UserProfile> listUserProfiles = JsonConvert.DeserializeObject<List<UserProfile>>(serializedResult);
            UserProfile userProfile = new UserProfile();
            foreach (UserProfile user in listUserProfiles)
            {
                userProfile.Id = user.Id;
                userProfile.OId = user.OId;
                userProfile.FirstName = user.FirstName;
                userProfile.LastName = user.LastName;
                userProfile.EMail = user.EMail;
                userProfile.IsActive = user.IsActive;
                userProfile.CreatedBy = user.CreatedBy;
                userProfile.CreatedTimeStamp = user.CreatedTimeStamp;
                userProfile.ModifiedBy = user.ModifiedBy;
                userProfile.ModifiedTimeStamp = user.ModifiedTimeStamp;
                userProfile.SharedResource = user.SharedResource;
            }
            return userProfile;
        }
        private object ResourceDeserialized(UserProfile userProfile)
        {
            var serializedResult = JsonConvert.SerializeObject(userProfile);
            return JsonConvert.DeserializeObject<object>(serializedResult);
        }
        private object ResourceDynamicDeserialized(dynamic userProfile)
        {
            var serializedResult = JsonConvert.SerializeObject(userProfile);
            return JsonConvert.DeserializeObject<object>(serializedResult);
        }
        public async Task<dynamic> UpsertUserPersonalizedPlanAsync(dynamic userData)
        {
            var serializedResult = JsonConvert.SerializeObject(userData);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userDocument.oId;
            dynamic result = null;
            if (userData.type == "plans")
            {
                string planId = userDocument.planId;
                List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
                List<string> values = new List<string>() { oId, planId };
                var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
                if (userDBData.Count == 0)
                {
                    result = CreateUserPersonalizedPlanAsync(userData);
                }
                else
                {
                    string id = userDBData[0].id;
                    result = UpdateUserPersonalizedPlanAsync(id, userData);
                }
            }
            else if (userData.type == "resources")
            {
                string type = userData.type;
                List<string> resourcesPropertyNames = new List<string>() { Constants.OId, Constants.Type };
                List<string> resourcesValues = new List<string>() { oId, type };
                var userResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, resourcesPropertyNames, resourcesValues);
                if (userResourcesDBData.Count == 0)
                {
                    result = CreateUserSavedResourcesAsync(userData);
                }
                else
                {
                    string id = userResourcesDBData[0].id;
                    result = UpdateUserSavedResourcesAsync(id, userData);
                }
            }
            return result;
        }

        public async Task<dynamic> CreateUserPersonalizedPlanAsync(dynamic userData)
        {
            var serializedResult = JsonConvert.SerializeObject(userData);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        }

        public async Task<dynamic> UpdateUserPersonalizedPlanAsync(string id, dynamic userUIData)
        {
            var serializedResult = JsonConvert.SerializeObject(userUIData);
            var userUIDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userUIDocument.oId;
            string planId = userUIDocument.planId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
            List<string> values = new List<string>() { oId, planId };
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
            var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
            JObject dbObject = JObject.Parse(serializedDBResult);
            JObject uiObject = JObject.Parse(serializedResult);

            foreach (var prop in uiObject.Properties())
            {
                var targetProperty = dbObject.Property(prop.Name);
                if (targetProperty == null)
                {
                    dbObject.Add(prop.Name, prop.Value);
                }
                else
                {
                    targetProperty.Value = prop.Value;
                }
            }
            return await dbService.UpdateItemAsync(id, dbObject, dbSettings.ResourceCollectionId);
        }

        public async Task<dynamic> CreateUserSavedResourcesAsync(dynamic userResources)
        {
            var serializedResult = JsonConvert.SerializeObject(userResources);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        }

        public async Task<dynamic> UpdateUserSavedResourcesAsync(string id, dynamic userResources)
        {
            var serializedResult = JsonConvert.SerializeObject(userResources);
            var userUIDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userUIDocument.oId;
            string type = userUIDocument.type;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.Type };
            List<string> values = new List<string>() { oId, type };
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
            var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
            JObject dbObject = JObject.Parse(serializedDBResult);
            JObject uiObject = JObject.Parse(serializedResult);

            foreach (var prop in uiObject.Properties())
            {
                var targetProperty = dbObject.Property(prop.Name);
                if (targetProperty == null)
                {
                    dbObject.Add(prop.Name, prop.Value);
                }
                else
                {
                    targetProperty.Value = prop.Value;
                }
            }
            return await dbService.UpdateItemAsync(id, dbObject, dbSettings.ResourceCollectionId);
        }

        public async Task<dynamic> UpsertUserPlanAsync(dynamic userPlan)
        {
            var serializedResult = JsonConvert.SerializeObject(userPlan);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userDocument.oId;
            dynamic result = null;
            string id = userDocument.id;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.Id };
            List<string> values = new List<string>() { oId, id };
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
            if (userDBData.Count == 0)
            {
                result = CreateUserPlanAsync(userPlan);
            }
            else
            {
                result = UpdateUserPlanAsync(id, userPlan);
            }
            return result;
        }

        public async Task<dynamic> CreateUserPlanAsync(dynamic userData)
        {
            var serializedResult = JsonConvert.SerializeObject(userData);
            var userDocument = JsonConvert.DeserializeObject(serializedResult);
            return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        }

        public async Task<dynamic> UpdateUserPlanAsync(string id, dynamic userUIData)
        {
            var serializedResult = JsonConvert.SerializeObject(userUIData);
            var userUIDocument = JsonConvert.DeserializeObject(serializedResult);
            string oId = userUIDocument.oId;
            List<string> propertyNames = new List<string>() { Constants.OId, Constants.Id };
            List<string> values = new List<string>() { oId, id };
            var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
            var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
            JObject dbObject = JObject.Parse(serializedDBResult);
            JObject uiObject = JObject.Parse(serializedResult);

            foreach (var prop in uiObject.Properties())
            {
                var targetProperty = dbObject.Property(prop.Name);
                if (targetProperty == null)
                {
                    dbObject.Add(prop.Name, prop.Value);
                }
                else
                {
                    targetProperty.Value = prop.Value;
                }
            }
            return await dbService.UpdateItemAsync(id, dbObject, dbSettings.ResourceCollectionId);
        }

        public async Task<object> ShareResourceDataAsync(ShareInput shareInput)
        {
            UserProfile userProfile = await GetUserProfileDataAsync(shareInput.UserId);
            var permaLink = Utilities.GenerateSHA256String(shareInput.UserId + shareInput.ResourceId);
            var sharedResource = new SharedResource
            {
                ExpirationDate = DateTime.UtcNow.AddYears(1),
                IsShared = true,
                Url = new Uri(shareInput.Url + "/" + shareInput.ResourceId.ToString("D", CultureInfo.InvariantCulture), UriKind.Relative),
                PermaLink = permaLink
            };
            if (userProfile.SharedResource == null)
            {
                userProfile.SharedResource = new List<SharedResource>();
            }
            userProfile.SharedResource.Add(sharedResource);
            if (shareInput.Url.OriginalString.Contains("plan"))
            {
                //ToDo - Update the IsShared flag in the personalized plan document when user share the plan 
                //PersonalizedPlanSteps plan = GetPersonalizedPlan(shareInput.ResourceId);
                //plan.IsShared = true;
                //UpdatePersonalizedPlan(plan);
            }
            var response = await UpdateUserProfileDataAsync(userProfile);
            if (response == null)
            {
                return StatusCodes.Status500InternalServerError;
            }
            return permaLink;
        }

        public async Task<object> UnshareResourceDataAsync(UnShareInput unShareInput)
        {
            UserProfile userProfile = await GetUserProfileDataAsync(unShareInput.UserId);
            var permaLink = Utilities.GenerateSHA256String(unShareInput.UserId + unShareInput.ResourceId);
            var sharedResource = userProfile.SharedResource.FindAll(a => a.PermaLink == permaLink);
            if (sharedResource.Count == 0)
            {
                return false;
            }
            userProfile.SharedResource.RemoveAll(a => a.PermaLink == permaLink);
            var response = await UpdateUserProfileDataAsync(userProfile);
            if (response == null)
            {
                return StatusCodes.Status500InternalServerError;
            }
            return true;
        }

        public async Task<object> UpdateUserProfileDataAsync(UserProfile userProfile)
        {
            return await dbService.UpdateItemAsync(userProfile.Id, ResourceDeserialized(userProfile), dbSettings.UserProfileCollectionId);
        }
    }
}