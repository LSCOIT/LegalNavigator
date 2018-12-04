using Access2Justice.Shared.Interfaces;
using Access2Justice.Shared.Utilities;
using Microsoft.Extensions.Configuration;
using System;

namespace Access2Justice.Integration.Models
{
    public class RtmSettings 
    {
        public string TopicName { get; set; }

        public string ApiKey { get; set; }

        public Uri SessionURL { get; set; }

        public Uri ServiceProviderURL { get; set; }

        public Uri ServiceProviderDetailURL { get; set; }
    }
}