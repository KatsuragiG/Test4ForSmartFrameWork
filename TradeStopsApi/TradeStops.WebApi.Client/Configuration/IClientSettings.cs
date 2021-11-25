using System;

namespace TradeStops.WebApi.Client.Configuration
{
    public interface IClientSettings
    {
        string BaseAddress { get; }

        string LicenseKey { get; }

        int TimeoutInSeconds { get; }

        int ApiTokenExpirationInSeconds { get; }

        string ApiTokenEncryptionKey { get; }
    }
}