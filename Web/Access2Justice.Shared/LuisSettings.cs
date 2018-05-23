namespace Access2Justice.Shared
{
    using Microsoft.Extensions.Configuration;
    using System;

    public class LuisSettings : ILuisSettings
    {
        public LuisSettings(IConfiguration configuration)
        {
            try
            {
                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                TopIntentsCount = configuration.GetSection("TopIntentsCount").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        
        public Uri Endpoint { get; set; }
        public string TopIntentsCount { get; set; }
    }
}
