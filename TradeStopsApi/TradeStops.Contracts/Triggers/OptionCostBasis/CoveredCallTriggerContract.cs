using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Covered Call Cost Basis Trigger that monitors the price of the Underlying Stock below or above the Cost Basis of the Option trade.
    /// Cost Basis equals the Entry Price of the purchased Stock minus the option's Entry Price per share.
    /// (For example, it monitors and notifies when the Latest Close is 25% below Cost Basis and I bought the Underlying Stock for $1.)
    /// A Covered Call Cost Basis Trigger is available only for Short Call options.
    /// </summary>
    public class CoveredCallTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CoveredCallTriggerContract()
            : base(TriggerTypes.CoveredCall)
        {
        }

        /// <summary>
        /// Trigger price type. The only possible valid value is Close.
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Percent value of the option Latest Close.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Trigger operation types. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }

        /// <summary>
        /// Price value of the Underlying Stock.
        /// </summary>
        public decimal StockPurchasePrice { get; set; }
    }
}
