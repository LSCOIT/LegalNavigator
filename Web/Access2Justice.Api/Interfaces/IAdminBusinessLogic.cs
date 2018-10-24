using Access2Justice.Api.ViewModels;
using Access2Justice.Shared;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Api.Interfaces
{
    public interface IAdminBusinessLogic
    {
        Task<object> UploadCuratedContentPackage(List<IFormFile> file);
    }
}
