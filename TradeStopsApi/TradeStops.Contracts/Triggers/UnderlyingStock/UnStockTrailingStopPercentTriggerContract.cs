using System;

using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Percentage Trailing Stop Trigger that monitors the percent below the highest point of closing profitability since the entry date of the Underlying Stock.
    /// </summary>
    public class UnStockTrailingStopPercentTriggerContract : BaseTriggerContract, ITrailingStopPercentTrigger
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnStockTrailingStopPercentTriggerContract()
            : base(TriggerTypes.UnStockTrailingStopPercent)
        {
        }

        /// <summary>
        /// (optional) The Position Trigger start date is used as a start point for the trigger calculation and could be specified by the user. If the field is skipped, the Position Trigger start date will be taken from position Entry Date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Сustom percent value.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }
    }
}
