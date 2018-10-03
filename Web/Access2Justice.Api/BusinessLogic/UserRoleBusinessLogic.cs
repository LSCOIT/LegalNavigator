﻿using Access2Justice.Api.Interfaces;
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
            if (string.IsNullOrEmpty(ou) || string.IsNullOrEmpty(oId))
                return false;

            oId = EncryptionUtilities.GenerateSHA512String(oId);
            var userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile?.RoleInformationId != Guid.Empty)
            {
                List<UserRole> userRole = await GetUserRoleDataAsync(userProfile.RoleInformationId.ToString());
                if (userRole?.Count() > 0)
                {
                    List<string> orgUnits = ou.Split(Constants.Delimiter).Select(p => p.Trim()).ToList();
                    List<string> userOUs = userRole[0].OrganizationalUnit.Split(Constants.Delimiter).Select(p => p.Trim()).ToList();
                    foreach (var userOU in userOUs)
                    {
                        if (!string.IsNullOrEmpty(userOU) && orgUnits.Find(x => x.Contains(userOU)) != null)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
