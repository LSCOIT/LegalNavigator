using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class UserProfileBusinessLogic : IUserProfileBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public UserProfileBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetUserProfileDataAsync(string oId, bool isProfileViewModel = false)
        {
            var resultUserData = await dbClient.FindItemsWhereAsync(dbSettings.UserProfileCollectionId, Constants.OId, oId);
            if (isProfileViewModel) {
                return ConvertUserProfileViewModel(resultUserData);
            }
            return ConvertUserProfile(resultUserData);
        }

        public async Task<dynamic> GetUserResourceProfileDataAsync(string oId, string type)
        {
            var userProfile = await GetUserProfileDataAsync(oId);
            dynamic userResourcesDBData = null;
            if (type == "resources" && userProfile?.SavedResourcesId != null && userProfile?.SavedResourcesId != Guid.Empty)
            {
                userResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(userProfile.SavedResourcesId, CultureInfo.InvariantCulture));
            }
            else if (type == "plan" && userProfile?.PersonalizedActionPlanId != null && userProfile?.PersonalizedActionPlanId != Guid.Empty)
            {
                userResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(userProfile.PersonalizedActionPlanId, CultureInfo.InvariantCulture));
            }
            return userResourcesDBData;
        }

        private UserProfile ConvertUserProfile(dynamic convObj)
        {
            List<UserProfile> userProfile = JsonUtilities.DeserializeDynamicObject<List<UserProfile>>(convObj);
            return userProfile.FirstOrDefault();
        }

        private UserProfileViewModel ConvertUserProfileViewModel(dynamic convObj)
        {
            List<UserProfileViewModel> userProfileViewModel = JsonUtilities.DeserializeDynamicObject<List<UserProfileViewModel>>(convObj);
            return userProfileViewModel.FirstOrDefault();
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
            if (userProfile?.SavedResourcesId != null && userProfile?.SavedResourcesId != Guid.Empty)
            {
                userResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(userProfile.SavedResourcesId, CultureInfo.InvariantCulture));
            }
            if (userResourcesDBData == null || userResourcesDBData?.Count == 0)
            {
                result = await CreateUserSavedResourcesAsync(userData);
                string savedResourcesId = result.Id;
                userProfile.SavedResourcesId = new Guid(savedResourcesId);
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
            return await dbService.CreateItemAsync((userDocument), dbSettings.UserResourceCollectionId);
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
            return await dbService.UpdateItemAsync(id.ToString(), userDocument, dbSettings.UserResourceCollectionId);
        }

        public async Task<UserProfileViewModel> UpsertUserProfileAsync(UserProfile userProfile)
        {
            if (userProfile == null || string.IsNullOrEmpty(userProfile?.OId))
                throw new Exception("Please login into Application");
                    
            userProfile.OId = EncryptionUtilities.GenerateSHA512String(userProfile?.OId);
            var resultUP = await GetUserProfileDataAsync(userProfile?.OId, true);
            if (string.IsNullOrEmpty(resultUP?.OId))
            {
                userProfile.RoleInformationId = await GetDefaultUserRole();
                List<dynamic> profile = new List<dynamic>();
                var result = await dbService.CreateItemAsync(userProfile, dbSettings.UserProfileCollectionId);
                profile.Add(result);
                resultUP = ConvertUserProfileViewModel(profile);
            }
            return resultUP;
        }

        public async Task<Guid> GetDefaultUserRole()
        {
            List<string> propertyNames = new List<string>() { Constants.Type, Constants.RoleName };
            List<string> values = new List<string>() { Constants.UserRole, Constants.DefaultUser };
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, propertyNames, values);
            List<UserRole> userRole = JsonUtilities.DeserializeDynamicObject<List<UserRole>>(result);
            return userRole.Count() == 0 ? Guid.Empty : userRole[0].RoleInformationId;
        }
    }
}