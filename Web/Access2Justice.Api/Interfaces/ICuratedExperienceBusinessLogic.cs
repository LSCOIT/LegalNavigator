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
        // Todo:@Alaa I think I'll make this a private method to the business logic. Still thinking about it.
        // CuratedExperienceComponent GetComponent(CuratedExperience curatedExperience, Guid buttonId);
        CuratedExperienceComponent SaveAndGetNextComponent(CuratedExperience curatedExperience, Guid buttonId);
    }
}
