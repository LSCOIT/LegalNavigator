using Access2Justice.Shared.Models;

namespace Access2Justice.Shared.Interfaces
{
    public interface ICuratedExperienceBuisnessLogic
    {
        CuratedExperience ConvertA2JAuthorToCuratedExperience(dynamic a2jSchema);
    }
}
