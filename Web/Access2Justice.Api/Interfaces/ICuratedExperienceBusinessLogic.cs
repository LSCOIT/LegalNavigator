using Access2Justice.Api.ViewModels;
using Access2Justice.Shared.Models.CuratedExperience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperience> GetCuratedExperience(Guid id);
        CuratedExperienceComponent GetComponent(CuratedExperience curatedExperience);
        CuratedExperienceComponent GetComponent(CuratedExperience curatedExperience, Guid componentId);
        CuratedExperienceComponent SaveAndGetNextComponent(CuratedExperience curatedExperience, Guid buttonId);
        CuratedExperienceComponentViewModel MapComponentToViewModelComponent(CuratedExperienceComponent dbComponent, Guid curatedExperienceId);
    }
}
