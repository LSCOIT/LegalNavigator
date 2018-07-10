using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<dynamic> GetUserProfileDataAsync(string oId,string collectionId);
        Task<dynamic> CreateUserProfileDataAsync(UserProfile userProfile);
        Task<dynamic> UpdateUserProfileDataAsync(UserProfile userProfile, string useroId);
        Task<object> CreateUserPersonalizedPlanAsync(dynamic userData);
        Task<object> UpdateUserPersonalizedPlanAsync(string id, dynamic userData);
        Task<object> UpsertUserPersonalizedPlanAsync(dynamic userData);
        Task<object> UpsertUserPlanAsync(dynamic userPlan);
    }
}