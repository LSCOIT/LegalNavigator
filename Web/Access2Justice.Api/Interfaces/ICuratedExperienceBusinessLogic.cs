using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperience> GetCuratedExperienceAsync(Guid id);
        Task<IEnumerable<CuratedExperienceViewModel>> GetCuratedExperiencesAsync(string location);
        Task<CuratedExperienceComponentViewModel> GetComponent(CuratedExperience curatedExperience, Guid componentId);
        Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component, Document answers);
        Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswers answers, bool fillAnswer = false);
        Task<Guid> GetUserAnswerId(string userId);
        Task<CuratedExperienceAnswers> GetLastAnswerProgress(Guid curatedExperienceId, params Guid[] answerIds);

        Task<Document> SaveAnswersAsync(CuratedExperienceAnswersViewModel component, CuratedExperience curatedExperience, string userId = null);
        CuratedExperienceAnswers MapCuratedExperienceViewModel(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience);

        Task<bool> AnswersStepNext(CuratedExperienceAnswers answers);
        Task<bool> AnswersStepBack(CuratedExperienceAnswers answers);
        Task DeleteCuratedExperienceAsync(string id, string partitionKey = null);
    }
}
