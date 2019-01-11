using Access2Justice.Api.BusinessLogic;
using Access2Justice.Api.Tests.TestData;
using Access2Justice.Shared;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Interfaces.A2JAuthor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Access2Justice.Api.Tests.BusinessLogic
{
    public class AdminBusinessLogicTests
    {
        private readonly IBackendDatabaseService dbService;
        private readonly AdminBusinessLogic adminBusiness;
        private readonly IDynamicQueries dynamicQueries;
        private readonly ICosmosDbSettings cosmosDbSettings;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ICuratedExperienceConvertor a2jAuthorBuisnessLogic;
        private readonly IAdminSettings adminSettings;
        private readonly ITopicsResourcesBusinessLogic topicsResourcesBusinessLogic;
        private readonly CuratedExperienceBuisnessLogic curatedExperience;
        public AdminBusinessLogicTests()
        {
            dynamicQueries = Substitute.For<IDynamicQueries>();
            cosmosDbSettings = Substitute.For<ICosmosDbSettings>();
            dbService = Substitute.For<IBackendDatabaseService>();
            hostingEnvironment = Substitute.For<IHostingEnvironment>();
            a2jAuthorBuisnessLogic = Substitute.For<ICuratedExperienceConvertor>();
            adminSettings = Substitute.For<IAdminSettings>();
            topicsResourcesBusinessLogic = Substitute.For<ITopicsResourcesBusinessLogic>();
            adminBusiness = new AdminBusinessLogic(dynamicQueries, cosmosDbSettings, dbService, hostingEnvironment, a2jAuthorBuisnessLogic, adminSettings, topicsResourcesBusinessLogic);
            cosmosDbSettings.CuratedExperiencesCollectionId.Returns("CuratedExperiences");
            cosmosDbSettings.GuidedAssistantAnswersCollectionId.Returns("GuidedAssistantAnswers");
            cosmosDbSettings.A2JAuthorDocsCollectionId.Returns("A2JAuthorDocs");
        }      
    }
}
