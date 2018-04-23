namespace Access2Justice.CosmosDbService
{
    public interface ICosmosDbConfigurations
    {
        string AuthKey { get; set; }
        string Endpoint { get; set; }
        string DatabaseId { get; set; }
        string CollectionId { get; set; }
    }
}