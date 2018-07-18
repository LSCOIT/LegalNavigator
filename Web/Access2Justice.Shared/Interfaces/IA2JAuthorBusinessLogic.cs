using Access2Justice.Shared.Models.CuratedExperience;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Shared.Interfaces
{
    public interface IA2JAuthorBusinessLogic
    {
        CuratedExperienceTree ConvertA2JAuthorToCuratedExperience(JObject a2jSchema);
    }
}
