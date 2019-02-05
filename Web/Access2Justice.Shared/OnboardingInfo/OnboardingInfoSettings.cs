using System;
using System.Globalization;
using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Access2Justice.Shared.Admin
{
    public class OnboardingInfoSettings : IOnboardingInfoSettings
    {
        public OnboardingInfoSettings(IConfiguration configuration, ISecretsService secretsService)
        {
            try
            {
                var getSecretTask = secretsService.GetSecretAsync("EmailServiceSecretKey");
                getSecretTask.Wait();
                Password = getSecretTask.Result;

                HostAddress = configuration.GetSection("HostAddress").Value;
                PortNumber = configuration.GetSection("PortNumber").Value;
                UserName = configuration.GetSection("UserName").Value;
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