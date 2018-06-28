using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IUserProfileBusinessLogic
    {
        Task<dynamic> GetUserProficeDataAsync(string id);
    }
}