using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Profitable Closes After Entry Trigger that monitors a specific number of profitable Closes/Opens. The alert will be triggered when the number of profitable Closes/Opens between the current Close Date and the position Entry Date will be equal to the specified value.
    /// </summary>
    public class ProfitableClosesTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfitableClosesTriggerContract()
            : base(TriggerTypes.ProfitableCloses)
        {
        }

        /// <summary>
        /// Trigger profitable type. Valid values are Open or Close .
        /// </summary>
        public PriceTypes PriceType { get; set; }

        /// <summary>
        /// Number of profitable closes or opens.
        /// </summary>
        public int ThresholdValue { get; set; }
    }
}
