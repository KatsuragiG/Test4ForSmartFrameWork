using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Pure Quant portfolio input contract.
    /// </summary>
    public class PureQuantPortfolioInputContract
    {
        /// <summary>
        /// The name of portfolio.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of newsletter portfolio.
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// The name of the publisher.
        /// </summary>
        public PublisherTypes? PublisherType { get; set; }

        /// <summary>
        /// ID of the portfolio.
        /// </summary>
        public int PortfolioId { get; set; }
    }
}
