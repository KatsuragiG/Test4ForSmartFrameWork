using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about Backtester task
    /// </summary>
    public class BacktesterTaskContract
    {
        /// <summary>
        /// ID of the task
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// List of strategy IDs that were applied to the task
        /// </summary>
        public List<int> StrategyIds { get; set; }

        /// <summary>
        /// The source of positions (symbols) that are used as input for Backtester
        /// </summary>
        public BacktesterPositionSourcesContract PositionsSources { get; set; }

        /// <summary>
        /// Current status of the task
        /// </summary>
        public BacktesterTaskStatuses Status { get; set; }

        /// <summary>
        /// Time (UTC) when the task was created.
        /// </summary>
        public DateTime CreationDateUtc { get; set; }

        /// <summary>
        /// Time (UTC) when the task was updated.
        /// </summary>
        public DateTime UpdateDateUtc { get; set; }

        /// <summary>
        /// Start date for backtesting
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Stop date for backtesting
        /// </summary>
        public DateTime ToDate { get; set; }
    }
}
