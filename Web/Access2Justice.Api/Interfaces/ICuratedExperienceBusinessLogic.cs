using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Access2Justice.Api.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperience> GetCuratedExperienceAsync(Guid id);
        Task<CuratedExperienceComponentViewModel> GetComponent(CuratedExperience curatedExperience, Guid componentId);
        Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component);
        Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswers answers);
        Task<CuratedExperienceAnswers> GetAnswerProgress(string userId);

        Task<Document> SaveAnswersAsync(CuratedExperienceAnswersViewModel component, CuratedExperience curatedExperience, string userId = null);
        CuratedExperienceAnswers MapCuratedExperienceViewModel(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience);
    }
}
