using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about portfolio for which user tries to change or has changed currency.
    /// </summary>
    public class CheckPublishersPortfolioConversionContract
    {
        /// <summary>
        /// Portfolio Id.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Target currency.
        /// </summary>
        public PublishersCurrencyTypes? TargetCurrency { get; set; }
    }
}
