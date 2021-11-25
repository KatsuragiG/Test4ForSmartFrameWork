using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Fields required to generate alert state for Early Entry Signal trigger
    /// </summary>
    public class EarlyEntrySignalTriggerStateContract : BaseTriggerStateContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EarlyEntrySignalTriggerStateContract()
            : base(TriggerTypes.EarlyEntrySignal)
        {
        }

        /// <summary>
        /// The date when Early Entry Signal was triggered
        /// </summary>
        public DateTime ExtremumDate { get; set; }
    }
}
