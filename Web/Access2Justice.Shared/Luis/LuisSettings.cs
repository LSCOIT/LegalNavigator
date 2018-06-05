using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace Access2Justice.Shared.Luis
{
    public class LuisSettings : ILuisSettings
    {
        public LuisSettings(IConfiguration configuration)
        {
            try
            {
                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                TopIntentsCount = int.Parse(configuration.GetSection("TopIntentsCount").Value, CultureInfo.InvariantCulture);
                UpperThreshold = decimal.Parse(configuration.GetSection("UpperThreshold").Value, CultureInfo.InvariantCulture);
                LowerThreshold = decimal.Parse(configuration.GetSection("LowerThreshold").Value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        
        public Uri Endpoint { get; set; }
        public int TopIntentsCount { get; set; }
        public decimal UpperThreshold { get; set; }
        public decimal LowerThreshold { get; set; }
    }
}
