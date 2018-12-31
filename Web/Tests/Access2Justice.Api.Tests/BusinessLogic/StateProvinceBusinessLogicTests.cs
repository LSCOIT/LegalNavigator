using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class StateProvinceBusinessLogicTests
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;
        private readonly IDynamicQueries dbClient;
        private readonly StateProvinceBusinessLogic stateProvinceBusinessLogic;

        public StateProvinceBusinessLogicTests()
        {
            dbSettings = Substitute.For<ICosmosDbSettings>();
            dbService = Substitute.For<IBackendDatabaseService>();
            dbClient = Substitute.For<IDynamicQueries>();

            stateProvinceBusinessLogic = new StateProvinceBusinessLogic(dbSettings, dbService, dbClient);
            dbSettings.StateProvincesCollectionId.Returns("StateProvinces");
        }

        [Theory]
        [MemberData(nameof(StateProvinceTestData.GetStateCodesTestData), MemberType = typeof(StateProvinceTestData))]
        public void GetStateCodesShouldValidate(JArray stateProvinceViewModel, dynamic expectedResult)
        {
            var dbResponse = dbClient.FindItemsWhereAsync(dbSettings.StateProvincesCollectionId, Constants.Type, Constants.StateProvinceType);
            dbResponse.ReturnsForAnyArgs(stateProvinceViewModel);

            //act
            var response = stateProvinceBusinessLogic.GetStateCodes();
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(StateProvinceTestData.GetStateNameTestData), MemberType = typeof(StateProvinceTestData))]
        public void GetStateCodeForStateShouldValidate(string stateName, JArray StateDetails, dynamic expectedResult)
        {
            var dbResponse = dbClient.FindFieldWhereArrayContainsAsync(dbSettings.StateProvincesCollectionId,
                Constants.StateProvince, Constants.Name, stateName);
            dbResponse.ReturnsForAnyArgs(StateDetails);

            //act
            var response = stateProvinceBusinessLogic.GetStateCodeForState(stateName);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(StateProvinceTestData.GetStateCodeTestData), MemberType = typeof(StateProvinceTestData))]
        public void GetStateNameForStateCodeShouldValidate(string stateCode, JArray StateDetails, dynamic expectedResult)
        {
            var dbResponse = dbClient.FindFieldWhereArrayContainsAsync(dbSettings.StateProvincesCollectionId,
                Constants.StateProvince, Constants.Code, stateCode);
            dbResponse.ReturnsForAnyArgs(StateDetails);

            //act
            var response = stateProvinceBusinessLogic.GetStateNameForStateCode(stateCode);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
