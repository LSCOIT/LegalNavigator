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
            return curatedExperience.Components.Where(x => x.ComponentId == componentId).FirstOrDefault();
        }

        public async Task SaveAnswers(CuratedExperienceAnswersViewModel component)
        {
            // Todo: we could store the answers doc in the session and persist it to the db when the user
            // answers the last question. This will save us a trip to the database each time the user moves to
            // the next step. The caveat for this is that the users will need to repeat the survey from the
            // beginning if the session expires which might be frustrating.
            var savedAnswersDoc = await dbService.GetItemAsync<CuratedExperienceAnswers>(component.AnswersDocId.ToString(), dbSettings.CuratedExperienceAnswersCollectionId);
        }

        public async Task<CuratedExperience> GetCuratedExperience(Guid id)
        {
            return await dbService.GetItemAsync<CuratedExperience>(id.ToString(), dbSettings.CuratedExperienceCollectionId);
        }

        public CuratedExperienceComponentViewModel MapComponentToViewModelComponent(CuratedExperience curatedExperience, CuratedExperienceComponent dbComponent)
        {
            int stepRemained = 0;  // Todo:@Alaa calculated remaining steps
            return new CuratedExperienceComponentViewModel
            {
                CuratedExperienceId = curatedExperience.CuratedExperienceId,
                AnswersDocId = Guid.NewGuid(),
                StepsRemained = stepRemained,
                ComponentId = dbComponent.ComponentId,
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
