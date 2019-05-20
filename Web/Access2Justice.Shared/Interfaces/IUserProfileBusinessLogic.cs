using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        [Obsolete("Use generic version")]
        Task<dynamic> GetUserProfileDataAsync(string oId, bool isProfileViewModel = false);
        Task<T> GetUserProfileDataAsync<T>(string oId);
        Task<UserProfile> GetUserProfileByEmailAsync(string email);
        Task<dynamic> GetUserResourceProfileDataAsync(string oId, string type);
        Task<dynamic> GetUserResourceProfileDataAsync(UserProfile userProfile, string type);
        Task<dynamic> UpsertUserSavedResourcesAsync(ProfileResources userResources);
        Task<dynamic> UpsertUserIncomingResourcesAsync(ProfileIncomingResources userData);
        Task<dynamic> DeleteUserProfileResourceAsync(UserProfileResource resource);
        Task<UserProfileViewModel> UpsertUserProfileAsync(UserProfile userProfile);
        Task<Guid> GetDefaultUserRole();
        Task<List<Role>> GetRoleDetailsAsync(List<string> roleInformationId);
        Task<Document> DeleteUserSharedResource(ShareInput shareInput);
    }
}