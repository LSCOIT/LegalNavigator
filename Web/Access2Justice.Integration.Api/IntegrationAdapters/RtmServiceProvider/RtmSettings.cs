using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Access2Justice.Integration.Api.IntegrationAdapters.RtmServiceProvider
{
    public class RtmSettings
    {
        public RtmSettings(IConfiguration configuration, ISecretsService secretsService)
        {
            try
            {
                ApiKey = secretsService.GetSecret("IntegrationRtmApiKey");
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
