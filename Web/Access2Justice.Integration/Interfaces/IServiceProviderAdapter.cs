using System;
using System.Collections.Generic;
using System.Text;
using Access2Justice.Shared.Models;

namespace Access2Justice.Integration.Interfaces
{
    public interface IServiceProviderAdapter
    {
        IEnumerable<Organization> GetServiceProviders(string organizationalUnit, Topic topic);

        Organization GetServiceProviderDetails(string id);
    }
}
