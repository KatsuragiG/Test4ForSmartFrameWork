using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Breakout Position Trigger that monitors the new position's High or Low for a specified period of time (for example, monitor and notify when the Open price is a new 1 Week High).
    /// </summary>
    public class BreakoutTriggerContract : BaseTriggerContract, ITriggerWithIntraday
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BreakoutTriggerContract()
            : base(TriggerTypes.Breakout)
        {
        }

        /// <summary>
        /// Type of the price for setting up a Position Trigger.
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Numeric value of a specific period of time.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Type of the period.
        /// </summary>
        public PeriodTypes PeriodType { get; set; }

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
