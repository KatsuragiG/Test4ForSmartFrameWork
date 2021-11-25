using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Target Trigger that monitors a specific value of fundamental data:
    /// Market Cap; 
    /// Enterprise Value; 
    /// Enterprise Value/Revenue; 
    /// Enterprise Value/EBITDA; 
    /// Price/Book; 
    /// Price/Earnings; 
    /// PEG. 
    /// Trigger of this type is available only for positions with the fundamental data.
    /// </summary>
    public class TargetTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TargetTriggerContract()
            : base(TriggerTypes.Target)
        {
        }

        /// <summary>
        /// Value of fundamental data.
        /// </summary>
        public decimal ThresholdValue { get; set; }

        /// <summary>
        /// Trigger operation types. Valid values are Less or Greater .
        /// </summary>
        public TriggerOperationTypes OperationType { get; set; }

        /// <summary>
        /// Type of the  fundamental data.
        /// </summary>
        public TargetColumnNames TargetColumnName { get; set; }
    }
}
