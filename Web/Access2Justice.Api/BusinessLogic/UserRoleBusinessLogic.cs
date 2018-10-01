using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
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
        private readonly IBackendDatabaseService dbService;
        private readonly IUserProfileBusinessLogic dbUserProfile;
        private readonly ActionExecutingContext context;

        public UserRoleBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IBackendDatabaseService backendDatabaseService, IUserProfileBusinessLogic userProfileBusinessLogic)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            dbUserProfile = userProfileBusinessLogic;
        }
        public async Task<List<UserRole>> GetUserRoles()
        {
            List<UserRole> userRole = new List<UserRole>();
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Type, Constants.UserRole);
            userRole = JsonUtilities.DeserializeDynamicObject<List<UserRole>>(result);
            return userRole;
        }

        public async Task<List<UserRole>> GetUserRoleDataAsync(string roleInformationId)
        {
            List<UserRole> userRole = new List<UserRole>();
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, roleInformationId);
            userRole = JsonUtilities.DeserializeDynamicObject<List<UserRole>>(result);
            return userRole;
        }

        public async Task<string> GetRoleInfo(string oId)
        {
            string roleName = string.Empty;
            List<UserRole> userRole = new List<UserRole>();
            var userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile != null && userProfile?.RoleInformationId != Guid.Empty)
            {
                userRole = await GetUserRoleDataAsync(userProfile?.RoleInformationId.ToString());
                if (userRole.Count() > 0)
                {
                    roleName = userRole[0].RoleName;
                }
            }
            return roleName;
        }

        public async Task<UserProfile> GetUserProfileDataAsync(string userName)
        {
            var resultUserData = await dbClient.FindItemsWhereAsync(dbSettings.UserProfileCollectionId, Constants.UserName, userName);
            List<UserProfile> userProfile = JsonUtilities.DeserializeDynamicObject<List<UserProfile>>(resultUserData);
            return userProfile.Count() == 0 ? null : userProfile[0];
        }

        public async Task<List<string>> GetPermissionDataAsyn(string oId)
        {
            List<string> permissionPaths = new List<string>();
            List<UserRole> userRole = new List<UserRole>();
            var userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile != null && userProfile?.RoleInformationId != Guid.Empty)
            {
                userRole = await GetUserRoleDataAsync(userProfile?.RoleInformationId.ToString());
                if (userRole.Count() > 0)
                {
                    if (userRole[0].Permissions.Count() > 0)
                    {
                        return userRole[0].Permissions;
                    }
                }
            }
            return permissionPaths;
        }
        public async Task<List<Permission>> GetPermissionDetails()
        {
            List<Permission> permissions = new List<Permission>();
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Type, Constants.PermissionDetails);
            List<PermissionDetails> permissionDetails = JsonUtilities.DeserializeDynamicObject<List<PermissionDetails>>(result);
            if (permissionDetails.Count() > 0)
            {
                if (permissionDetails[0].Permissions.Count() > 0)
                {
                    foreach (var permission in permissionDetails[0].Permissions)
                    {
                        permissions.Add(new Permission()
                        {
                            PermissionId = permission.PermissionId,
                            PermissionName = permission.PermissionName,
                            Path = permission.Path
                        });
                    }
                }
            }
            return permissions;
        }

        public async Task<bool> GetOrganizationalUnit(string oId, string ou)
        {
            string sdf = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            //var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(ou) || string.IsNullOrEmpty(oId))
            {
                return false;
            }
            string organizationalUnit = string.Empty;
            List<UserRole> userRole = new List<UserRole>();
            var userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile != null && userProfile?.RoleInformationId != Guid.Empty)
            {
                userRole = await GetUserRoleDataAsync(userProfile?.RoleInformationId.ToString());
                if (userRole.Count() > 0)
                {
                    organizationalUnit = userRole[0].OrganizationalUnit;
                }
            }
            return organizationalUnit == ou;
        }
    }
}
