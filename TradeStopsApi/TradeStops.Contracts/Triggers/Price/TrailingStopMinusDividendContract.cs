using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    // TODO: need implement in TradeStops or remove

    /// <summary>
    /// Contract for creating specific Trigger from Portfolio Tracker
    /// </summary>
    public class TrailingStopMinusDividendContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TrailingStopMinusDividendContract()
            : base(TriggerTypes.TrailingStopMinusDividend)
        {
        }

        /// <summary>
        /// Specific percent value.
        /// </summary>
        public decimal ThresholdValue { get; set; }
    }
}
