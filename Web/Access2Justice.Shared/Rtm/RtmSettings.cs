using System;
using System.Globalization;
using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared.Rtm
{
    public class RtmSettings : IRtmSettings
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