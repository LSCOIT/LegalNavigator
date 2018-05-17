namespace Access2Justice.Shared
{
    using Microsoft.Extensions.Configuration;
    using System;

    public class App : IApp
    {
        public App(IConfiguration configuration)
        {
            try
            {
                LuisUrl = new Uri(configuration.GetSection("LuisUrl").Value);
                TopIntentsCount = configuration.GetSection("TopIntentsCount").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        
        public Uri LuisUrl { get; set; }
        public string TopIntentsCount { get; set; }
    }
}
