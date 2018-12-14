using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Access2Justice.Integration.Api.IntegrationAdapters.RtmServiceProvider
{
    public class RtmSettings
    {
        public RtmSettings(IConfiguration configuration)
        {
            try
            {
                ApiKey = configuration.GetSection("ApiKey").Value;
                SessionURL = new Uri(configuration.GetSection("SessionURL").Value);
                ServiceProviderURL = new Uri(configuration.GetSection("ServiceProviderURL").Value);
                ServiceProviderDetailURL = new Uri(configuration.GetSection("ServiceProviderDetailURL").Value);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }

        public string ApiKey { get; set; }
        public Uri SessionURL { get; set; }
        public Uri ServiceProviderURL { get; set; }
        public Uri ServiceProviderDetailURL { get; set; }
    }
}
