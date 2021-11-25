using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Fields required to generate alert state for New High Profit trigger
    /// </summary>
    public class NewHighProfitTriggerStateContract : BaseTriggerStateContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NewHighProfitTriggerStateContract()
            : base(TriggerTypes.NewHighProfit)
        {
        }

        /// <summary>
        ///  The new high profit price 
        /// </summary>
        public decimal ExtremumPrice { get; set; }

        /// <summary>
        ///  The new high profit price currency sign
        /// </summary>
        public string Currency { get; set; }
    }
}
