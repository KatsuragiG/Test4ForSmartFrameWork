using System;

using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Trailing Stop Percent Trigger based on the position entry date or a custom start date. Percentage trailing stops track the percent below the highest point of closing profitability since the entry date of the position. You can send a custom start price which will be used for setting up the highest Position Trigger price. 
    /// </summary>
    public class TrailingStopPercentTriggerContract : BaseTriggerContract, ITrailingStopPercentTrigger, ITriggerWithStartDate, ITriggerWithIntraday
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TrailingStopPercentTriggerContract()
            : base(TriggerTypes.TrailingStopPercent)
        {
        }

        /// <summary>
        /// Position Trigger creation date. If the field is skipped, the Position Trigger will be set up for the position Entry Date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// The percent value below the highest point of closing profitability since the start date.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool UseIntraday { get; set; }
    }
}
