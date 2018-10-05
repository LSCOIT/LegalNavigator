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
            if (userProfile?.RoleInformationId != Guid.Empty)
            {
                var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, userProfile.RoleInformationId.ToString());
                List<Role> userRole = JsonUtilities.DeserializeDynamicObject<List<Role>>(result);
                return userRole.SelectMany(x => x.Permissions).ToList();
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
            if (userProfile?.RoleInformationId != Guid.Empty)
            {
                return await ValidateOUForRole(userProfile.RoleInformationId.ToString(), ou);
            }
            return false;
        }

        public async Task<bool> ValidateOUForRole(string roleInformationId, string ou)
        {
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, roleInformationId);
            List<Role> userRole = JsonUtilities.DeserializeDynamicObject<List<Role>>(result);
            if (userRole?.Count() > 0)
            {
                if (userRole[0].RoleName == Permissions.Role.PortalAdmin.ToString())
                {
                    return true;
                }
                List<string> orgUnits = ou.Split(Constants.Delimiter).Select(p => p.Trim()).ToList();
                string userOU = userRole[0].OrganizationalUnit;
                if (!string.IsNullOrEmpty(userOU) && orgUnits.Find(x => x.Contains(userOU)) != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}