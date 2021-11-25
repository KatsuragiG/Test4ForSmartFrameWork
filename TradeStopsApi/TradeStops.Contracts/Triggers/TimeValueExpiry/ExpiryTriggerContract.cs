using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Expiry Trigger that monitors a specific number of days before the option expires.
    /// </summary>
    public class ExpiryTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExpiryTriggerContract()
            : base(TriggerTypes.Expiry)
        {
        }

        /// <summary>
        /// Number of calendar days before expiry.
        /// </summary>
        public int ThresholdValue { get; set; }
    }
}
