using Access2Justice.Shared.Interfaces;
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
        
        public async Task<dynamic> GetPageStaticResourceDataAsync(string name)
        {
            return await dbClient.FindItemsWhereAsync(dbSettings.StaticResourceCollectionId, Constants.Name, name);
        }

        //public async Task<int> CreateStaticResourceDataAsync(StaticResource StaticResource)
        //{
        //    StaticResource resultStaticResource = await GetStaticResourceDataAsync(StaticResource.OId);
        //    if (!(resultStaticResource.OId == StaticResource.OId))
        //    {
        //        var result = await dbService.CreateItemAsync(ResourceDeserialized(StaticResource), dbSettings.StaticResourceCollectionId);
        //        if (result.ToString().Contains(StaticResource.OId)) return 1;
        //    }
        //    return 0;
        //}
        //public async Task<int> UpdateStaticResourceDataAsync(StaticResource StaticResource, string userIdGuid)
        //{
        //    var resultUP = GetStaticResourceDataAsync(StaticResource.OId);
        //    var StaticResourceObjects = JsonConvert.SerializeObject(resultUP);

        //    if (StaticResourceObjects.Contains(StaticResource.OId)) // condition to verify oId and update the details
        //    {
        //        StaticResource.Id = userIdGuid; // guid id of the document

        //        var result = await dbService.UpdateItemAsync(userIdGuid, ResourceDeserialized(StaticResource), dbSettings.StaticResourceCollectionId);
        //        if (result.ToString().Contains(StaticResource.OId)) return 1;
        //    }
        //    return 0;
        //}
        //private StaticResource ConvertStaticResource(dynamic convObj)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(convObj);
        //    List<StaticResource> listStaticResources = JsonConvert.DeserializeObject<List<StaticResource>>(serializedResult);
        //    StaticResource StaticResource = new StaticResource();
        //    foreach (StaticResource user in listStaticResources)
        //    {
        //        StaticResource.Id = user.Id;
        //        StaticResource.OId = user.OId;
        //        StaticResource.FirstName = user.FirstName;
        //        StaticResource.LastName = user.LastName;
        //        StaticResource.EMail = user.EMail;
        //        StaticResource.IsActive = user.IsActive;
        //        StaticResource.CreatedBy = user.CreatedBy;
        //        StaticResource.CreatedTimeStamp = user.CreatedTimeStamp;
        //        StaticResource.ModifiedBy = user.ModifiedBy;
        //        StaticResource.ModifiedTimeStamp = user.ModifiedTimeStamp;
        //    }
        //    return StaticResource;
        //}
        //private object ResourceDeserialized(StaticResource StaticResource)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(StaticResource);
        //    return JsonConvert.DeserializeObject<object>(serializedResult);
        //}
        //private object ResourceDynamicDeserialized(dynamic StaticResource)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(StaticResource);
        //    return JsonConvert.DeserializeObject<object>(serializedResult);
        //}
        //public async Task<dynamic> UpsertUserPersonalizedPlanAsync(dynamic userData)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userData);
        //    var userDocument = JsonConvert.DeserializeObject(serializedResult);
        //    string oId = userDocument.oId;
        //    dynamic result = null;
        //    if (userData.type == "plans")
        //    {
        //        string planId = userDocument.planId;
        //        List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
        //        List<string> values = new List<string>() { oId, planId };
        //        var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
        //        if (userDBData.Count == 0)
        //        {
        //            result = CreateUserPersonalizedPlanAsync(userData);
        //        }
        //        else
        //        {
        //            string id = userDBData[0].id;
        //            result = UpdateUserPersonalizedPlanAsync(id, userData);
        //        }
        //    }
        //    else if (userData.type == "resources")
        //    {
        //        string type = userData.type;
        //        List<string> resourcesPropertyNames = new List<string>() { Constants.OId, Constants.Type };
        //        List<string> resourcesValues = new List<string>() { oId, type };
        //        var userResourcesDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, resourcesPropertyNames, resourcesValues);
        //        if (userResourcesDBData.Count == 0)
        //        {
        //            result = CreateUserSavedResourcesAsync(userData);
        //        }
        //        else
        //        {
        //            string id = userResourcesDBData[0].id;
        //            result = UpdateUserSavedResourcesAsync(id, userData);
        //        }
        //    }
        //    return result;
        //}

        //public async Task<dynamic> CreateUserPersonalizedPlanAsync(dynamic userData)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userData);
        //    var userDocument = JsonConvert.DeserializeObject(serializedResult);
        //    return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        //}

        //public async Task<dynamic> UpdateUserPersonalizedPlanAsync(string id, dynamic userUIData)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userUIData);
        //    var userUIDocument = JsonConvert.DeserializeObject(serializedResult);
        //    string oId = userUIDocument.oId;
        //    string planId = userUIDocument.planId;
        //    List<string> propertyNames = new List<string>() { Constants.OId, Constants.PlanId };
        //    List<string> values = new List<string>() { oId, planId };
        //    var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
        //    var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
        //    JObject dbObject = JObject.Parse(serializedDBResult);
        //    JObject uiObject = JObject.Parse(serializedResult);

        //    foreach (var prop in uiObject.Properties())
        //    {
        //        var targetProperty = dbObject.Property(prop.Name);
        //        if (targetProperty == null)
        //        {
        //            dbObject.Add(prop.Name, prop.Value);
        //        }
        //        else
        //        {
        //            targetProperty.Value = prop.Value;
        //        }
        //    }
        //    return await dbService.UpdateItemAsync(id, dbObject, dbSettings.ResourceCollectionId);
        //}

        //public async Task<dynamic> CreateUserSavedResourcesAsync(dynamic userResources)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userResources);
        //    var userDocument = JsonConvert.DeserializeObject(serializedResult);
        //    return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        //}

        //public async Task<dynamic> UpdateUserSavedResourcesAsync(string id, dynamic userResources)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userResources);
        //    var userUIDocument = JsonConvert.DeserializeObject(serializedResult);
        //    string oId = userUIDocument.oId;
        //    string type = userUIDocument.type;
        //    List<string> propertyNames = new List<string>() { Constants.OId, Constants.Type };
        //    List<string> values = new List<string>() { oId, type };
        //    var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
        //    var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
        //    JObject dbObject = JObject.Parse(serializedDBResult);
        //    JObject uiObject = JObject.Parse(serializedResult);

        //    foreach (var prop in uiObject.Properties())
        //    {
        //        var targetProperty = dbObject.Property(prop.Name);
        //        if (targetProperty == null)
        //        {
        //            dbObject.Add(prop.Name, prop.Value);
        //        }
        //        else
        //        {
        //            targetProperty.Value = prop.Value;
        //        }
        //    }
        //    return await dbService.UpdateItemAsync(id, dbObject, dbSettings.ResourceCollectionId);
        //}

        //public async Task<dynamic> UpsertUserPlanAsync(dynamic userPlan)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userPlan);
        //    var userDocument = JsonConvert.DeserializeObject(serializedResult);
        //    string oId = userDocument.oId;
        //    dynamic result = null;
        //    string id = userDocument.id;
        //    List<string> propertyNames = new List<string>() { Constants.OId, Constants.Id };
        //    List<string> values = new List<string>() { oId, id };
        //    var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
        //    if (userDBData.Count == 0)
        //    {
        //        result = CreateUserPlanAsync(userPlan);
        //    }
        //    else
        //    {
        //        result = UpdateUserPlanAsync(id, userPlan);
        //    }
        //    return result;
        //}

        //public async Task<dynamic> CreateUserPlanAsync(dynamic userData)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userData);
        //    var userDocument = JsonConvert.DeserializeObject(serializedResult);
        //    return await dbService.CreateItemAsync(userDocument, dbSettings.ResourceCollectionId);
        //}

        //public async Task<dynamic> UpdateUserPlanAsync(string id, dynamic userUIData)
        //{
        //    var serializedResult = JsonConvert.SerializeObject(userUIData);
        //    var userUIDocument = JsonConvert.DeserializeObject(serializedResult);
        //    string oId = userUIDocument.oId;
        //    List<string> propertyNames = new List<string>() { Constants.OId, Constants.Id };
        //    List<string> values = new List<string>() { oId, id };
        //    var userDBData = await dbClient.FindItemsWhereAsync(dbSettings.ResourceCollectionId, propertyNames, values);
        //    var serializedDBResult = JsonConvert.SerializeObject(userDBData[0]);
        //    JObject dbObject = JObject.Parse(serializedDBResult);
        //    JObject uiObject = JObject.Parse(serializedResult);

        //    foreach (var prop in uiObject.Properties())
        //    {
        //        var targetProperty = dbObject.Property(prop.Name);
        //        if (targetProperty == null)
        //        {
        //            dbObject.Add(prop.Name, prop.Value);
        //        }
        //        else
        //        {
        //            targetProperty.Value = prop.Value;
        //        }
        //    }
        //    return await dbService.UpdateItemAsync(id, dbObject, dbSettings.ResourceCollectionId);
        //}
    }
}