using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models.CuratedExperience;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperience> GetCuratedExperience(Guid id);
        CuratedExperienceComponentViewModel GetComponent(CuratedExperience curatedExperience, Guid componentId);
        CuratedExperienceComponentViewModel FindNextComponent(CuratedExperience curatedExperience, CuratedExperienceAnswersViewModel component);
        Task SaveAnswers(CuratedExperienceAnswersViewModel component);
    }
}
