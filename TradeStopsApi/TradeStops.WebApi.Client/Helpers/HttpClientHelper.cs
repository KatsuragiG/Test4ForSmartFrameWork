using System;
using System.Net.Http;
using TradeStops.WebApi.Client.Configuration;

namespace TradeStops.WebApi.Client.Helpers
{
    // todo: probably we don't need this wrapper after removing cryptostops api. consider to refactor
    /// <summary>
    /// Wrapper is used to combine TS and CS API with necessary HTTP client setup. It is intended to be used as a singleton
    /// </summary>
    public static class HttpClientHelper
    {
        private const string LicenseKeyHeader = "LicenseKey";

        private static HttpClient _tsHttpClient;

        // todo now: make sure clients never initialized twice (thread-safety is important)
        public static void Initialize(IClientSettings settings)
        {
            _tsHttpClient = CreateHttpClient(settings.BaseAddress, settings.TimeoutInSeconds, settings.LicenseKey);
        }

        public static HttpClient GetClient()
        {
            if (_tsHttpClient == null)
            {
                throw new Exception("HttpClient instance is not created. call HttpClientHelper.Initialize() on application startup to initialize");
            }

            return _tsHttpClient;
        }

        private static HttpClient CreateHttpClient(string baseAddress, int timeoutInSeconds, string licenseKey)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = TimeSpan.FromSeconds(timeoutInSeconds),
            };

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add(LicenseKeyHeader, licenseKey);

            return httpClient;
        }
    }
}
