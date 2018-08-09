﻿using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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
        
        public async Task<dynamic> GetPageStaticResourceDataAsync(string name, Location location)
        {         
            return await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourceCollectionId, Constants.Name, name, location);
        }
        
        public async Task<dynamic> UpsertStaticHomePageDataAsync(HomeContent homePageContent, Location location)
        {
            var serializedResult = JsonConvert.SerializeObject(homePageContent);
            var pageDocument = JsonConvert.DeserializeObject(serializedResult);
            string name = homePageContent.Name;
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourceCollectionId, Constants.Name, name, location);
            if (pageDBData.Count == 0)
            {
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourceCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourceCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticPrivacyPromisePageDataAsync(PrivacyPromiseContent privacyPromisePageContent, Location location)
        {
            var serializedResult = JsonConvert.SerializeObject(privacyPromisePageContent);
            var pageDocument = JsonConvert.DeserializeObject(serializedResult);
            string name = privacyPromisePageContent.Name;
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourceCollectionId, Constants.Name, name, location);
            if (pageDBData.Count == 0)
            {
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourceCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourceCollectionId);
            }
            return result;
        }

        public async Task<dynamic> UpsertStaticHelpAndFAQPageDataAsync(HelpAndFaqsContent helpAndFAQPageContent, Location location)
        {
            var serializedResult = JsonConvert.SerializeObject(helpAndFAQPageContent);
            var pageDocument = JsonConvert.DeserializeObject(serializedResult);
            string name = helpAndFAQPageContent.Name;
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourceCollectionId, Constants.Name, name, location);
            if (pageDBData.Count == 0)
            {
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourceCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourceCollectionId);
            }
            return result;
        }
                
        public async Task<dynamic> UpsertStaticNavigationDataAsync(Navigation navigationContent, Location location)
        {
            var serializedResult = JsonConvert.SerializeObject(navigationContent);
            var pageDocument = JsonConvert.DeserializeObject(serializedResult);
            string name = navigationContent.Name;
            dynamic result = null;
            var pageDBData = await dbClient.FindItemsWhereWithLocationAsync(dbSettings.StaticResourceCollectionId, Constants.Name, name, location);
            if (pageDBData.Count == 0)
            {
                result = await dbService.CreateItemAsync(pageDocument, dbSettings.StaticResourceCollectionId);
            }
            else
            {
                string id = pageDBData[0].id;
                result = await dbService.UpdateItemAsync(id, pageDocument, dbSettings.StaticResourceCollectionId);
            }
            return result;
        }
    }
}