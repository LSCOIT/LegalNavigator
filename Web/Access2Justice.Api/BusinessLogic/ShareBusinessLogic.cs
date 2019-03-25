using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class ShareBusinessLogic : IShareBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IShareSettings dbShareSettings;
        private readonly IUserProfileBusinessLogic dbUserProfile;
        private readonly IPersonalizedPlanBusinessLogic dbPersonalizedPlan;

        public ShareBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService, IShareSettings shareSettings,
            IUserProfileBusinessLogic userProfileBusinessLogic, IPersonalizedPlanBusinessLogic personalizedPlanBusinessLogic)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            dbShareSettings = shareSettings;
            dbUserProfile = userProfileBusinessLogic;
            dbPersonalizedPlan = personalizedPlanBusinessLogic;
        }

        public async Task<ShareViewModel> CheckPermaLinkDataAsync(ShareInput shareInput)
        {
            dynamic userSharedResourcesDBData = null;
            List<SharedResources> userSharedResources = new List<SharedResources>();
            if (shareInput.UserId == null || shareInput.Url == null)
            {
                return null;
            }
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(shareInput.UserId);
            if (userProfile == null)
            {
                return null;
            }
            else
            {
                if (userProfile?.SharedResourceId != null && userProfile.SharedResourceId != Guid.Empty)
                {
                    userSharedResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourcesCollectionId, Constants.Id, Convert.ToString(userProfile.SharedResourceId, CultureInfo.InvariantCulture));
                }
                if (userSharedResourcesDBData != null && userSharedResourcesDBData?.Count > 0)
                {
                    userSharedResources = JsonUtilities.DeserializeDynamicObject<List<SharedResources>>(userSharedResourcesDBData);
                }
                else
                {
                    return null;
                }
            }
            var resource = userSharedResources[0].SharedResource.FindAll(a => a.Url.OriginalString.
        Contains(shareInput.Url.OriginalString)
        && DateTime.Compare(a.ExpirationDate, DateTime.UtcNow) >= 0);

            return resource.Count == 0 ? null : new ShareViewModel
            {
                PermaLink = GetPermaLink(resource.Select(a => a.PermaLink).First())
            };
        }

        public async Task<ShareViewModel> ShareResourceDataAsync(ShareInput shareInput)
        {
            dynamic response = null;
            if (shareInput.Url == null || shareInput.UserId == null || shareInput.ResourceId == null)
            {
                return null;
            }
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(shareInput.UserId);

            shareInput.UniqueId = shareInput.UniqueId != Guid.Empty ? shareInput.UniqueId : Guid.NewGuid();
            shareInput.ResourceId = shareInput.ResourceId != Guid.Empty ? shareInput.ResourceId : Guid.NewGuid();

            var permaLink = EncryptionUtilities.GenerateSHA256String(shareInput.UniqueId + shareInput.UserId +
                shareInput.ResourceId);
            var sharedResource = new SharedResource
            {
                ExpirationDate = DateTime.UtcNow.AddYears(Constants.ExpirationDateDurationInYears),
                IsShared = true,
                Url = new Uri(shareInput.Url.OriginalString, UriKind.RelativeOrAbsolute),
                PermaLink = permaLink
            };
            response = await UpsertSharedResource(userProfile, sharedResource);
            if (shareInput.Url.OriginalString.Contains("plan"))
            {
                string planId = shareInput.Url.OriginalString.Substring(6);
                await UpdatePlanIsSharedStatus(planId, true);
            }

            return response == null ? null : new ShareViewModel
            {
                PermaLink = dbShareSettings.PermaLinkMaxLength > 0 ? GetPermaLink(permaLink) : permaLink
            };
        }

        public async Task<dynamic> ShareResourceAsync(SendLinkInput sendLinkInput)
        {
            dynamic response = null;

            if (string.IsNullOrWhiteSpace(sendLinkInput.UserId) ||
                string.IsNullOrWhiteSpace(sendLinkInput.Email))
            {
                return null;
            }

            UserProfile senderUserProfile = await dbUserProfile.GetUserProfileDataAsync(sendLinkInput.UserId);
            var recipientUserProfile = await dbUserProfile.GetUserProfileByEmailAsync(sendLinkInput.Email);

            if (senderUserProfile == null ||
                recipientUserProfile == null)
            {
                return null;
            }

            var incomingResource = new IncomingResource
            {
                ResourceId = sendLinkInput.ResourceId,
                ResourceDetails = sendLinkInput.ResourceDetails,
                ResourceType = sendLinkInput.ResourceType,
                SharedBy = senderUserProfile.FullName
            };

            response = await UpsertIncomingResourceAsync(recipientUserProfile, incomingResource);

            return response;
        }

        public async Task<object> UpsertSharedResource(UserProfile userProfile, SharedResource sharedResource)
        {
            List<SharedResource> sharedResources = new List<SharedResource>();
            dynamic userSharedResourcesDBData = null;
            dynamic response = null;
            if (userProfile?.SharedResourceId != null && userProfile.SharedResourceId != Guid.Empty)
            {
                userSharedResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourcesCollectionId, Constants.Id, Convert.ToString(userProfile.SharedResourceId, CultureInfo.InvariantCulture));
            }
            if (userSharedResourcesDBData != null && userSharedResourcesDBData.Count > 0)
            {
                var userSharedResources = new List<SharedResources>();
                userSharedResources = JsonUtilities.DeserializeDynamicObject<List<SharedResources>>(userSharedResourcesDBData);
                userSharedResources[0].SharedResourceId = userProfile.SharedResourceId;
                userSharedResources[0].SharedResource.Add(sharedResource);
                response = await dbService.UpdateItemAsync(userProfile.SharedResourceId.ToString(), userSharedResources[0],
                dbSettings.UserResourcesCollectionId);
            }
            else
            {
                var userSharedResources = new SharedResources();
                if (userSharedResourcesDBData != null)
                {
                    userSharedResources.SharedResourceId = userProfile.SharedResourceId;
                }
                else
                {
                    userSharedResources.SharedResourceId = Guid.NewGuid();
                }
                sharedResources.Add(sharedResource);
                userSharedResources.SharedResource = sharedResources;
                userProfile.SharedResourceId = userSharedResources.SharedResourceId;
                await dbService.UpdateItemAsync(userProfile.Id, userProfile,
                dbSettings.ProfilesCollectionId);
                response = await dbService.CreateItemAsync((userSharedResources), dbSettings.UserResourcesCollectionId);
            }
            return response;
        }

        public async Task<object> UpsertIncomingResourceAsync(
            UserProfile userProfile,
            IncomingResource resource)
        {
            var incomingResources = new List<IncomingResource>();
            dynamic userIncomingResourcesDBData = null;
            dynamic response = null;
            if (userProfile?.IncomingResourcesId != null &&
                userProfile.IncomingResourcesId != Guid.Empty)
            {
                userIncomingResourcesDBData =
                    await dbClient.FindItemsWhereAsync(
                        dbSettings.UserResourcesCollectionId,
                        Constants.Id,
                        Convert.ToString(userProfile.IncomingResourcesId, CultureInfo.InvariantCulture));
            }

            if (userIncomingResourcesDBData != null &&
                userIncomingResourcesDBData.Count > 0)
            {
                List<UserIncomingResources> userIncomingResources = 
                    JsonUtilities.DeserializeDynamicObject<List<UserIncomingResources>>(userIncomingResourcesDBData);

                userIncomingResources[0].IncomingResourcesId = userProfile.IncomingResourcesId;

                if (!userIncomingResources[0].Resources.Any(
                    r =>
                        r.ResourceId == resource.ResourceId &&
                        r.ResourceType == resource.ResourceType &&
                        r.SharedBy == resource.SharedBy))
                {
                    userIncomingResources[0].Resources.Add(resource);
                }

                response = await dbService.UpdateItemAsync(
                    userProfile.IncomingResourcesId.ToString(),
                    userIncomingResources[0],
                    dbSettings.UserResourcesCollectionId);
            }
            else
            {
                var userIncomingResources = new UserIncomingResources();
                if (userIncomingResourcesDBData != null)
                {
                    userIncomingResources.IncomingResourcesId = userProfile.SharedResourceId;
                }
                else
                {
                    userIncomingResources.IncomingResourcesId = Guid.NewGuid();
                }
                incomingResources.Add(resource);
                userIncomingResources.Resources = incomingResources;
                userProfile.IncomingResourcesId = userIncomingResources.IncomingResourcesId;

                await dbService.UpdateItemAsync(
                    userProfile.Id,
                    userProfile,
                    dbSettings.ProfilesCollectionId);

                response = await dbService.CreateItemAsync(
                    userIncomingResources,
                    dbSettings.UserResourcesCollectionId);
            }
            return response;
        }

        public async Task UpdatePlanIsSharedStatus(string planId, bool isShared)
        {
            var plan = await dbPersonalizedPlan.GetPersonalizedPlanAsync(Guid.Parse(planId));
            if (plan != null && plan.PersonalizedPlanId != Guid.Empty)
            {
                plan.IsShared = isShared;
                await dbService.UpdateItemAsync(planId, plan, dbSettings.ActionPlansCollectionId);
            }
        }

        public async Task<object> UnshareResourceDataAsync(ShareInput unShareInput)
        {
            dynamic userSharedResourcesDBData = null;
            var userSharedResources = new List<SharedResources>();
            if (unShareInput.UserId == null || unShareInput.ResourceId == null || unShareInput.Url == null)
            {
                return null;
            }
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(unShareInput.UserId);
            if (userProfile == null || userProfile?.SharedResourceId == null)
            {
                return null;
            }
            if (userProfile?.SharedResourceId != null && userProfile.SharedResourceId != Guid.Empty)
            {
                userSharedResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourcesCollectionId, Constants.Id, Convert.ToString(userProfile.SharedResourceId, CultureInfo.InvariantCulture));
            }
            if (userSharedResourcesDBData != null)
            {
                userSharedResources = JsonUtilities.DeserializeDynamicObject<List<SharedResources>>(userSharedResourcesDBData);
            }
            var sharedResource = userSharedResources[0].SharedResource.FindAll(a => a.Url.OriginalString.
            Contains(unShareInput.Url.OriginalString));
            if (sharedResource.Count == 0)
            {
                return false;
            }
            userSharedResources[0].SharedResource.RemoveAll(a => a.Url.OriginalString.
            Contains(unShareInput.Url.OriginalString));
            var response = await dbService.UpdateItemAsync(userSharedResources[0].SharedResourceId.ToString(), userSharedResources[0],
                dbSettings.UserResourcesCollectionId);
            if (unShareInput.Url.OriginalString.Contains("plan"))
            {
                string planId = unShareInput.Url.OriginalString.Substring(6);
                await UpdatePlanIsSharedStatus(planId, false);
            }
            return response == null ? false : true;
        }

        public async Task<object> GetPermaLinkDataAsync(string permaLink)
        {
            if (string.IsNullOrEmpty(permaLink))
            {
                return null;
            }
            var response = await dbClient.FindFieldWhereArrayContainsAsync(dbSettings.UserResourcesCollectionId,
                Constants.SharedResource, Constants.PermaLink, permaLink, Constants.ExpirationDate);
            if (response == null)
            {
                return null;
            }
            List<ShareProfileDetails> shareProfileDetails = JsonUtilities.DeserializeDynamicObject<List<ShareProfileDetails>>(response);
            ShareProfileViewModel profileViewModel = new ShareProfileViewModel();

            if (shareProfileDetails.Count() > 0)
            {
                var userprofileResponse = await dbClient.FindFieldWhereArrayContainsAsync(dbSettings.ProfilesCollectionId, Constants.sharedResourceId, shareProfileDetails[0].Id);
                List<ShareProfileResponse> shareProfileResponse = JsonUtilities.DeserializeDynamicObject<List<ShareProfileResponse>>(userprofileResponse);
                if (shareProfileResponse?.Count > 0)
                {
                    shareProfileResponse[0].Link = shareProfileDetails.FirstOrDefault().Link;
                    foreach (var profile in shareProfileResponse)
                    {
                        if (profile.Link == Constants.ProfileLink)
                        {
                            profileViewModel.UserName = profile.Name;
                            profileViewModel.UserId = profile.OId;
                        }
                        profileViewModel.ResourceLink = profile.Link;
                    }
                }
            }
            return profileViewModel;
        }

        private string GetPermaLink(string permaLink)
        {
            return permaLink.Substring(0, dbShareSettings.PermaLinkMaxLength);
        }
    }
}