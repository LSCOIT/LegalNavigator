using Access2Justice.Shared.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IStaticResourceBusinessLogic
    {
        Task<dynamic> GetPageStaticResourceDataAsync(string name);
    }
}