using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
    public interface IWebSearchBusinessLogic
    {
        Task<dynamic> SearchWebResourcesAsync(string searchTerm);
    }
}
