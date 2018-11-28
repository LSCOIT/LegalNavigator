using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.Extensions.Configuration;
using System;

namespace Access2Justice.Integration.Models
{
    public class RtmSettings : IRtmSettings
    {
        public RtmSettings(IConfiguration configuration, IConfiguration kvConfiguration)
        {
            try
            {
                if (kvConfiguration != null)
                {
                    IKeyVaultSettings kv = new KeyVaultSettings(kvConfiguration);
                    var kvSecret = kv.GetKeyVaultSecrets("RtmApiKey");
                    kvSecret.Wait();
                    ApiKey = kvSecret.Result;
                }
                else
                {
                    ApiKey = configuration.GetSection("ApiKey").Value;
                }
                SessionURL = new Uri(configuration.GetSection("SessionURL").Value);
                ServiceProviderURL = new Uri(configuration.GetSection("ServiceProviderURL").Value);
                ServiceProviderDetailURL = new Uri(configuration.GetSection("ServiceProviderDetailURL").Value);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }

        public string ApiKey { get; private set; }

        public Uri SessionURL { get; set; }

        public Uri ServiceProviderURL { get; set; }

        public Uri ServiceProviderDetailURL { get; set; }
    }
}