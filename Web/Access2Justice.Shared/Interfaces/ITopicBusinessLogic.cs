using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Access2Justice.Shared.Interfaces
{
   public interface ITopicBusinessLogic
    {
        Task<T> GetTopicsAsync<T>();
        Task<dynamic> GetSubTopicsAsync(string id);
        Task<dynamic> GetSubTopicDetailAsync(string id);
    }
}
