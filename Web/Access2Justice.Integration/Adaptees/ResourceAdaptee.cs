using Access2Justice.Integration.Interfaces;
using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Models;
using Access2Justice.Shared.Models.Integration;
using Access2Justice.Shared.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Access2Justice.Integration.Adapters
{
    public class ResourceAdaptee
    {
        public async Task<IEnumerable<Resource>> GetResources(string organizationalUnit, Topic topic)
        {
            throw new NotImplementedException();
        }
    }
}

