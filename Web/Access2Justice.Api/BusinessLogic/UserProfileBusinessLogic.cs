using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
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
            var userProfile = await GetUserProfileDataAsync(oId);
            dynamic userResourcesDBData = null;
            if (userProfile?.savedResourcesId != null && userProfile?.savedResourcesId != Guid.Empty)
            {
                userResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserSavedResourcesCollectionId, Constants.Id, Convert.ToString(userProfile.savedResourcesId, CultureInfo.InvariantCulture));
            }
            return userResourcesDBData;
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
                userProfile.PersonalizedActionPlanId = user.PersonalizedActionPlanId;
                userProfile.CuratedExperienceAnswersId = user.CuratedExperienceAnswersId;
                userProfile.savedResourcesId = user.savedResourcesId;
                userProfile.SharedResource = user.SharedResource;
            }
            return userProfile;
        }
        public async Task<UserProfile> UpdateUserProfilePlanIdAsync(string oId, Guid planId)
        {
            var resultUP = await GetUserProfileDataAsync(oId);
            resultUP.PersonalizedActionPlanId = planId;
            var result = await dbService.UpdateItemAsync(resultUP.Id, ResourceDeserialized(resultUP), dbSettings.UserProfileCollectionId);
            return JsonConvert.DeserializeObject<UserProfile>(JsonConvert.SerializeObject(result));
        }
        private object ResourceDeserialized(UserProfile userProfile)
        {
            var serializedResult = JsonConvert.SerializeObject(userProfile);
            return JsonConvert.DeserializeObject<object>(serializedResult);
        }
        public async Task<dynamic> UpsertUserSavedResourcesAsync(ProfileResources userData)
        {
            var userDocument = new ProfileResources();
            userDocument = JsonConvert.DeserializeObject<ProfileResources>(JsonConvert.SerializeObject(userData));
            string oId = userDocument.OId;
            dynamic result = null;
            string type = userData.Type;
            dynamic userResourcesDBData = null;
            var userProfile = await GetUserProfileDataAsync(oId);
            if (userProfile?.savedResourcesId != null && userProfile?.savedResourcesId != Guid.Empty)
            {
                userResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserSavedResourcesCollectionId, Constants.Id, Convert.ToString(userProfile.savedResourcesId, CultureInfo.InvariantCulture));
            }
            if (userResourcesDBData == null || userResourcesDBData?.Count == 0)
            {
                result = await CreateUserSavedResourcesAsync(userData);
                string savedResourcesId = result.Id;
                userProfile.savedResourcesId = new Guid(savedResourcesId);
                await dbService.UpdateItemAsync(userProfile.Id, userProfile, dbSettings.UserProfileCollectionId);
            }
            else
            {
                Guid id = Guid.Parse(userResourcesDBData[0].id);
                result = await UpdateUserSavedResourcesAsync(id, userData);
            }
            return result;
        }
        public async Task<dynamic> CreateUserSavedResourcesAsync(ProfileResources userResources)
        {
            var userDocument = new UserSavedResources()
            {
                SavedResourcesId = Guid.NewGuid(),
                Resources = BuildResources(userResources)
            };
            userDocument = JsonConvert.DeserializeObject<UserSavedResources>(JsonConvert.SerializeObject(userDocument));
            return await dbService.CreateItemAsync((userDocument), dbSettings.UserSavedResourcesCollectionId);
        }

        public List<SavedResource> BuildResources(ProfileResources userResources)
        {
            var savedResources = new List<SavedResource>();
            foreach (var resource in userResources.Resources)
            {
                savedResources.Add(new SavedResource()
                {
                    ResourceId = resource.ResourceId,
                    ResourceType = resource.ResourceType,
                    ResourceDetails = resource.ResourceDetails
                });
            }
            return savedResources;
        }

        public async Task<dynamic> UpdateUserSavedResourcesAsync(Guid id, ProfileResources userResources)
        {
            var userDocument = new UserSavedResources()
            {
                SavedResourcesId = id,
                Resources = BuildResources(userResources)
            };
            userDocument = JsonConvert.DeserializeObject<UserSavedResources>(JsonConvert.SerializeObject(userDocument));
            return await dbService.UpdateItemAsync(id.ToString(), userDocument, dbSettings.UserSavedResourcesCollectionId);
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