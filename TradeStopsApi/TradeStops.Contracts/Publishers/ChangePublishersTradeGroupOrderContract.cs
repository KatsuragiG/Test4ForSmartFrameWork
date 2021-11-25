using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for changing a trade group order
    /// </summary>
    public class ChangePublishersTradeGroupOrderContract
    {
        /// <summary>
        /// Trade group id for changing position
        /// </summary>
        public int DroppedTradeGroupId { get; set; }

        /// <summary>
        /// (optional) Previous trade group id to dropped trade group
        /// </summary>
        public Optional<int> PreviousTradeGroupId { get; set; }
    }
}
