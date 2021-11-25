using TradeStops.Common.Enums;
using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Platform Task fields to edit. Initialize only fields you want to patch
    /// </summary>
    public class EditPlatformTaskContract
    {
        /// <summary>
        /// Status of the task.
        /// </summary>
        public Optional<PlatformTaskStatuses> TaskStatus { get; set; }

        /// <summary>
        /// Current task progress type.
        /// </summary>
        public Optional<PlatformTaskProgressTypes> TaskProgressType { get; set; }

        /// <summary>
        /// Task progress in percent (0 - 100).
        /// </summary>
        public Optional<int> ProgressPercent { get; set; }
    }
}
