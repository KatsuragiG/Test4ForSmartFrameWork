using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract returns result of checking the possibility or completeness 
    /// of converting the portfolio to another currency.
    /// </summary>
    public class CheckPublishersPortfolioConversionResultContract
    {
        /// <summary>
        /// Conversion type.
        /// </summary>
        public PublishersPortfolioConversionTypes ConversionType { get; set; }
    }
}
