using System;
using System.Collections.Generic;
using System.Text;
using Access2Justice.Shared.Models;

namespace Access2Justice.Integration.Interfaces
{
    public interface IResourceAdapter
    {
        IEnumerable<Resource> GetResources(string organizationalUnit, Topic topic);

        Resource GetResourceDetails(string id);
    }
}
