using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class StaticResourceBusinessLogicTests
    {
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IBackendDatabaseService backendDatabaseService;
        private readonly StaticResourceBusinessLogic staticResourceBusinessLogic;
        //Mocked input data
        private readonly JArray homePageData = StaticResourceTestData.homePageData;
        private readonly JArray emptyData = JArray.Parse(@"[{}]");
        Location location = new Location
        {
            State = "Hawaii"
        };
        //expected data
        private readonly string expectedPageName = "HomePage";
        
        public StaticResourceBusinessLogicTests()
        {
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            backendDatabaseService = Substitute.For<IBackendDatabaseService>();
            staticResourceBusinessLogic = new StaticResourceBusinessLogic(dynamicQueries, cosmosDbSettings, backendDatabaseService);
            cosmosDbSettings.AuthKey.Returns("dummykey");
            cosmosDbSettings.Endpoint.Returns(new System.Uri("https://bing.com"));
            cosmosDbSettings.DatabaseId.Returns("dbname");
            cosmosDbSettings.TopicsCollectionId.Returns("TopicCollection");
            cosmosDbSettings.ResourcesCollectionId.Returns("ResourceCollection");
            cosmosDbSettings.ProfilesCollectionId.Returns("UserProfile");
            cosmosDbSettings.StaticResourcesCollectionId.Returns("StaticResource");
        }

        [Fact]
        public void GetStaticResourceDataAsyncTestsShouldReturnProperData()
        {
            //arrange
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.StaticResourcesCollectionId, Constants.Name, location);
            dbResponse.ReturnsForAnyArgs<dynamic>(homePageData);

            //act
            var response = staticResourceBusinessLogic.GetPageStaticResourcesDataAsync(location);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.Contains(expectedPageName, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void GetStaticResourceDataAsyncShouldReturnEmptyData()
        {
            //arrange      
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.StaticResourcesCollectionId, Constants.Id, location);
            dbResponse.ReturnsForAnyArgs<dynamic>(emptyData);

            //act
            var response = staticResourceBusinessLogic.GetPageStaticResourcesDataAsync(location);
            string result = JsonConvert.SerializeObject(response);

            //assert
            Assert.DoesNotContain(expectedPageName, result, StringComparison.InvariantCultureIgnoreCase);
        }

        [Theory]
        [MemberData(nameof(StaticResourceTestData.UpsertNavigationContent), MemberType = typeof(StaticResourceTestData))]
        public void UpsertStaticNavigationDataAsyncShouldValidate(Navigation navigationInput, JArray navigationDBData, dynamic expectedResult)
        {
            var expectedName = "Navigation";
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.StaticResourcesCollectionId, Constants.Name, navigationInput.Name, new Location());
            dbResponse.ReturnsForAnyArgs(navigationDBData);
            
            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(StaticResourceTestData.updatedStaticNavigationContent));
            updatedDocument.LoadFrom(reader);

            backendDatabaseService.CreateItemAsync<dynamic>(
               Arg.Any<Navigation>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            backendDatabaseService.UpdateItemAsync<dynamic>(
               Arg.Any<string>(),
               Arg.Any<Navigation>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);
            
            //act
            var response = staticResourceBusinessLogic.UpsertStaticNavigationDataAsync(navigationInput);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Contains(expectedName, expectedResult);
        }

        [Theory]
        [MemberData(nameof(StaticResourceTestData.UpsertPrivacyPromiseContent), MemberType = typeof(StaticResourceTestData))]
        public void UpsertStaticPrivacyPromisePageDataAsyncShouldValidate(PrivacyPromiseContent privacyPromiseInput, JArray privacyPromiseDBData, dynamic expectedResult)
        {
            var expectedName = "PrivacyPromisePage";
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.StaticResourcesCollectionId, Constants.Name, privacyPromiseInput.Name, new Location());
            dbResponse.ReturnsForAnyArgs(privacyPromiseDBData);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(StaticResourceTestData.updatedStaticNavigationContent));
            updatedDocument.LoadFrom(reader);

            backendDatabaseService.CreateItemAsync<dynamic>(
               Arg.Any<PrivacyPromiseContent>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            backendDatabaseService.UpdateItemAsync<dynamic>(
               Arg.Any<string>(),
               Arg.Any<PrivacyPromiseContent>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = staticResourceBusinessLogic.UpsertStaticPrivacyPromisePageDataAsync(privacyPromiseInput);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Contains(expectedName, expectedResult);
        }

        [Theory]
        [MemberData(nameof(StaticResourceTestData.UpsertHelpAndFAQPageContent), MemberType = typeof(StaticResourceTestData))]
        public void UpsertStaticHelpAndFAQPageDataAsyncShouldValidate(HelpAndFaqsContent helpAndFaqsInput, JArray helpAndFaqsDBData, dynamic expectedResult)
        {
            var expectedName = "HelpAndFAQPage";
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.StaticResourcesCollectionId, Constants.Name, helpAndFaqsInput.Name, new Location());
            dbResponse.ReturnsForAnyArgs(helpAndFaqsDBData);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(StaticResourceTestData.updatedHelpAndFAQContent));
            updatedDocument.LoadFrom(reader);

            backendDatabaseService.CreateItemAsync<dynamic>(
               Arg.Any<HelpAndFaqsContent>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            backendDatabaseService.UpdateItemAsync<dynamic>(
               Arg.Any<string>(),
               Arg.Any<HelpAndFaqsContent>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = staticResourceBusinessLogic.UpsertStaticHelpAndFAQPageDataAsync(helpAndFaqsInput);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Contains(expectedName, expectedResult);
        }

        [Theory]
        [MemberData(nameof(StaticResourceTestData.UpsertAboutPageContent), MemberType = typeof(StaticResourceTestData))]
        public void UpsertStaticAboutPageDataAsyncShouldValidate(AboutContent aboutInput, JArray aboutDBData, dynamic expectedResult)
        {
            var expectedName = "AboutPage";
            var dbResponse = dynamicQueries.FindItemsWhereWithLocationAsync(cosmosDbSettings.StaticResourcesCollectionId, Constants.Name, aboutInput.Name, new Location());
            dbResponse.ReturnsForAnyArgs(aboutDBData);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(StaticResourceTestData.updatedHelpAndFAQContent));
            updatedDocument.LoadFrom(reader);

            backendDatabaseService.CreateItemAsync<dynamic>(
               Arg.Any<AboutContent>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            backendDatabaseService.UpdateItemAsync<dynamic>(
               Arg.Any<string>(),
               Arg.Any<AboutContent>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = staticResourceBusinessLogic.UpsertStaticAboutPageDataAsync(aboutInput);
            expectedResult = JsonConvert.SerializeObject(expectedResult);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            //assert
            Assert.Contains(expectedName, expectedResult);
        }
    }
}
