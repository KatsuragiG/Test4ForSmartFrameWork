using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Percentage Gain/Loss Trigger for monitoring a specific percent gain/loss (for example, monitor and notify when the High Price is 25% below the entry price).
    /// </summary>
    public class PercentageGainLossTriggerContract : BaseTriggerContract, ITriggerWithIntraday
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PercentageGainLossTriggerContract()
            : base(TriggerTypes.PercentageGainLoss)
        {
        }

        /// <summary>
        /// Type of the price for setting up a Position Trigger.
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Specific percent value compared to the entry price.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Type of the operation. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool UseIntraday { get; set; }
    }
}
