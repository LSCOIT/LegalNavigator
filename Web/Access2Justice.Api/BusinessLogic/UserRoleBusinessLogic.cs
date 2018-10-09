using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class UserRoleBusinessLogic : IUserRoleBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IUserProfileBusinessLogic dbUserProfile;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserRoleBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IUserProfileBusinessLogic userProfileBusinessLogic,
            IHttpContextAccessor httpContextAccessor)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbUserProfile = userProfileBusinessLogic;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<string>> GetPermissionDataAsync(string oId)
        {
            List<string> permissionPaths = new List<string>();
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile?.RoleInformationId.Count() > 0)
            {
                var roleIdsList = userProfile.RoleInformationId.Select(x => x.ToString()).ToList().Distinct();
                var permissionData = await dbClient.FindItemsWhereInClauseAsync(dbSettings.UserRoleCollectionId, Constants.Id, roleIdsList);
                List<Role> userRole = JsonUtilities.DeserializeDynamicObject<List<Role>>(permissionData);
                return userRole.SelectMany(x => x.Permissions).Distinct().ToList();
            }
            return permissionPaths;
        }

        public async Task<bool> ValidateOrganizationalUnit(string ou)
        {
            string oId = string.Empty;
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                oId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            }
            if (string.IsNullOrEmpty(ou) || string.IsNullOrEmpty(oId))
                return false;
            oId = EncryptionUtilities.GenerateSHA512String(oId);
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile?.RoleInformationId.Count() > 0)
            {
                List<string> roleIdsList = userProfile.RoleInformationId.Select(x => x.ToString()).Distinct().ToList();
                return await ValidateOUForRole(roleIdsList, ou);
            }
            return false;
        }

        public async Task<bool> ValidateOUForRole(List<string> roleInformationId, string ou)
        {
            var roleData = await dbClient.FindItemsWhereInClauseAsync(dbSettings.UserRoleCollectionId, Constants.Id, roleInformationId);
            List<Role> userRole = JsonUtilities.DeserializeDynamicObject<List<Role>>(roleData);
            if (userRole?.Count() > 0)
            {
                List<string> roles = userRole.Select(x => x.RoleName).Distinct().ToList();
                if (roles.Contains(Permissions.Role.PortalAdmin.ToString()))
                {
                    return true;
                }
                List<string> userOUs = userRole.SelectMany(x => x.OrganizationalUnit.Split(Constants.Delimiter).Select(p => p.Trim())).Distinct().ToList();
                List<string> orgUnits = ou.Split(Constants.Delimiter).Select(p => p.Trim()).ToList();
                foreach(var userOU in userOUs)
                {
                    if (!string.IsNullOrEmpty(userOU) && orgUnits.Find(x => x.Contains(userOU)) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}