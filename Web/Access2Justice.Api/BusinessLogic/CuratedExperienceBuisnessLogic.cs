using Access2Justice.Api.Interfaces;
using Access2Justice.Api.ViewModels;
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

        public CuratedExperienceComponent GetComponent(CuratedExperience curatedExperience)
        {
            return GetComponent(curatedExperience, Guid.Empty);
        }

        public CuratedExperienceComponent SaveAndGetNextComponentXXX(CuratedExperience curatedExperience, Guid buttonId)
        {
            return null;
        }
        public CuratedExperienceComponentViewModel SaveAndGetNextComponentViewModel(CuratedExperience curatedExperience, Guid buttonId)
        {
            return null;
        }

        private CuratedExperienceComponent GetComponent(CuratedExperience curatedExperience, Guid componentId)
        {
            var dbCompoent = new CuratedExperienceComponent();
            if (componentId == default(Guid))
            {
                dbCompoent = curatedExperience.Components.First();
            }
            else
            {
                dbCompoent = curatedExperience.Components.Where(x => x.ComponentId == componentId).FirstOrDefault();
            }

            return dbCompoent; // MapComponentToComponentViewModel(dbCompoent, curatedExperience.CuratedExperienceId);
        }

        //private CuratedExperienceComponentViewModel MapComponentToComponentViewModel(CuratedExperienceComponent dbCompoent, Guid curatedExperienceId)
        //{
        //    var viewModel = (CuratedExperienceComponentViewModel)dbCompoent;
        //    viewModel.CuratedExperienceId = curatedExperienceId;
        //    viewModel.AnswersDocId = new Guid();

        //    return viewModel;
        //}

        public async Task SaveUserAnswer(CuratedExperienceComponentViewModel component)
        {
            var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(component.ComponentId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
        }

        public async Task<CuratedExperience> GetCuratedExperience(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
        }

        public CuratedExperienceComponent SaveAndGetNextComponent(CuratedExperience curatedExperience, Guid buttonId)
        {
            throw new NotImplementedException();
        }
    }
}
