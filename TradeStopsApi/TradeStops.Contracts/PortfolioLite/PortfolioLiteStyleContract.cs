namespace TradeStops.Contracts
{
    /// <summary>
    /// Url to css file with styles for current Portfolio Lite partner.
    /// </summary>
    public class PortfolioLiteStyleContract
    {
        /// <summary>
        /// Portfolio Lite partner id.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Url to css file with styles
        /// </summary>
        public string StyleUrl { get; set; }
    }
}
