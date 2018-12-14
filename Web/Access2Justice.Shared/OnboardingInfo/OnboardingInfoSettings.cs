using System;
using System.Globalization;
using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared.Admin
{
    public class OnboardingInfoSettings : IOnboardingInfoSettings
    {
        public OnboardingInfoSettings(IConfiguration configuration, IConfiguration kvConfiguration)
        {
            try
            {
                if (configuration != null)
                {
                    IKeyVaultSettings kv = new Access2Justice.Shared.Utilities.KeyVaultSettings(kvConfiguration);
                    var kvSecret = kv.GetKeyVaultSecrets("EmailServiceSecretKey");
                    kvSecret.Wait();
                    Password = kvSecret.Result;
                }
                else
                {
                    Password = configuration.GetSection("Password").Value;
                }

                HostAddress = configuration.GetSection("HostAddress").Value;
                PortNumber = configuration.GetSection("PortNumber").Value;
                UserName = configuration.GetSection("UserName").Value;
                Password = configuration.GetSection("Password").Value;
                FromAddress = configuration.GetSection("FromAddress").Value;
                Subject = configuration.GetSection("Subject").Value;
                FallbackToAddress = configuration.GetSection("FallbackToAddress").Value;
                FallbackBodyMessage = configuration.GetSection("FallbackBodyMessage").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }
        public string HostAddress { get; set; }
        public string PortNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string Subject { get; set; }
        public string FallbackToAddress { get; set; }
        public string FallbackBodyMessage { get; set; }
    }
}