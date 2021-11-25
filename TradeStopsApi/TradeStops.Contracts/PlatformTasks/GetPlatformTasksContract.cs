using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Platform tasks parameters to get platform tasks.
    /// </summary>
    public class GetPlatformTasksContract
    {
        /// <summary>
        /// Types of the task.
        /// </summary>
        public List<PlatformTaskTypes> TaskTypes { get; set; }

        /// <summary>
        /// Statuses of the task.
        /// </summary>
        public List<PlatformTaskStatuses> TaskStatuses { get; set; }
    }
}
