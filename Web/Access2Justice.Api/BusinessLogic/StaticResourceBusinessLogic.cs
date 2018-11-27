using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class StaticResourceBusinessLogic : IStaticResourceBusinessLogic
    {
        private readonly IDynamicQueries dbClient;
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        public StaticResourceBusinessLogic(IDynamicQueries dynamicQueries, ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbClient = dynamicQueries;
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public async Task<dynamic> GetPageStaticResourcesDataAsync(Location location)
        {
            dynamic result = null;
            location.County = string.Empty;
            location.City = string.Empty;
            location.ZipCode = string.Empty;
            if (!string.IsNullOrEmpty(location.State))
            {
                result = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, location);
            }
            location.State = "Default";
            return result.Count > 0 ? result : await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, location);
        }

        public async Task<dynamic> UpsertStaticHomePageDataAsync(HomeContent homePageContent)
        {
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, homePageContent.Name, homePageContent.Location.FirstOrDefault());
            if (pageDBData.Count == 0)
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(homePageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                homePageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(homePageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromisePageContent)
        {
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, privacyPromisePageContent.Name, privacyPromisePageContent.Location.FirstOrDefault());
            if (pageDBData.Count == 0)
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(privacyPromisePageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                privacyPromisePageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(privacyPromisePageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent)
        {
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, helpAndFAQPageContent.Name, helpAndFAQPageContent.Location.FirstOrDefault());
            if (pageDBData.Count == 0)
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(helpAndFAQPageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                helpAndFAQPageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(helpAndFAQPageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticNavigationDataAsync(Navigation navigationContent)
        {
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, navigationContent.Name, navigationContent.Location.FirstOrDefault());
            if (pageDBData.Count == 0)
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(navigationContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                navigationContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(navigationContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticAboutPageDataAsync(AboutContent aboutPageContent)
        {
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, aboutPageContent.Name, aboutPageContent.Location.FirstOrDefault());
            if (pageDBData.Count == 0)
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(aboutPageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                aboutPageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(aboutPageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }
    }
}