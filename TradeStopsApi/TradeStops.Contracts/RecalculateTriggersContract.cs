using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to force recalculate triggers
    /// </summary>
    public class RecalculateTriggersContract
    {
        /// <summary>
        /// IDs of position triggers
        /// </summary>
        public List<int> PositionTriggerIds { get; set; }

        /// <summary>
        /// Option to preserve original NumTriggered value even if trigger state was changed
        /// </summary>
        public bool PreserveNumTriggered { get; set; }
    }
}
