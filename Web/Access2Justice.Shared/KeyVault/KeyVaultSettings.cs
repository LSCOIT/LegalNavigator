using System;
using Microsoft.Extensions.Configuration;
using Access2Justice.Shared.Interfaces;

namespace Access2Justice.Shared.KeyVault
{    
    public class KeyVaultSettings: IKeyVaultSettings
    {
        public KeyVaultSettings(IConfiguration configuration)
        {
            KeyVaultUrl = new Uri(configuration.GetSection("KeyVaultEndPoint").Value);
        }

        public Uri KeyVaultUrl { get; set; }
    }
}
