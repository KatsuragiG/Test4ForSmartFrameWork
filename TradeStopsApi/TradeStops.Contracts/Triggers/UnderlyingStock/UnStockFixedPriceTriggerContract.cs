using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Fixed Price Above/Below on Underlying Stock Trigger that monitors a specific price on Underlying Stock.
    /// </summary>
    public class UnStockFixedPriceTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnStockFixedPriceTriggerContract()
            : base(TriggerTypes.UnStockFixedPrice)
        {
        }

        /// <summary>
        ///  Trigger price type. The only possible valid value is Close.
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Fixed price value.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        ///  Trigger operation types. Valid values are Less or Greater.
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }

        /// <summary>
        /// Determines if intraday prices has to be used.
        /// </summary>
        public bool UseIntraday { get; set; }
    }
}
