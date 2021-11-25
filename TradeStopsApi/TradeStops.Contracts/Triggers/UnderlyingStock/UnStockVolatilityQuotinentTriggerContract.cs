using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Percentage Trailing Stop Trigger that monitors the percent below the highest point of closing profitability since the entry date of the position using Volatility Quotient of Underlying Stock automatically calculated by system.
    /// </summary>
    public class UnStockVolatilityQuotinentTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnStockVolatilityQuotinentTriggerContract()
            : base(TriggerTypes.UnStockVolatilityQuotinent)
        {
        }

        /// <summary>
        /// (optional) The Position Trigger start date is used as a start point for the trigger calculation and could be specified by the user. If the field is skipped, the Position Trigger start date will be taken from position Entry Date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///  Position trade type. 
        /// </summary>
        public TradeTypes TradeType { get; set; }
    }
}
