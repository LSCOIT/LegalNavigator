using Access2Justice.Api.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
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

        public async Task<UserRole> GetDefaultUserRole()
        {
            List<UserRole> userRole = new List<UserRole>();
            List<string> propertyNames = new List<string>() { Constants.Type, Constants.RoleName };
            List<string> values = new List<string>() { Constants.UserRole, Constants.DefaultUser };
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, propertyNames, values);
            userRole = JsonUtilities.DeserializeDynamicObject<List<UserRole>>(result);
            return userRole[0];
        }

        public async Task<dynamic> GetRoleNameById(string oId)
        {
            List<UserRole> userRole = new List<UserRole>();
            dynamic roleName = null;
            var userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile != null && userProfile?.RoleInformationId != Guid.Empty)
            {
                userRole = await GetUserRoleDataAsync(userProfile?.RoleInformationId.ToString());
                if (userRole.Count() > 0)
                {
                    roleName = userRole[0]?.RoleName;
                }
            }
            return roleName;
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
                        List<Permission> permissions = await GetPermissionDetails();
                        foreach (var permission in userRole[0].Permissions)
                        {
                            foreach (var perm in permissions)
                            {
                                if(permission == perm.PermissionId)
                                {
                                    permissionPaths.Add(perm.Path);
                                }
                            }
                        }
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
    }
}
