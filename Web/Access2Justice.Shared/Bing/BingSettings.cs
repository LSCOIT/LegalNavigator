using System;
using System.Globalization;
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
                DefaultCount = Int16.Parse(configuration.GetSection("DefaultCount").Value, CultureInfo.InvariantCulture);
                DefaultOffset = Int16.Parse(configuration.GetSection("DefaultOffset").Value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        public Uri BingSearchUrl { get; set; }

        public string SubscriptionKey { get; set; }

        public string CustomConfigId { get; set; }

        public Int16 DefaultCount { get; set; }

        public Int16 DefaultOffset { get; set; }
    }
}