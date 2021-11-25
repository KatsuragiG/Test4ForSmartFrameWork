using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Fields required to generate alert state for Entry Signal trigger
    /// </summary>
    public class EntrySignalTriggerStateContract : BaseTriggerStateContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EntrySignalTriggerStateContract()
            : base(TriggerTypes.EntrySignal)
        {
        }

        /// <summary>
        /// The date when Early Entry Signal was triggered
        /// </summary>
        public DateTime ExtremumDate { get; set; }
    }
}
