using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration;
using Access2Justice.Shared.Interfaces;

namespace Access2Justice.Shared.Interfaces
{
    public interface IKeyVaultSettings
    {
        Task<string> GetKeyVaultSecrets(string strSecretName);
        Task<string> GetToken(string authority, string resource, string scope);
        string KeyVaultClientId { get;  }
        string KeyVaultClientSecret { get;  }
        System.Uri KeyVaultURL { get;  }


    }
}
