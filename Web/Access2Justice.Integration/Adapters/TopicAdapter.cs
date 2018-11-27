using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Adapters
{
    public class TopicAdapter : ITopicAdapter
    {
        public async Task<IEnumerable<Topic>> GetTopics(string organizationalUnit)
        {
            throw new NotImplementedException();
        }

        public Topic GetTopicDetails(string id)
        {
            throw new NotImplementedException();
        }
    }
}