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
    public class LocationBusinessLogic: ILocationBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IDynamicQueries dbClient;

        public LocationBusinessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService,
            IDynamicQueries dynamicQueries)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
            dbClient = dynamicQueries;
        }

        public async Task<List<StateCode>> GetStateCodes()
        {
            List<StateCode> stateCodes = null;
            List<StateCodeViewModel> stateCodeDetails = await dbClient.FindItemsWhereAsync(dbSettings.StateProvincesCollectionId, Constants.Type, "stateCode");
            StateCodeViewModel states = JsonUtilities.DeserializeDynamicObject<StateCodeViewModel>(stateCodeDetails.FirstOrDefault());
            stateCodes = states.StateCodes;
            if (!stateCodes.Any())
            {
                throw new Exception($"No state codes available");
            }
            return stateCodes;
        }
    }
}
