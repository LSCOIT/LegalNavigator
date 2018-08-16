using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Azure.Documents;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<UserProfile> GetUserProfileDataAsync(string oId);
        Task<dynamic> GetUserResourceProfileDataAsync(string oId);
        Task<UserProfile> UpdateUserProfilePlanIdAsync(string oId, Guid planId);
        Task<dynamic> UpsertUserSavedResourcesAsync(ProfileResources userData);
        Task<object> ShareResourceDataAsync(ShareInput shareInput);
        Task<object> UnshareResourceDataAsync(UnShareInput shareInput);
    }
}