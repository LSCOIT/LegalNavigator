using Microsoft.Extensions.Configuration;
using System;

namespace Access2Justice.Shared.Luis
{
    public class LuisSettings : ILuisSettings
    {
        public LuisSettings(IConfiguration configuration)
        {
            try
            {
                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                TopIntentsCount = configuration.GetSection("TopIntentsCount").Value;
                UpperThreshold = configuration.GetSection("UpperThreshold").Value;
                LowerThreshold = configuration.GetSection("LowerThreshold").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        
        public Uri Endpoint { get; set; }
        public string TopIntentsCount { get; set; }
        public string UpperThreshold { get; set; }
        public string LowerThreshold { get; set; }
    }
}
