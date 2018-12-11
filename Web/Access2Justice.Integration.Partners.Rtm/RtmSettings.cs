using System;

namespace Access2Justice.Integration.Partners.Rtm
{
    public class RtmSettings
    {
        public string ApiKey { get; set; }

        public Uri SessionURL { get; set; }

        public Uri ServiceProviderURL { get; set; }

        public Uri ServiceProviderDetailURL { get; set; }
    }
}