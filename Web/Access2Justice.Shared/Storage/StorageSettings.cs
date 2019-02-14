using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared.Storage
{
    public class StorageSettings : IStorageSettings
    {
        public StorageSettings(IConfiguration configuration)
        {
            StaticResourcesRootUrl = configuration.GetSection("StaticResourcesRootUrl").Value;
        }

        public string StaticResourcesRootUrl { get; private set; }
    }
}
