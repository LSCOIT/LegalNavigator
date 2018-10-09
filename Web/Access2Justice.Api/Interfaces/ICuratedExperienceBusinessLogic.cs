using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models;
using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperience> GetCuratedExperience(Guid id);
        CuratedExperienceComponentViewModel GetComponent(CuratedExperience curatedExperience, Guid componentId);
        CuratedExperienceComponentViewModel GetNextComponent(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component);
        Task<Document> SaveAnswers(CuratedExperienceAnswersViewModel component, CuratedExperience curatedExperience);
        CuratedExperienceAnswers MapViewModel(CuratedExperienceAnswersViewModel viewModelAnswer,
            CuratedExperience curatedExperience);
    }
}
