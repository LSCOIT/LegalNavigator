using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
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
					userSharedResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(userProfile.SharedResourceId, CultureInfo.InvariantCulture));
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
				await UpdatePersonalizedPlan(planId, true);
			}

			return response == null ? null : new ShareViewModel
			{
				PermaLink = dbShareSettings.PermaLinkMaxLength > 0 ? GetPermaLink(permaLink) : permaLink
			};
		}
		public async Task<object> UpsertSharedResource(UserProfile userProfile, SharedResource sharedResource)
		{
			List<SharedResource> sharedResources = new List<SharedResource>();
			dynamic userSharedResourcesDBData = null;
			dynamic response = null;
			if (userProfile?.SharedResourceId != null && userProfile.SharedResourceId != Guid.Empty)
			{
				userSharedResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(userProfile.SharedResourceId, CultureInfo.InvariantCulture));
			}
			if (userSharedResourcesDBData != null && userSharedResourcesDBData.Count > 0)
			{
				var userSharedResources = new List<SharedResources>();
				userSharedResources = JsonUtilities.DeserializeDynamicObject<List<SharedResources>>(userSharedResourcesDBData);
				userSharedResources[0].SharedResourceId = userProfile.SharedResourceId;
				userSharedResources[0].SharedResource.Add(sharedResource);
				response = await dbService.UpdateItemAsync(userProfile.SharedResourceId.ToString(), userSharedResources[0],
				dbSettings.UserResourceCollectionId);
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
				dbSettings.UserProfileCollectionId);
				response = await dbService.CreateItemAsync((userSharedResources), dbSettings.UserResourceCollectionId);
			}
			return response;
		}
		public async Task UpdatePersonalizedPlan(string planId, bool isShared)
		{
			var plan = await dbPersonalizedPlan.GetPersonalizedPlan(planId);
			if (plan != null)
			{
				plan.IsShared = isShared;
				UserPersonalizedPlan userPlan = new UserPersonalizedPlan
				{
					PersonalizedPlan = plan
				};
				await dbPersonalizedPlan.UpdatePersonalizedPlan(userPlan);
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
				userSharedResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.UserResourceCollectionId, Constants.Id, Convert.ToString(userProfile.SharedResourceId, CultureInfo.InvariantCulture));
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
				dbSettings.UserResourceCollectionId);
			if (unShareInput.Url.OriginalString.Contains("plan"))
			{
				string planId = unShareInput.Url.OriginalString.Substring(6);
				await UpdatePersonalizedPlan(planId, false);
			}
			return response == null ? false : true;
		}
		public async Task<object> GetPermaLinkDataAsync(string permaLink)
		{
			if (string.IsNullOrEmpty(permaLink))
			{
				return null;
			}
			var response = await dbClient.FindFieldWhereArrayContainsAsync(dbSettings.UserResourceCollectionId,
				Constants.SharedResource, Constants.PermaLink, permaLink, Constants.ExpirationDate);
            if (response == null)
            {
                return null;
            }
            List<ShareProfileDetails> shareProfileDetails = JsonUtilities.DeserializeDynamicObject<List<ShareProfileDetails>>(response);
            ShareProfileViewModel profileViewModel = new ShareProfileViewModel();

            if (shareProfileDetails.Count() > 0)
            {
                var userprofileResponse = await dbClient.FindFieldWhereArrayContainsAsync(dbSettings.UserProfileCollectionId, Constants.sharedResourceId, shareProfileDetails[0].Id);
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