using System;

namespace TradeStops.WebApi.Client.Configuration
{
    public class ManualClientSettings : IClientSettings
    {
        public string BaseAddress { get; set; }

        public string LicenseKey { get; set; }

        public int TimeoutInSeconds { get; set; }

        public int ApiTokenExpirationInSeconds { get; set; }

        public string ApiTokenEncryptionKey { get; set; }
    }
}
