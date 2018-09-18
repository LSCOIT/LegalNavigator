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

        public async Task<RolePermissionAccess> GetRoleInfo(string oId)
        {
            List<UserRole> userRole = new List<UserRole>();
            List<RolePermission> rolePermissions = new List<RolePermission>();
            RolePermissionAccess rolePermissionAccess = null;
            var userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile != null && userProfile?.RoleInformationId != Guid.Empty)
            {
                userRole = await GetUserRoleDataAsync(userProfile?.RoleInformationId.ToString());
                if (userRole.Count() > 0)
                {
                    if (userRole[0].Permissions.Count() > 0)
                    {
                        foreach (var permission in userRole[0].Permissions)
                        {
                            RolePermission rolePermission = await GetPermissionDataAsyn(permission.ToString());
                            rolePermissions.Add(rolePermission);
                        }
                    }
                    rolePermissionAccess = new RolePermissionAccess
                    {
                        RoleName = userRole[0].RoleName,
                        RolePermissions = rolePermissions
                    };
                }
            }
            return rolePermissionAccess;
        }

        public async Task<RolePermission> GetPermissionDataAsyn(string permission)
        {
            RolePermission rolePermission = null;
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, permission);
            List<RolePermission> rolePermissions = JsonUtilities.DeserializeDynamicObject<List<RolePermission>>(result);
            if (rolePermissions.Count() > 0)
            {
                rolePermission = rolePermissions[0];
            }
            return rolePermission;
        }
    }
}
