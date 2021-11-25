#if NET47
using System;
using System.Configuration;

namespace TradeStops.WebApi.Client.Configuration
{
    public class ClientSettings : ConfigurationSection, IClientSettings
    {
        [ConfigurationProperty(nameof(BaseAddress), IsRequired = true)]
        public string BaseAddress => (string)base[nameof(BaseAddress)];

        [ConfigurationProperty(nameof(LicenseKey), IsRequired = true)]
        public string LicenseKey => (string)base[nameof(LicenseKey)];

        [ConfigurationProperty(nameof(TimeoutInSeconds), IsRequired = true)]
        public int TimeoutInSeconds => (int)base[nameof(TimeoutInSeconds)];

        [ConfigurationProperty(nameof(ApiTokenExpirationInSeconds), IsRequired = false)]
        public int ApiTokenExpirationInSeconds => (int)base[nameof(ApiTokenExpirationInSeconds)];

        [ConfigurationProperty(nameof(ApiTokenEncryptionKey), IsRequired = false)]
        public string ApiTokenEncryptionKey => (string)base[nameof(ApiTokenEncryptionKey)];
    }
}
#endif