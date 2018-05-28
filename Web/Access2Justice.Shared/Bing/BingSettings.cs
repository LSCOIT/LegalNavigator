using System;
using System.Collections.Generic;
using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared.Bing
{
    public class BingSettings : IBingSettings
    {
        public BingSettings(IConfiguration configuration)
        {
            try
            {
                BingSearchUrl = new Uri(configuration.GetSection("BingSearchUrl").Value);
                SubscriptionKey = configuration.GetSection("SubscriptionKey").Value;
                CustomConfigId = configuration.GetSection("CustomConfigId").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        public Uri BingSearchUrl { get; set; }

        public string SubscriptionKey { get; set; }

        public string CustomConfigId { get; set; }
    }
}
