using System;
using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Checklist result of boolean checks.
    /// </summary>
    public class ChecklistBoolCheckContract : ChecklistBaseCheckContract
    {
        /// <summary>
        /// Original saved check value
        /// </summary>
        public bool OriginalValue { get; set; }

        /// <summary>
        /// Dates of events of the check
        /// </summary>
        public List<DateTime> EventDates { get; set; }
    }
}
