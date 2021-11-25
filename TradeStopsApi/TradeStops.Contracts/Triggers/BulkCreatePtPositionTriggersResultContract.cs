using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of creating 
    /// </summary>
    public class BulkCreatePtPositionTriggersResultContract
    {
        /// <summary>
        /// An array of skipped triggers that were not created.
        /// </summary>
        public List<SkippedTrigger> SkippedTriggers { get; set; }

        /// <summary>
        /// An array of triggers that were created successfully.
        /// </summary>
        public List<PtPositionTriggerContract> CreatedTriggers { get; set; }

        /// <summary>
        /// Trigger that was not created because it was not possible
        /// </summary>
        public class SkippedTrigger
        {
            /// <summary>
            /// Error code number.
            /// </summary>
            public int ErrorCode { get; set; }

            /// <summary>
            /// Position Id.
            /// </summary>
            public int PositionId { get; set; }

            /// <summary>
            /// Trigger values.
            /// </summary>
            public TriggerFieldsContract Trigger { get; set; }

            /// <summary>
            /// Trigger description.
            /// </summary>
            public string TriggerDescription { get; set; }
        }
    }
}
