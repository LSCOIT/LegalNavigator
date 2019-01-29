using Access2Justice.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces.A2JAuthor
{
    public interface ICuratedExperienceConvertor
    {
        Task<CuratedExperience> ConvertA2JAuthorToCuratedExperienceAsync(JObject a2jSchema,bool isFromAdminImport = false);
    }
}
