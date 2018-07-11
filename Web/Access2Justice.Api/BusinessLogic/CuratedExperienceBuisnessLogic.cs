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
             // Todo:@Alaa add try/catch
            return curatedExperience.Components.First();
        }

        public CuratedExperienceComponent GetComponent(CuratedExperience curatedExperience, Guid componentId)
        {
            // Todo:@Alaa add try/catch
            return curatedExperience.Components.Where(x => x.ComponentId == componentId).FirstOrDefault();
        }

        public async Task SaveAnswers(CuratedExperienceAnswersViewModel component)
        {
             // Todo:@Alaa check if there is an answer file already
            var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(component.AnswersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
        }

        public async Task<CuratedExperience> GetCuratedExperience(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
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
    }
}
