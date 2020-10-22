using Access2Justice.Api.Authentication;
using Access2Justice.Api.Authorization;
using Access2Justice.Api.Interfaces;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IConfiguration configuration;

        public UserRoleBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings,
            IUserProfileBusinessLogic userProfileBusinessLogic,
            IHttpContextAccessor httpContextAccessor, IOptions<AzureAdOptions> azureOptions,
            IConfiguration configuration)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbUserProfile = userProfileBusinessLogic;
            this.httpContextAccessor = httpContextAccessor;
            this.azureOptions = azureOptions.Value;
            this.configuration = configuration;
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
            if (this.IsApplication())
            {
                return true;
            }
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

        public string GetOId()
        {
            var oId = httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == azureOptions.UserClaimsUrl)?.Value ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(oId))
            {
                oId = EncryptionUtilities.GenerateSHA512String(oId);
            }

            return oId;
        }

        public async Task<bool> ValidateOUForRole(List<string> roleInformationId, string ou)
        {
            List<Role> userRole = await dbUserProfile.GetRoleDetailsAsync(roleInformationId);
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

        public bool IsApplication()
        {
            try
            {
                IEnumerable<Claim> claims = httpContextAccessor.HttpContext.User.Claims;
                IConfigurationSection apiOids = configuration.GetSection("ApiOids");
                IEnumerable<string> oids = apiOids.GetSection("oIds").Get<IEnumerable<string>>();
                string azpacr = apiOids.GetSection(Constants.Azpacr).Get<string>();
                string oId = claims.FirstOrDefault(c => c.Type == azureOptions.UserClaimsUrl)?.Value ?? string.Empty;
                string sub = claims.FirstOrDefault(c => c.Type == Constants.NameIdentifier)?.Value ?? string.Empty;
                string azpacrClaim = claims.FirstOrDefault(c => c.Type == Constants.Azpacr)?.Value ?? string.Empty;

                // Application tokens always have the same value of oId and sub claims
                // azpacr 1 means that token was generated using client secret
                if (!string.IsNullOrEmpty(oId) && oids.Contains(oId) && oId == sub && azpacr == azpacrClaim)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}