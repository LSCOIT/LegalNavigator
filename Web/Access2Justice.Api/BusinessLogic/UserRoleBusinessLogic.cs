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

        public async Task<List<UserRole>> GetUserRoleDataAsync(string roleInformationId)
        {
            List<UserRole> userRole = new List<UserRole>();
            var result = await dbClient.FindItemsWhereAsync(dbSettings.UserRoleCollectionId, Constants.Id, roleInformationId);
            userRole = JsonUtilities.DeserializeDynamicObject<List<UserRole>>(result);
            return userRole;
        }
        
        public async Task<List<string>> GetPermissionDataAsyn(string oId)
        {
            List<string> permissionPaths = new List<string>();
            List<UserRole> userRole = new List<UserRole>();
            var userProfile = await dbUserProfile.GetUserProfileDataAsync(oId);
            if (userProfile != null && userProfile?.RoleInformationId != Guid.Empty)
            {
                userRole = await GetUserRoleDataAsync(userProfile.RoleInformationId.ToString());
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
            if (!(string.IsNullOrEmpty(userProfile?.OrganizationalUnit)))
            {
                List<string> orgUnits = ou.Split(Constants.Delimiter).Select(p => p.Trim()).ToList();
                List<string> userOUs = userProfile.OrganizationalUnit.Split(Constants.Delimiter).Select(p => p.Trim()).ToList();
                foreach (var userOU in userOUs)
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
