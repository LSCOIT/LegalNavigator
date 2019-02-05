using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace Access2Justice.Shared.Luis
{
    public class LuisSettings : ILuisSettings
    {
        public LuisSettings(IConfiguration configuration, ISecretsService secretsService)
        {
            try
            {
                Endpoint = new Uri(secretsService.GetSecret("LUISEndpoint"));

                TopIntentsCount = int.Parse(configuration.GetSection("TopIntentsCount").Value, CultureInfo.InvariantCulture);
                IntentAccuracyThreshold = decimal.Parse(configuration.GetSection("IntentAccuracyThreshold").Value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        
        public Uri Endpoint { get; set; }
        public int TopIntentsCount { get; set; }
        public decimal IntentAccuracyThreshold { get; set; }
    }
}
