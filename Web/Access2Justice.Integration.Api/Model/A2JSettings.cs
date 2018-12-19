using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Access2Justice.Integration.Api.Model
{
    public class A2JSettings : IA2JSettings
    {
        public A2JSettings(IConfiguration configuration)
        {
            try
            {
                ServiceProviderURL = new Uri(configuration.GetSection("ServiceProviderURL").Value);
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }

        public Uri ServiceProviderURL { get; set; }
    }
}
