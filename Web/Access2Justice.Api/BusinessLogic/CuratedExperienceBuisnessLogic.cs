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

        public CuratedExperienceComponentViewModel SaveAndGetNextComponentViewModel(CuratedExperience curatedExperience, Guid buttonId)
        {
            return null;
        }

        public async Task SaveAnswers(CuratedExperienceComponentViewModel component)
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

        public CuratedExperienceComponentViewModel MapComponentToViewModelComponent(CuratedExperienceComponent dbComponent, Guid curatedExperienceId)
        {
            return new CuratedExperienceComponentViewModel
            {
                CuratedExperienceId = curatedExperienceId,
                ComponentId = dbComponent.ComponentId,
                AnswersDocId = Guid.NewGuid(),
                Name = dbComponent.Name,
                Text = dbComponent.Text,
                Help = dbComponent.Help,
                Learn = dbComponent.Learn,
                Tags = dbComponent.Tags,
                Buttons = dbComponent.Buttons,
                Fields = dbComponent.Fields,
            };
        }

        private CuratedExperienceComponent GetComponent(CuratedExperience curatedExperience, Guid componentId)
        {
            if (componentId == default(Guid))
            {
                return curatedExperience.Components.First();
            }
            else
            {
                return curatedExperience.Components.Where(x => x.ComponentId == componentId).FirstOrDefault();
            }
        }
    }
}
