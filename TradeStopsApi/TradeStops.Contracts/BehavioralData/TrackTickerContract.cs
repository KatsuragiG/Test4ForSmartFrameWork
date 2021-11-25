using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to track ticker
    /// </summary>
    public class TrackTickerContract
    {
        /// <summary>
        /// The unique Id of the symbol
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// The type of behavioral event
        /// </summary>
        public BehavioralEventTypes BehavioralEventTypeId { get; set; }

        /// <summary>
        /// ID of the product
        /// </summary>
        public Products ProductId { get; set; }
    }
}
