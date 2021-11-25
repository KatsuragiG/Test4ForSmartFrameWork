using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create single Position Trigger for multiple Positions.
    /// </summary>
    public class BulkCreatePositionTriggersContract
    {
        /// <summary>
        ///  List of positions IDs.
        /// </summary>
        public List<int> PositionIds { get; set; }

        /// <summary>
        ///  Trigger to create.
        /// </summary>
        public TriggerFieldsContract Trigger { get; set; }
    }
}
