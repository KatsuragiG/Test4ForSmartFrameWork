using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for creating a custom symbol
    /// </summary>
    public class CreatePublishersCustomSymbolContract
    {
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
    }
}
