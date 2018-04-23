namespace Access2Justice.CosmosDbService
{
    public class CosmosDbConfigurations : ICosmosDbConfigurations
    {
        public string AuthKey { get; set; }
        public string Endpoint { get; set; }
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
    }
}
