using Access2Justice.CosmosDb.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Access2Justice.CosmosDb
{
    public class CosmosDbSettings : ICosmosDbSettings
    {
        public CosmosDbSettings(IConfiguration configuration)
        {
            try
            {
                AuthKey = configuration.GetSection("AuthKey").Value;
                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                DatabaseId = configuration.GetSection("DatabaseId").Value;
                CollectionId = configuration.GetSection("CollectionId").Value;
            }
            catch
            {
                throw new Exception("Invalid CosmosDB configurations");
            }
        }
        public string AuthKey { get; private set; }
        public Uri Endpoint { get; private set; }
        public string DatabaseId { get; private set; }
        public string CollectionId { get; private set; }
    }
}
