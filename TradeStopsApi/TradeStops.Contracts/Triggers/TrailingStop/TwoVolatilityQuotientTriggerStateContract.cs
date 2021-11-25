using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Double VQ trigger
    /// </summary>
    public class TwoVolatilityQuotientTriggerStateContract : BaseTriggerStateContract
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TwoVolatilityQuotientTriggerStateContract()
            : base(TriggerTypes.TwoVolatilityQuotient)
        {
        }

        /// <summary>
        /// Current value - difference between latest close and entry price divided by VQ value
        /// </summary>
        public decimal CurrentValue { get; set; }

        /// <summary>
        /// Trade type of position (long, short)
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Price type that is being used in calculations
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Latest price
        /// </summary>
        public decimal LatestPrice { get; set; }

        /// <summary>
        /// Entry price of position
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Currency sign
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Indicates whether intraday price is used for calculations or not
        /// </summary>
        public bool UseIntraday { get; set; }
    }
}
