using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<dynamic> GetUserProfileDataAsync(string oId, bool isProfileViewModel = false);
        Task<UserProfile> GetUserProfileByEmailAsync(string email);
        Task<dynamic> GetUserResourceProfileDataAsync(string oId, string type);
        Task<dynamic> GetUserResourceProfileDataAsync(UserProfile userProfile, string type);
        Task<dynamic> UpsertUserSavedResourcesAsync(ProfileResources userResources);
        Task<dynamic> UpsertUserIncomingResourcesAsync(ProfileIncomingResources userData);
        Task<UserProfileViewModel> UpsertUserProfileAsync(UserProfile userProfile);
        Task<Guid> GetDefaultUserRole();
        Task<List<Role>> GetRoleDetailsAsync(List<string> roleInformationId);
    }
}