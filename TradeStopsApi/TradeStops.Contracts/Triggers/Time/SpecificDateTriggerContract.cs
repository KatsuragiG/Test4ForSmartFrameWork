using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Create a Specific Date Trigger that monitors a specific date.
    /// </summary>
    public class SpecificDateTriggerContract : BaseTriggerContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SpecificDateTriggerContract()
            : base(TriggerTypes.SpecificDate)
        {
        }

        /// <summary>
        /// Date when the alert will be triggered.
        /// </summary>
        public DateTime ThresholdDate { get; set; }
    }
}
