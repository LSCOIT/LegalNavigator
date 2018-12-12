using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.BusinessLogic
{
    public class StateProvinceBusinessLogic : IStateProvinceBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IDynamicQueries dbClient;

        public StateProvinceBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IDynamicQueries dynamicQueries)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            dbClient = dynamicQueries;
        }

        public async Task<List<StateCode>> GetStateCodes()
        {
            var stateProvinceDetails = await dbClient.FindItemsWhereAsync(dbSettings.StateProvincesCollectionId, Constants.Type, Constants.StateProvinceType);
            List<StateProvinceViewModel> stateProvince = JsonUtilities.DeserializeDynamicObject<List<StateProvinceViewModel>>(stateProvinceDetails);
            return stateProvince.Count() > 0 ? stateProvince[0].StateProvinces : null;
        }

        public async Task<string> GetStateCodeForState(string stateName)
        {
            var response = await dbClient.FindFieldWhereArrayContainsAsync(dbSettings.StateProvincesCollectionId,
                Constants.StateProvince, Constants.Name, stateName);
            List<StateCode> stateCodes = JsonUtilities.DeserializeDynamicObject<List<StateCode>>(response);            
            return stateCodes.Count() > 0 ? stateCodes[0].Code : null;
        }
    }
}
