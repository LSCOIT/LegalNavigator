using System;
using System.Globalization;
using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared.Share
{
    public class ShareSettings : IShareSettings
    {
        public ShareSettings(IConfiguration configuration)
        {
            try
            {
                PermaLinkMaxLength = Int16.Parse(configuration.GetSection("PermaLinkMaxLength").Value,CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        public int PermaLinkMaxLength { get; set; }
    }
}