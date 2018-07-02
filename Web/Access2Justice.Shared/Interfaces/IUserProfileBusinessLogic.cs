using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<dynamic> GetUserProficeDataAsync(string oId);
        Task<dynamic> CreateUserProfileDataAsync(UserProfile userProfile);             
    }
}