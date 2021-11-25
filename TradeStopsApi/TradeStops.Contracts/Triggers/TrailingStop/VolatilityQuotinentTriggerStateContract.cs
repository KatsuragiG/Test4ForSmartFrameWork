using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class VolatilityQuotinentTriggerStateContract : BaseTriggerStateContract
    {
        /// <summary>
        /// constructor
        /// </summary>
        public VolatilityQuotinentTriggerStateContract()
            : base(TriggerTypes.VolatilityQuotinent)
        {
        }

        public decimal CurrentValue { get; set; }
        public TradeTypes TradeType { get; set; }
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// The max/min price for trading period (from start date to current date)
        /// </summary>
        public decimal ExtremumPrice { get; set; }

        /// <summary>
        /// The date when ExtremumPrice was recorded
        /// </summary>
        public DateTime ExtremumDate { get; set; }

        public decimal StopPrice { get; set; }

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
