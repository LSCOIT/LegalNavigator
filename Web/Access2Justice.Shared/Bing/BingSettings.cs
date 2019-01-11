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
                PageResultsCount = Int16.Parse(configuration.GetSection("PageResultsCount").Value, CultureInfo.InvariantCulture);
                PageOffsetValue = Int16.Parse(configuration.GetSection("PageOffsetValue").Value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        public Uri BingSearchUrl { get; set; }

        public string SubscriptionKey { get; set; }

        public string CustomConfigId { get; set; }

        public Int16 PageResultsCount { get; set; }

        public Int16 PageOffsetValue { get; set; }
    }
}