using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<dynamic> GetUserProfileDataAsync(string oId);
        Task<dynamic> CreateUserProfileDataAsync(UserProfile userProfile);
        Task<object> CreateUserPersonalizedPlanAsync(dynamic userData);
        Task<object> UpdateUserPersonalizedPlanAsync(string id, dynamic userData);
        Task<object> UpsertUserPersonalizedPlanAsync(dynamic userData);
        void DeleteUserPersonalizedPlanAsync(dynamic userData);
    }
}