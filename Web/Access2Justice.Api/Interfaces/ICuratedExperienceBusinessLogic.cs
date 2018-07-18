using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models.CuratedExperience;
using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperienceTree> GetCuratedExperience(Guid id);
        CuratedExperienceComponentViewModel GetComponent(CuratedExperienceTree curatedExperience, Guid componentId);
        CuratedExperienceComponentViewModel GetNextComponent(CuratedExperienceTree curatedExperience, CuratedExperienceAnswersViewModel component);
        Task<Document> SaveAnswers(CuratedExperienceAnswersViewModel component);
    }
}
