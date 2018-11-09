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
                RtmApiUrl = new Uri(configuration.GetSection("RtmApiUrl").Value);                
                RtmApiKey = configuration.GetSection("RtmApiKey").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        public Uri RtmApiUrl { get; set; }       

        public string RtmApiKey { get; set; }

    }
}