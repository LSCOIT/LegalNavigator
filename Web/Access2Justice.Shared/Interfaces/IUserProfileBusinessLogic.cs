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
        Task<int> CreateUserProfileDataAsync(UserProfile userProfile);
        Task<int> UpdateUserProfileDataAsync(string oId, Guid planId);
        Task<object> CreateUserPersonalizedPlanAsync(dynamic userData);
        Task<object> UpdateUserPersonalizedPlanAsync(string id, dynamic userData);
        Task<object> UpsertUserPersonalizedPlanAsync(dynamic userData);
        Task<object> UpsertUserPlanAsync(dynamic userPlan);
    }
}