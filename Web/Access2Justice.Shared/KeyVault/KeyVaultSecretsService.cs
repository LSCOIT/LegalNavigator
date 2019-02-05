using Access2Justice.Shared.Interfaces;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;

namespace Access2Justice.Shared.KeyVault
{
    public class KeyVaultSecretsService : ISecretsService
    {
        private readonly IKeyVaultSettings _keyVaultSettings;
        private readonly AzureServiceTokenProvider _azureServiceTokenProvider;

        public KeyVaultSecretsService(IKeyVaultSettings keyVaultSettings)
        {
            _keyVaultSettings = keyVaultSettings;
            _azureServiceTokenProvider = new AzureServiceTokenProvider();
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var keyVaultClient = new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(_azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = await keyVaultClient.GetSecretAsync(
                _keyVaultSettings.KeyVaultUrl + secretName).ConfigureAwait(false);

            return secret.Value;
        }

        public string GetSecret(string secretName)
        {
            var getSecretTask = GetSecretAsync(secretName);
            getSecretTask.Wait();

            return getSecretTask.Result;
        }
    }
}
