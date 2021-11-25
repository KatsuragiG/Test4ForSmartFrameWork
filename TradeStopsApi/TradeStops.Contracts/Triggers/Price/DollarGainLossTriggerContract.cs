using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Dollar Gain/Loss Position Trigger that monitors a specific dollar amount above or below the entry price (for example, the High price per share is $10 above the entry price). 
    /// </summary>
    public class DollarGainLossTriggerContract : BaseTriggerContract, ITriggerWithIntraday
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DollarGainLossTriggerContract()
            : base(TriggerTypes.DollarGainLoss)
        {
        }

        /// <summary>
        /// Type of the price for setiing up a Position Trigger.
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Price value per share for triggering the alert.
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
