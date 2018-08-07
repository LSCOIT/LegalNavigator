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
using System.Linq;
using Access2Justice.Api.ViewModels;
using Access2Justice.Api.Interfaces;

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
            IBackendDatabaseService backendDatabaseService, IShareSettings shareSettings,IUserProfileBusinessLogic userProfileBusinessLogic)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            dbShareSettings = shareSettings;
            dbUserProfile = userProfileBusinessLogic;
        }

        public async Task<ShareViewModel> CheckPermaLinkDataAsync(ShareInput shareInput)
        {
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(shareInput.UserId);
            if (userProfile.SharedResource != null)
            {
                var resource = userProfile.SharedResource.FindAll(a => a.Url.OriginalString.Contains(Convert.ToString(shareInput.ResourceId, CultureInfo.InvariantCulture)));
                if (resource.Count > 0)
                {
                    return new ShareViewModel
                    {
                        PermaLink = GetPermaLink(resource.Select(a => a.PermaLink).First())
                    };
                }
            }
            return new ShareViewModel();
        }
        public async Task<ShareViewModel> ShareResourceDataAsync(ShareInput shareInput)
        {
            try
            {
                UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(shareInput.UserId);
                var permaLink = Utilities.GenerateSHA256String(Guid.NewGuid() + shareInput.UserId + shareInput.ResourceId);
                var sharedResource = new SharedResource
                {
                    ExpirationDate = DateTime.UtcNow.AddYears(1),
                    IsShared = true,
                    Url = new Uri(shareInput.Url.OriginalString, UriKind.Relative),
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
                return new ShareViewModel
                {
                    PermaLink = dbShareSettings.PermaLinkMaxLength > 0 ? GetPermaLink(permaLink) : permaLink
                };
            }
            catch
            {
                // log exception
                return null;
            }
        }

        public async Task<object> UnshareResourceDataAsync(UnShareInput unShareInput)
        {
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(unShareInput.UserId);
            var sharedResource = userProfile.SharedResource.FindAll(a => a.Url.OriginalString.Contains(Convert.ToString(unShareInput.ResourceId, CultureInfo.InvariantCulture)));
            if (sharedResource.Count == 0)
            {
                return false;
            }
            userProfile.SharedResource.RemoveAll(a => a.Url.OriginalString.Contains(Convert.ToString(unShareInput.ResourceId, CultureInfo.InvariantCulture)));
            var response = await UpdateUserProfileDataAsync(userProfile);
            if (response == null)
            {
                return StatusCodes.Status500InternalServerError;
            }
            return true;
        }


        public async Task<object> GetPermaLinkDataAsync(string permaLink)
        {
            return await dbClient.FindFieldWhereArrayContainsAsync(dbSettings.UserProfileCollectionId, Constants.StoredResource, Constants.Url, Constants.PermaLink, permaLink);
        }

        public async Task<object> UpdateUserProfileDataAsync(UserProfile userProfile)
        {
            return await dbService.UpdateItemAsync(userProfile.Id, JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(userProfile)), dbSettings.UserProfileCollectionId);
        }

        private string GetPermaLink(string permaLink)
        {
            return permaLink.Substring(0, dbShareSettings.PermaLinkMaxLength);
        }

    }
}