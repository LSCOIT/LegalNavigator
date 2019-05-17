﻿using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class CuratedExperienceBuisnessLogicTests
    {
        private readonly IBackendDatabaseService dbService;
        private readonly ICosmosDbSettings dbSettings;
        private readonly CuratedExperienceBuisnessLogic curatedExperience;
        private readonly IA2JAuthorLogicParser parser;
        private readonly IUserProfileBusinessLogic userProfileBusinessLogic;

        public CuratedExperienceBuisnessLogicTests()
        {
            dbService = Substitute.For<IBackendDatabaseService>();
            dbSettings = Substitute.For<ICosmosDbSettings>();
            parser = Substitute.For<IA2JAuthorLogicParser>();
            userProfileBusinessLogic = Substitute.For<IUserProfileBusinessLogic>();

            curatedExperience = new CuratedExperienceBuisnessLogic(dbSettings, dbService, parser, userProfileBusinessLogic);
            dbSettings.CuratedExperiencesCollectionId.Returns("CuratedExperiences");
            dbSettings.GuidedAssistantAnswersCollectionId.Returns("GuidedAssistantAnswers");
        }

        [Theory]
        [MemberData(nameof(CuratedExperienceTestData.CuratedExperienceData), MemberType = typeof(CuratedExperienceTestData))]
        public void GetCuratedExperienceAsyncValidate(CuratedExperience curatedExperiencedata, dynamic expectedData, Guid id)
        {
            //arrange            
            var dbResponse = dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperiencesCollectionId);
            dbResponse.ReturnsForAnyArgs<CuratedExperience>(curatedExperiencedata);
            var response = curatedExperience.GetCuratedExperienceAsync(id);

            //act
            var actualResult = JsonConvert.SerializeObject(response.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(CuratedExperienceTestData.CuratedExperienceComponentViewModelData), MemberType = typeof(CuratedExperienceTestData))]
        public void GetComponentValidate(CuratedExperience curatedExperiencedata, CuratedExperienceComponentViewModel expectedData, Guid id, dynamic curateExperienceAnswers)
        {
            //arrange
            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(curateExperienceAnswers));
            updatedDocument.LoadFrom(reader);

            dbService.CreateItemAsync<CuratedExperienceComponentViewModel>(
               Arg.Any<CuratedExperienceComponentViewModel>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            var response = curatedExperience.GetComponent(curatedExperiencedata, id);

            //act
            var actualResult = JsonConvert.SerializeObject(response.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(CuratedExperienceTestData.CuratedExperienceComponentViewModelDataWithDefaultComponentId), MemberType = typeof(CuratedExperienceTestData))]
        public void GetComponentValidateNewAnswerDoc(CuratedExperience curatedExperiencedata, CuratedExperienceComponentViewModel expectedData, Guid id, dynamic curateExperienceAnswers)
        {
            //arrange
            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(curateExperienceAnswers));
            updatedDocument.LoadFrom(reader);

            dbService.CreateItemAsync<CuratedExperienceComponentViewModel>(
               Arg.Any<CuratedExperienceComponentViewModel>(),
               Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = curatedExperience.GetComponent(curatedExperiencedata, id);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.DoesNotContain(id.ToString(), actualResult);
        }

        [Theory]
        [MemberData(nameof(CuratedExperienceTestData.NextComponentViewModelData), MemberType = typeof(CuratedExperienceTestData))]
        public void GetNextComponentAsyncValidate(CuratedExperience curatedExperiencedata, CuratedExperienceComponentViewModel expectedData, Guid answersDocId, CuratedExperienceAnswersViewModel component, CuratedExperienceAnswers curatedExperienceAnswers)
        {
            //arrange            
            var dbResponse = dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
            dbResponse.ReturnsForAnyArgs<CuratedExperienceAnswers>(curatedExperienceAnswers);

            //act
            var response = curatedExperience.GetNextComponentAsync(curatedExperiencedata, component);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(CuratedExperienceTestData.SaveAnswersData), MemberType = typeof(CuratedExperienceTestData))]
        public void SaveAnswersAsyncValidate(CuratedExperience curatedExperiencedata, dynamic expectedData, Guid answersDocId, CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperienceAnswers curatedExperienceAnswers, dynamic CuratedExperienceAnswersSchema)
        {
            //arrange 
            var dbResponse = dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
            dbResponse.ReturnsForAnyArgs<CuratedExperienceAnswers>(curatedExperienceAnswers);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(CuratedExperienceAnswersSchema));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<CuratedExperienceAnswersViewModel>(
            Arg.Any<string>(),
            Arg.Any<CuratedExperienceAnswersViewModel>(),
            Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            dbService.CreateItemAsync<CuratedExperienceAnswers>(
            Arg.Any<CuratedExperienceAnswers>(),
            Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = curatedExperience.SaveAnswersAsync(viewModelAnswer, curatedExperiencedata);
            var actualResult = JsonConvert.SerializeObject(response.Result);
            var expectedResult = JsonConvert.SerializeObject(expectedData);

            //assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [MemberData(nameof(CuratedExperienceTestData.SaveAnswersData2), MemberType = typeof(CuratedExperienceTestData))]
        public void SaveAnswersAsyncNoAnswersDocIdValidate(CuratedExperience curatedExperiencedata, dynamic expectedData, Guid answersDocId, CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperienceAnswers curatedExperienceAnswers, dynamic CuratedExperienceAnswersSchema)
        {
            //arrange 
            var dbResponse = dbService.GetItemAsync<CuratedExperienceAnswers>(answersDocId.ToString(), dbSettings.GuidedAssistantAnswersCollectionId);
            dbResponse.ReturnsForAnyArgs<CuratedExperienceAnswers>(curatedExperienceAnswers);

            Document updatedDocument = new Document();
            JsonTextReader reader = new JsonTextReader(new StringReader(CuratedExperienceAnswersSchema));
            updatedDocument.LoadFrom(reader);

            dbService.UpdateItemAsync<CuratedExperienceAnswersViewModel>(
            Arg.Any<string>(),
            Arg.Any<CuratedExperienceAnswersViewModel>(),
            Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            dbService.CreateItemAsync<CuratedExperienceAnswers>(
            Arg.Any<CuratedExperienceAnswers>(),
            Arg.Any<string>()).ReturnsForAnyArgs<Document>(updatedDocument);

            //act
            var response = curatedExperience.SaveAnswersAsync(viewModelAnswer, curatedExperiencedata);
            var actualResult = JsonConvert.SerializeObject(response.Result);

            //assert
            Assert.Contains(expectedData, actualResult);
        }
    }
}
