using System;
using System.Collections.Generic;
using System.Text;
using Access2Justice.Shared.Models;

namespace Access2Justice.Integration.Interfaces
{
    public interface ITopicAdapter
    {
        IEnumerable<Topic> GetTopics(string organizationalUnit);

        Topic GetTopicDetails(string id);
    }
}
