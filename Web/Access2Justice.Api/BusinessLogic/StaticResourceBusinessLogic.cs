using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Utilities;
using System;
using System.Collections.Generic;
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

        public async Task<dynamic> GetAllStaticResourcesAsync()
        {
            var groupedStaticResources = new Dictionary<string, Dictionary<string, List<dynamic>>>();
            var staticResources = await dbClient.FindItemsAllAsync(dbSettings.StaticResourcesCollectionId);

            foreach(var staticResource in staticResources)
            {
                var organizationalUnit = (string)staticResource.organizationalUnit;
                if (string.IsNullOrWhiteSpace(organizationalUnit))
                {
                    organizationalUnit = Constants.DefaultOgranizationalUnit;
                }
                var name = (string)staticResource.name;

                if (!groupedStaticResources.TryGetValue(organizationalUnit,
                    out Dictionary<string, List<dynamic>> orgUnitResources))
                {
                    orgUnitResources = new Dictionary<string, List<dynamic>>();
                    groupedStaticResources[organizationalUnit] = orgUnitResources;
                }

                if (!orgUnitResources.TryGetValue(name, out List<dynamic> resources))
                {
                    resources = new List<dynamic>();
                    orgUnitResources[name] = resources;
                }

                resources.Add(staticResource);
            }

            return groupedStaticResources;
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
            location.State = Constants.DefaultOgranizationalUnit;
            return result.Count > 0 ? result : await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, location);
        }

        public async Task<dynamic> CreateStaticResourcesFromDefaultAsync(Location location)
        {
            dynamic stateStaticResources = null;
            location.County = string.Empty;
            location.City = string.Empty;
            location.ZipCode = string.Empty;

            stateStaticResources = await dbClient.FindItemsWhereWithLocationAsync(
                dbSettings.StaticResourcesCollectionId, Constants.Name, location);

            if (stateStaticResources.Count > 0)
            {
                return stateStaticResources;
            }

            var defaultLocation = new Location
            {
                State = Constants.DefaultOgranizationalUnit,
                City = string.Empty,
                County = string.Empty,
                ZipCode = string.Empty
            };

            var defaultStaticResources = await dbClient.FindItemsWhereWithLocationAsync(
                dbSettings.StaticResourcesCollectionId, Constants.Name, defaultLocation);

            var createdStaticResources = new List<dynamic>();

            foreach(var staticResource in defaultStaticResources)
            {
                var newStaticResource = DeserializeStaticContentData(staticResource);

                newStaticResource.Id = Guid.NewGuid().ToString();
                newStaticResource.OrganizationalUnit = location.State;
                newStaticResource.Location = new List<Location> { location };

                var newStaticContentObject =
                    JsonUtilities.DeserializeDynamicObject<object>(newStaticResource);

                createdStaticResources.Add(
                    dbService.CreateItemAsync(newStaticResource, dbSettings.StaticResourcesCollectionId));
            }

            return createdStaticResources;
        }

        public async Task<dynamic> UpsertStaticHomePageDataAsync(HomeContent homePageContent)
        {
            dynamic result = null;
            EnsureNotNullLocation(homePageContent);

            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(
                dbSettings.StaticResourcesCollectionId,
                Constants.Name,
                homePageContent.Name,
                homePageContent.Location.FirstOrDefault());

            if (pageDBData?.Count > 0)
            {
                string id = pageDBData[0].id;
                homePageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(homePageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(homePageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<Dictionary<string, Image>> RetrieveLogo(
            IEnumerable<string> organizationalUnits)
        {
            Dictionary<string, Image> result = null;
            foreach (var location in organizationalUnits.Distinct().Select(x => new Location
            {
                City = string.Empty,
                County = string.Empty,
                State = x,
                ZipCode = string.Empty
            }))
            {
                HomeContent homeResources = JsonUtilities.DeserializeDynamicObject<List<HomeContent>>(
                    await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId,
                        Constants.Name, Constants.StaticResourceTypes.HomePage, location))[0];
                (result ?? (result = new Dictionary<string, Image>()))[location.State] = homeResources
                    ?.Carousel
                    ?.Overviewdetails
                    ?.Select(x => x.Image)
                    .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Source));
            }

            return result;
        }

        public async Task<dynamic> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromisePageContent)
        {
            dynamic result = null;
            EnsureNotNullLocation(privacyPromisePageContent);

            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, privacyPromisePageContent.Name, privacyPromisePageContent.Location.FirstOrDefault());
            if (pageDBData?.Count > 0)
            {
                string id = pageDBData[0].id;
                privacyPromisePageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(privacyPromisePageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(privacyPromisePageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent)
        {
            dynamic result = null;
            EnsureNotNullLocation(helpAndFAQPageContent);

            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, helpAndFAQPageContent.Name, helpAndFAQPageContent.Location.FirstOrDefault());
            if (pageDBData?.Count > 0)
            {
                string id = pageDBData[0].id;
                helpAndFAQPageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(helpAndFAQPageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(helpAndFAQPageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticNavigationDataAsync(Navigation navigationContent)
        {
            dynamic result = null;
            EnsureNotNullLocation(navigationContent);

            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, navigationContent.Name, navigationContent.Location.FirstOrDefault());
            if (pageDBData?.Count > 0)
            {
                string id = pageDBData[0].id;
                navigationContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(navigationContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(navigationContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticAboutPageDataAsync(AboutContent aboutPageContent)
        {
            dynamic result = null;
            EnsureNotNullLocation(aboutPageContent);

            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, aboutPageContent.Name, aboutPageContent.Location.FirstOrDefault());
            if (pageDBData?.Count > 0)
            {
                string id = pageDBData[0].id;
                aboutPageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(aboutPageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(aboutPageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticPersnalizedPlanPageDataAsync(PersonalizedPlanContent personalizedPlanContent)
        {
            dynamic result = null;
            EnsureNotNullLocation(personalizedPlanContent);

            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, personalizedPlanContent.Name, personalizedPlanContent.Location.FirstOrDefault());
            if (pageDBData?.Count > 0)
            {
                string id = pageDBData[0].id;
                personalizedPlanContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(personalizedPlanContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(personalizedPlanContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result;
        }
        public async Task<dynamic> UpsertStaticGuidedAssistantPageDataAsync(GuidedAssistantPageContent guidedAssistantPageContent)
        {
            dynamic result = null;
            EnsureNotNullLocation(guidedAssistantPageContent);

            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourcesCollectionId, Constants.Name, guidedAssistantPageContent.Name, guidedAssistantPageContent.Location.FirstOrDefault());
            if (pageDBData?.Count > 0)
            {
                string id = pageDBData[0].id;
                guidedAssistantPageContent.Id = id;
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(guidedAssistantPageContent);
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            else
            {
                var pageDocument = JsonUtilities.DeserializeDynamicObject<object>(guidedAssistantPageContent);
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourcesCollectionId);
            }
            return result; 
        }

        private static void EnsureNotNullLocation(NameLocation staticResource)
        {
            if (staticResource?.Location != null &&
                staticResource.Location.Count > 0)
            {
                staticResource.Location =
                    staticResource.
                        Location.
                        Select(LocationUtilities.GetNotNullLocation).
                        ToList();
            }
        }

        private static dynamic DeserializeStaticContentData(dynamic staticContent)
        {
            var name = (string)staticContent.name;
            switch(name)
            {
                case Constants.StaticResourceTypes.AboutPage:
                    return JsonUtilities.DeserializeDynamicObject<AboutContent>(staticContent);
                case Constants.StaticResourceTypes.GuidedAssistantPrivacyPage:
                    return JsonUtilities.DeserializeDynamicObject<GuidedAssistantPageContent>(staticContent);
                case Constants.StaticResourceTypes.HelpAndFAQPage:
                    return JsonUtilities.DeserializeDynamicObject<HelpAndFaqsContent>(staticContent);
                case Constants.StaticResourceTypes.HomePage:
                    return JsonUtilities.DeserializeDynamicObject<HomeContent>(staticContent);
                case Constants.StaticResourceTypes.Navigation:
                    return JsonUtilities.DeserializeDynamicObject<Navigation>(staticContent);
                case Constants.StaticResourceTypes.PersonalizedActionPlanPage:
                    return JsonUtilities.DeserializeDynamicObject<PersonalizedPlanContent>(staticContent);
                case Constants.StaticResourceTypes.PrivacyPromisePage:
                    return JsonUtilities.DeserializeDynamicObject<PrivacyPromiseContent>(staticContent);
                default:
                    throw new NotSupportedException($"Not supported type of static content - {name}");
            }
        }
    }
}