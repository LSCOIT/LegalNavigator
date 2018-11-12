using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Access2Justice.Shared.Models;

namespace Access2Justice.Integration.Interfaces
{
    public interface ITopicAdapter
    {
        Task<IEnumerable<Topic>> GetTopics(string organizationalUnit);

        Topic GetTopicDetails(string id);
    }
}
