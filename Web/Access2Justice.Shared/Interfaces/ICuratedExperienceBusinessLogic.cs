using Access2Justice.Shared.Models.CuratedExperience;
using System;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface ICuratedExperienceBusinessLogic
    {
        Task<CuratedExperience> GetCuratedExperience(Guid id);
        Component GetComponent(CuratedExperience curatedExperience, Guid buttonId);
    }
}
