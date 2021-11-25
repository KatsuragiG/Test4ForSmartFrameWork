using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Naked Put Cost Basis Trigger that monitors the price of the Underlying Stock below or above the Cost Basis of the Option trade. Cost Basis equals the Strike Price minus the option's Entry Price per share.
    /// (For example, it monitors and notifies when the Latest Close is 25% below Cost Basis.)
    /// A Naked Put Cost Basis Trigger is available only for Short Put options.
    /// </summary>
    public class NakedPutTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NakedPutTriggerContract()
            : base(TriggerTypes.NakedPut)
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
    }
}
