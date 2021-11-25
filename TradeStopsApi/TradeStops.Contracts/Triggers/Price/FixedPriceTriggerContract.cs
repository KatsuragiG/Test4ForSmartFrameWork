using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Fixed Price Position Trigger for monitoring a specific price (for example, monitor and notify when the Close price is above $87.00).
    /// </summary>
    public class FixedPriceTriggerContract : BaseTriggerContract, ITriggerWithIntraday
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FixedPriceTriggerContract()
            : base(TriggerTypes.FixedPrice)
        {
        }

        /// <summary>
        /// Type of the price for setting up a Position Trigger. See Appendix A for the list of the available PriceTypes values.
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Price value for triggering the alert.
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
