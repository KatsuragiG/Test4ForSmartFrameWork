using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Custom symbol contract
    /// </summary>
    public class PublishersCustomSymbolContract
    {
        /// <summary>
        /// Custom symbol id
        /// </summary>
        public int CustomSymbolId { get; set; }

        /// <summary>
        /// Custom symbol name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Custom symbol description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Custom symbol visibility
        /// </summary>
        public bool Visibility { get; set; }

        /// <summary>
        /// Custom symbol currency type
        /// </summary>
        public PublishersCurrencyTypes CurrencyType { get; set; }

        /// <summary>
        /// Custom symbol exchange market
        /// </summary>
        public string ExchangeMarket { get; set; }

        /// <summary>
        /// Custom symbol customer id
        /// </summary>
        public int CustomerId { get; set; }
    }
}
