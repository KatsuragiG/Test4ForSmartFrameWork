using System;

using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Volatility Quotient Trigger based on the position entry date or a custom start date. Yuo can send a custom start price which will be used for setting up the highest Position Trigger price. 
    /// </summary>
    public class VolatilityQuotinentTriggerContract : BaseTriggerContract, ITriggerWithStartDate, ITriggerWithIntraday
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VolatilityQuotinentTriggerContract()
            : base(TriggerTypes.VolatilityQuotinent)
        {
        }

        /// <summary>
        /// The Position Trigger will be created starting from the given date. If the field is skipped, the Position Trigger will be set up for the position Entry Date. </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool UseIntraday { get; set; }
    }
}
