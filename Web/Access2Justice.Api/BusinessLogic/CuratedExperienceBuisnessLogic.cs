using Access2Justice.Api.ViewModels;
using Access2Justice.CosmosDb.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models.CuratedExperience;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Access2Justice.Api.BusinessLogic
{
    public class CuratedExperienceBuisnessLogic : ICuratedExperienceBusinessLogic
    {
        private readonly ICosmosDbSettings dbSettings;
        private readonly IBackendDatabaseService dbService;

        public CuratedExperienceBuisnessLogic(ICosmosDbSettings cosmosDbSettings, IBackendDatabaseService backendDatabaseService)
        {
            dbSettings = cosmosDbSettings;
            dbService = backendDatabaseService;
        }

        public CuratedExperienceComponentViewModel GetComponent(CuratedExperience curatedExperience, Guid componentId)
        {
            var dbCompoent = new CuratedExperienceComponent();
            if(componentId == default(Guid))
            {
                dbCompoent = curatedExperience.Components.First();
            }
            else
            {
                dbCompoent = curatedExperience.Components.Where(x => x.ComponentId == componentId).FirstOrDefault();
            }

            return MapComponentToComponentViewModel(dbCompoent, curatedExperience.CuratedExperienceId);
        }

        public CuratedExperienceComponentViewModel SaveAndGetNextComponent(CuratedExperience curatedExperience, Guid buttonId)
        {
            return null;
        }
        private CuratedExperienceComponentViewModel MapComponentToComponentViewModel(CuratedExperienceComponent dbCompoent, Guid curatedExperienceId)
        {
            var viewModel = (CuratedExperienceComponentViewModel)dbCompoent;
            viewModel.CuratedExperienceId = curatedExperienceId;
            viewModel.AnswersDocId = new Guid();

            return viewModel;
        }

        public async Task SaveUserAnswer(CuratedExperienceComponentViewModel compoent)
        {
            var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(compoent.ComponentId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
        }

        public async Task<CuratedExperience> GetCuratedExperience(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
        }

        CuratedExperienceComponent ICuratedExperienceBusinessLogic.GetComponent(CuratedExperience curatedExperience, Guid buttonId)
        {
            throw new NotImplementedException();
        }

        CuratedExperienceComponent ICuratedExperienceBusinessLogic.SaveAndGetNextComponent(CuratedExperience curatedExperience, Guid buttonId)
        {
            throw new NotImplementedException();
        }
    }
}
