using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Adapters
{
    public class ResourceAdapter : IResourceAdapter
    {
        public Resource GetResourceDetails(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Resource>> GetResources(string organizationalUnit, Topic topic)
        {
            ResourceAdaptee resourceAdaptee = new ResourceAdaptee();
            return await resourceAdaptee.GetResources(organizationalUnit, null).ConfigureAwait(false);           
        }
    }

}

