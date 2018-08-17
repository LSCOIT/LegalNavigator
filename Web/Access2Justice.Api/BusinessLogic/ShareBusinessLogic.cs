using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public ShareBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService, IShareSettings shareSettings,
            IUserProfileBusinessLogic userProfileBusinessLogic)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            dbShareSettings = shareSettings;
            dbUserProfile = userProfileBusinessLogic;
        }

        public async Task<ShareViewModel> CheckPermaLinkDataAsync(ShareInput shareInput)
        {
            if (shareInput.UserId == null || shareInput.Url == null)
            {
                return null;
            }
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(shareInput.UserId);
            if (userProfile == null || userProfile.SharedResource == null)
            {
                return null;
            }
            var resource = userProfile.SharedResource.FindAll(a => a.Url.OriginalString.
            Contains(shareInput.Url.OriginalString)
            && DateTime.Compare(a.ExpirationDate, DateTime.UtcNow) >= 0);

            return resource.Count == 0 ? null : new ShareViewModel
            {
                PermaLink = GetPermaLink(resource.Select(a => a.PermaLink).First())
            };
        }
        public async Task<ShareViewModel> ShareResourceDataAsync(ShareInput shareInput)
        {
            if (shareInput.Url == null || shareInput.UserId == null || shareInput.ResourceId == null)
            {
                return null;
            }
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(shareInput.UserId);

            shareInput.UniqueId = shareInput.UniqueId != Guid.Empty ? shareInput.UniqueId : Guid.NewGuid();
            shareInput.ResourceId = shareInput.ResourceId != Guid.Empty ? shareInput.ResourceId : Guid.NewGuid();

            var permaLink = Utilities.GenerateSHA256String(shareInput.UniqueId + shareInput.UserId +
                shareInput.ResourceId);
            var sharedResource = new SharedResource
            {
                ExpirationDate = DateTime.UtcNow.AddYears(Constants.ExpirationDateDurationInYears),
                IsShared = true,
                Url = new Uri(shareInput.Url.OriginalString, UriKind.RelativeOrAbsolute),
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
            var response = await dbService.UpdateItemAsync(userProfile.Id, userProfile,
                dbSettings.UserProfileCollectionId);
            return response == null ? null : new ShareViewModel
            {
                PermaLink = dbShareSettings.PermaLinkMaxLength > 0 ? GetPermaLink(permaLink) : permaLink
            };
        }

        public async Task<object> UnshareResourceDataAsync(ShareInput unShareInput)
        {
            if (unShareInput.UserId == null || unShareInput.ResourceId == null || unShareInput.Url == null)
            {
                return null;
            }
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(unShareInput.UserId);
            if (userProfile == null || userProfile.SharedResource == null || userProfile.SharedResource.Count == 0)
            {
                return null;
            }
            var sharedResource = userProfile.SharedResource.FindAll(a => a.Url.OriginalString.
            Contains(unShareInput.Url.OriginalString));
            if (sharedResource.Count == 0)
            {
                return false;
            }
            userProfile.SharedResource.RemoveAll(a => a.Url.OriginalString.
            Contains(unShareInput.Url.OriginalString));
            var response = await dbService.UpdateItemAsync(userProfile.Id, userProfile,
                dbSettings.UserProfileCollectionId);
            return response == null ? false : true;
        }


        public async Task<object> GetPermaLinkDataAsync(string permaLink)
        {
            if (string.IsNullOrEmpty(permaLink))
            {
                return null;
            }
            var response = await dbClient.FindFieldWhereArrayContainsAsync(dbSettings.UserProfileCollectionId,
                Constants.SharedResource, Constants.PermaLink, permaLink, Constants.ExpirationDate);
            if (response == null)
            {
                return null;
            }
            List<ShareProfileResponse> shareProfileResponse = JsonConvert.DeserializeObject<List<ShareProfileResponse>>
                (JsonConvert.SerializeObject(response));
            ShareProfileViewModel profileViewModel = new ShareProfileViewModel();
            foreach (var profile in shareProfileResponse)
            {
                if (profile.Url.OriginalString == Constants.ProfileLink)
                {
                    profileViewModel.UserName = profile.FistName + " " + profile.LastName;
                    profileViewModel.UserId = profile.OId;
                }
                profileViewModel.ResourceLink = profile.Url.OriginalString;
            }
            return profileViewModel;
        }

        private string GetPermaLink(string permaLink)
        {
            return permaLink.Substring(0, dbShareSettings.PermaLinkMaxLength);
        }

    }
}