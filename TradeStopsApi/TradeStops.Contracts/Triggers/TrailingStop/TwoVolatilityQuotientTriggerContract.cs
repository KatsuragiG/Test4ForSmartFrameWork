using TradeStops.Common.Enums;
using TradeStops.Contracts.Interfaces;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Volatility Quotient Trigger based on the position entry date or a custom start date. Yuo can send a custom start price which will be used for setting up the highest Position Trigger price. 
    /// </summary>
    public class TwoVolatilityQuotientTriggerContract : BaseTriggerContract, ITriggerWithIntraday
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TwoVolatilityQuotientTriggerContract()
            : base(TriggerTypes.TwoVolatilityQuotient)
        {
        }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool UseIntraday { get; set; }
    }
}
