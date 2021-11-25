using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    // TODO: need implement in TradeStops or remove

    /// <summary>
    /// Contract for displaying specific Trigger from Portfolio Tracker
    /// </summary>
    public class TrailingStopMinusDividendStateContract : BaseTriggerStateContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TrailingStopMinusDividendStateContract()
            : base(TriggerTypes.TrailingStopMinusDividend)
        {
        }

        /// <summary>
        /// Specific percent value.
        /// </summary>
        public decimal ThresholdValue { get; set; }
    }
}
