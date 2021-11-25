using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to update the Url to css file with styles for current Portfolio Lite partner.
    /// </summary>
    public class UpdatePortfolioLiteStyleContract
    {
        /// <summary>
        /// Portfolio Lite partner id.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Url to css file with styles
        /// </summary>
        public Optional<string> StyleUrl { get; set; }
    }
}
