using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<UserProfile> GetUserProfileDataAsync(string oId);
        Task<dynamic> GetUserResourceProfileDataAsync(string oId);
        Task<dynamic> UpdateUserProfileDataAsync(UserProfile userProfile);
        Task<dynamic> UpdateUserProfilePlanIdAsync(string oId, Guid planId);
        Task<dynamic> UpsertUserPersonalizedPlanAsync(dynamic userData);
    }
}