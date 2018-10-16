using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperience> GetCuratedExperienceAsync(Guid id);
        Task<CuratedExperienceComponentViewModel> GetComponentAsync(CuratedExperience curatedExperience, Guid componentId);
        Task<CuratedExperienceComponentViewModel> GetNextComponentAsync(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component);
        Task<Document> SaveAnswersAsync(CuratedExperienceAnswersViewModel component, CuratedExperience curatedExperience);
        Task<CuratedExperienceAnswers> MapViewModelAsync(CuratedExperienceAnswersViewModel viewModelAnswer, CuratedExperience curatedExperience);
    }
}
