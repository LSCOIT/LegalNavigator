using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IUserRoleBusinessLogic
    {
        Task<List<UserRole>> GetUserRoles();
        Task<Guid> GetDefaultUserRole();
        Task<string> GetRoleInfo(string oId);
        Task<List<string>> GetPermissionDataAsyn(string oId);
    }
}
