using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface IA2JAuthorBusinessLogic
    {
        CuratedExperience ConvertA2JAuthorToCuratedExperience(JObject a2jSchema);
    }
}
