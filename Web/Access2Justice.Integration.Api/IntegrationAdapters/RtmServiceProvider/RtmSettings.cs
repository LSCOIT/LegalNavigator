using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.IntegrationAdapters.RtmServiceProvider
{
    public class RtmSettings
    {
        public string ApiKey { get; set; }
        public Uri SessionURL { get; set; }
        public Uri ServiceProviderURL { get; set; }
        public Uri ServiceProviderDetailURL { get; set; }
    }
}
