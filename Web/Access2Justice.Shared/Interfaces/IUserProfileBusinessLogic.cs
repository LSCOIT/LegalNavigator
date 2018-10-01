using Access2Justice.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<dynamic> GetUserProfileDataAsync(string oId, bool isProfileViewModel = false);
        Task<dynamic> GetUserResourceProfileDataAsync(string oId, string type);
        Task<UserProfile> UpdateUserProfilePlanIdAsync(string oId, Guid planId);
        Task<dynamic> UpsertUserSavedResourcesAsync(ProfileResources userResources);
        Task<UserProfileViewModel> UpsertUserProfileAsync(UserProfile userProfile);
    }
}