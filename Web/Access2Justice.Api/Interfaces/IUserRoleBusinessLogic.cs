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
        Task<List<string>> GetPermissionDataAsync(string oId);
        Task<bool> ValidateOrganizationalUnit(string ou);
        Task<bool> ValidateOUForRole(List<string> roleInformationId, string ou);
        string GetOId();
        bool IsApplication();
    }
}
