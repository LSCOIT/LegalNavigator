using Access2Justice.Api.Authentication;
using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
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
        private readonly AzureAdOptions azureOptions;

        public UserRoleBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IUserProfileBusinessLogic userProfileBusinessLogic,
            IHttpContextAccessor httpContextAccessor, IOptions<AzureAdOptions> azureOptions)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbUserProfile = userProfileBusinessLogic;
            this.httpContextAccessor = httpContextAccessor;
            this.azureOptions = azureOptions.Value;
        }

        public async Task<List<string>> GetPermissionDataAsync(string oId)
        {
            List<string> permissionPaths = new List<string>();
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile?.RoleInformationId.Count() > 0)
            {
                var roleIdsList = userProfile.RoleInformationId.Select(x => x.ToString()).ToList().Distinct();
                var permissionData = await dbClient.FindItemsWhereInClauseAsync(dbSettings.RolesCollectionId, Constants.Id, roleIdsList);
                List<Role> userRole = JsonUtilities.DeserializeDynamicObject<List<Role>>(permissionData);
                return userRole.SelectMany(x => x.Permissions).Distinct().ToList();
            }
            return permissionPaths;
        }

        public async Task<bool> ValidateOrganizationalUnit(string ou)
        {
            string oId = GetOId();
            if (string.IsNullOrEmpty(ou) || string.IsNullOrEmpty(oId))
                return false;
            UserProfile userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile?.RoleInformationId.Count() > 0)
            {
                List<string> roleIdsList = userProfile.RoleInformationId.Select(x => x.ToString()).Distinct().ToList();
                return await ValidateOUForRole(roleIdsList, ou);
            }
            return false;
        }

        private string GetOId()
        {
            string oId = string.Empty;
            if (httpContextAccessor.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                oId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == azureOptions.UserClaimsUrl).Value;
                oId = EncryptionUtilities.GenerateSHA512String(oId);
            }
            return oId;
        }

        public async Task<bool> ValidateOUForRole(List<string> roleInformationId, string ou)
        {
            var roleData = await dbClient.FindItemsWhereInClauseAsync(dbSettings.RolesCollectionId, Constants.Id, roleInformationId);
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