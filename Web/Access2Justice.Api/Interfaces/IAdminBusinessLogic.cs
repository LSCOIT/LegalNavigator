using Access2Justice.Shared;
using System.Threading.Tasks;
using Access2Justice.Shared.Admin;

namespace Access2Justice.Api.Interfaces
{
    public interface IAdminBusinessLogic
    {
        Task<JsonUploadResult> UploadCuratedContentPackage(CuratedTemplate curatedTemplate);
    }
}
