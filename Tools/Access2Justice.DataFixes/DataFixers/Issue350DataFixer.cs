using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access2Justice.DataFixes.DataAccess;
using Access2Justice.DataFixes.Helpers;
using Access2Justice.DataFixes.Models;

namespace Access2Justice.DataFixes.DataFixers
{
    public class Issue350DataFixer : IssueFixerBase, IDataFixer
    {
        protected override string IssueId => "#350";

        public async Task ApplyFixAsync(
            CosmosDbSettings cosmosDbSettings,
            CosmosDbService cosmosDbService)
        {

            LogEntry("Loading profiles");
            List<UserProfile> profiles = JsonHelper.Deserialize<List<UserProfile>>(await cosmosDbService.FindAllItemsAsync(cosmosDbSettings.ProfilesCollectionId));
            if (profiles.Count == 0)
            {
                LogEntry("No profiles found, finishing");
                return;
            }
            var userNameToSharedResource = new Dictionary<string, Guid>();
            var incomingResourcesIds = new HashSet<Guid>();
            foreach (var profile in profiles)
            {
                userNameToSharedResource[profile.FullName] = profile.SharedResourceId;
                if (profile.IncomingResourcesId != Guid.Empty)
                {
                    incomingResourcesIds.Add(profile.IncomingResourcesId);
                }
            }

            if (incomingResourcesIds.Count == 0)
            {
                LogEntry("No incoming resources found, finishing");
                return;
            }

            LogEntry("Loading incoming resources");
            List<UserIncomingResources> incomingResources = JsonHelper.Deserialize<List<UserIncomingResources>>(await cosmosDbService.QueryItemsAsync(cosmosDbSettings.UserResourcesCollectionId,
                $"SELECT * FROM c WHERE c.id in ({string.Join(",", incomingResourcesIds.Select(x => $"'{x.ToString()}'"))})"));

            var resourcesToUpdate = new List<UserIncomingResources>();
            foreach (var incomingResource in incomingResources)
            {
                if (incomingResource.Resources == null)
                {
                    continue;
                }
                var needsToUpdate = false;
                foreach (var resource in incomingResource.Resources)
                {
                    if (string.IsNullOrWhiteSpace(resource.SharedBy))
                    {
                        continue;
                    }

                    if (resource.SharedFromResourceId != Guid.Empty)
                    {
                        continue;
                    }

                    needsToUpdate |= userNameToSharedResource.TryGetValue(resource.SharedBy, out var sharedFromId);
                    resource.SharedFromResourceId = sharedFromId;
                }

                if (needsToUpdate)
                {
                    resourcesToUpdate.Add(incomingResource);
                }
            }

            if (resourcesToUpdate.Count == 0)
            {
                LogEntry("No items to update, finishing");
                return;
            }
            
            LogEntry($"Updating {resourcesToUpdate.Count} incoming resources");
            foreach (var resourceToUpdate in resourcesToUpdate)
            {
                await cosmosDbService.UpdateItemAsync(resourceToUpdate.IncomingResourcesId.ToString(), resourceToUpdate,
                    cosmosDbSettings.UserResourcesCollectionId);
            }
        }
    }
}
