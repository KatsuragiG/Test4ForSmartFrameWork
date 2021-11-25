using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Platform tasks contract represents job run by user to execute long-running tasks.
    /// </summary>
    public class PlatformTaskContract
    {
        /// <summary>
        /// ID of the task in platform.
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// User ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Task creation date in UTC timezone.
        /// </summary>
        public DateTime CreationDateUtc { get; set; }

        /// <summary>
        /// Task last update date in UTC timezone.
        /// </summary>
        public DateTime UpdateDateUtc { get; set; }

        /// <summary>
        /// Type of the task.
        /// </summary>
        public PlatformTaskTypes TaskType { get; set; }

        /// <summary>
        /// Status of the task.
        /// </summary>
        public PlatformTaskStatuses TaskStatus { get; set; }

        /// <summary>
        /// Task is deleted by user (soft delete).
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        /// Current task progress type.
        /// </summary>
        public virtual PlatformTaskProgressTypes TaskProgressType { get; set; }

        /// <summary>
        /// Task progress in percent (0 - 100).
        /// </summary>
        public int ProgressPercent { get; set; }

        /// <summary>
        /// Number of task processing attempt.
        /// </summary>
        public int Attempt { get; set; }
    }
}
