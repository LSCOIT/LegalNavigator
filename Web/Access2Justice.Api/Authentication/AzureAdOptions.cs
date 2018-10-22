
namespace Access2Justice.Api.Authentication
{
    public class AzureAdOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Instance { get; set; }        
        public string TenantId { get; set; }
        public string TokenVersion { get; set; }
        public string UserClaimsUrl { get; set; }
    }
}
