using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for editing a custom symbol
    /// </summary>
    public class EditPublishersCustomSymbolContract
    {
        /// <summary>
        /// Custom symbol id of editing symbol 
        /// </summary>
        public int CustomSymbolId { get; set; }

        /// <summary>
        /// (optional) New custom symbol name
        /// </summary>
        public Optional<string> Name { get; set; }

        /// <summary>
        /// (optional) New custom symbol description
        /// </summary>
        public Optional<string> Description { get; set; }

        /// <summary>
        /// (optional) Custom symbol visibility
        /// </summary>
        public Optional<bool> Visibility { get; set; }

        /// <summary>
        /// (optional) Custom symbol currency type
        /// </summary>
        public Optional<PublishersCurrencyTypes> CurrencyType { get; set; }

        /// <summary>
        /// (optional) New custom symbol exchange market
        /// </summary>
        public Optional<string> ExchangeMarket { get; set; }
    }
}
