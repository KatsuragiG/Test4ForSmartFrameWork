namespace TradeStops.Common.Utils
{
    public static class UrlUtils
    {
        /// <summary>
        /// Method is used to maintain single approach for combining urls in the code.
        /// </summary>
        /// <param name="baseUrl">First part of the url, like 'url.com/something/' or 'https://url.com'</param>
        /// <param name="urlPart">Second part of the url, like '/endpoint/' or 'endpoint' or 'endpoint/anything' </param>
        /// <returns>Combined url, like 'url.com/something/endpoint/' or 'https://url.com/endpoint'</returns>
        public static string Combine(string baseUrl, string urlPart)
        {
            var trimmedBaseUrl = baseUrl.TrimEnd('/');
            var trimmedUrlPart = urlPart.TrimStart('/');

            return $"{trimmedBaseUrl}/{trimmedUrlPart}";
        }
    }
}
