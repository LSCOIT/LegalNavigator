using Access2Justice.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using System;

namespace Access2Justice.Shared.QnAMaker
{
    public class QnAMakerSettings : IQnAMakerSettings
    {
        public QnAMakerSettings(IConfiguration configuration, ISecretsService secretsService)
        {
            try
            {
                AuthorizationKey = secretsService.GetSecret("QnAMakerAuthorizationKey");
                Endpoint = new Uri(configuration.GetSection("Endpoint").Value);
                KnowledgeId = configuration.GetSection("KnowledgeId").Value;
            }
            catch
            {
                throw new Exception("Invalid Application configurations");
            }
        }

        public Uri Endpoint { get; set; }
        public string KnowledgeId { get; set; }
        public string AuthorizationKey { get; set; }
    }
}
