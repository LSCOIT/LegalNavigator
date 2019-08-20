using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace Access2Justice.Shared.Spot
{
    public class SpotSettings : ISpotSettings
    {
        public SpotSettings(IConfiguration configuration, ISecretsService secretsService)
        {
            try
            {
                ApiToken = secretsService.GetSecret("SpotApiToken");

                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                TopIntentsCount = int.Parse(configuration.GetSection("TopIntentsCount").Value, CultureInfo.InvariantCulture);
                IntentAccuracyThreshold = decimal.Parse(configuration.GetSection("IntentAccuracyThreshold").Value, CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }

        public string ApiToken { get; set; }
        public Uri Endpoint { get; set; }
        public int TopIntentsCount { get; set; }
        public decimal IntentAccuracyThreshold { get; set; }
    }
}
