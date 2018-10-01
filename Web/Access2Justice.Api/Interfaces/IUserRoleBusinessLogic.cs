using Access2Justice.Shared.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IUserRoleBusinessLogic
    {
        Task<List<UserRole>> GetUserRoles();
        Task<string> GetRoleInfo(string oId);
        Task<List<string>> GetPermissionDataAsyn(string oId);
        Task<bool> GetOrganizationalUnit(string oId, string ou);
    }
}
