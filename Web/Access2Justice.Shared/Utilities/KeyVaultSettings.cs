using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Access2Justice.Shared.Interfaces;


namespace Access2Justice.Shared.Utilities
{    
    public class KeyVaultSettings: IKeyVaultSettings
    {
        public KeyVaultSettings(IConfiguration configuration)
        {
            KeyVaultClientId = configuration.GetSection("KeyVaultClientId").Value;
            KeyVaultClientSecret = configuration.GetSection("KeyVaultClientSecret").Value;
            KeyVaultURL = new Uri(configuration.GetSection("KeyVaultEndPoint").Value);
        }
        public async Task<string> GetKeyVaultSecrets(string strSecretName)
        {
            var kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
            var kvSecretKey = await kvClient.GetSecretAsync(KeyVaultURL + strSecretName).ConfigureAwait(false);
            return kvSecretKey.Value;
        }

        public async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);          
            ClientCredential clientCred = new ClientCredential(KeyVaultClientId, KeyVaultClientSecret);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);
            if (result == null)
                throw new InvalidOperationException("Failed to obtain the KV JWT token");
            return result.AccessToken;
        }
        public string KeyVaultClientId { get; set; }
        public string KeyVaultClientSecret { get; set; }
        public System.Uri KeyVaultURL { get; set; }

    }
}
