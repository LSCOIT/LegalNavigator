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
        Task<UserRole> GetDefaultUserRole();
        Task<dynamic> GetRoleNameById(string oId);
        Task<RolePermissionAccess> GetRoleInfo(string oId);
    }
}
