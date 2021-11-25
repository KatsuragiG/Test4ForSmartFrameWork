using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with all values as it saved in the database.
    /// It's recommended to use child classes inherited from BaseTriggerContract if possible
    /// </summary>
    public class PtPositionTriggerContract
    {
        /// <summary>
        /// Position trigger ID.
        /// </summary>
        public int PositionTriggerId { get; set; }

        /// <summary>
        /// Position Id.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Type of Position Trigger.
        /// </summary>
        public TriggerTypes TriggerType { get; set; }

        /// <summary>
        /// Creation date of the alert template.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Defines that Position Trigger has been triggered.
        /// </summary>
        public bool IsTriggered { get; set; }

        /// <summary>
        /// The price when the alert will be triggered.
        /// Displayed on PositionsGrid as 'Trigger Price' column
        /// </summary>
        public decimal? StopPrice { get; set; }

        /// <summary>
        /// Earliest date when the Position Trigger has been triggered.
        /// </summary>
        public DateTime? FirstTimeTriggered { get; set; }

        /// <summary>
        /// Latest date when the Position Trigger has been triggered.
        /// </summary>
        public DateTime? LastTriggered { get; set; }

        /// <summary>
        /// Number of days in a row, when Position Trigger stays in triggered state.
        /// </summary>
        public int NumTriggered { get; set; }

        /// <summary>
        /// Date when Position Triggers was processed by the system last time.
        /// </summary>
        public DateTime ProcessTime { get; set; }

        /// <summary>
        /// Position Trigger status.
        /// </summary>
        public TriggerStatuses Status { get; set; }

        /// <summary>
        /// Position Trigger values.
        /// </summary>
        public TriggerFieldsContract Trigger { get; set; }

        /// <summary>
        /// Trigger State values.
        /// </summary>
        public TriggerStateFieldsContract TriggerState { get; set; }

        /// <summary>
        /// Trigger name including trigger parameters. May contain currency sign of position.
        /// </summary>
        public string TriggerDescription { get; set; }

        /// <summary>
        /// State of a trigger applied to specific position
        /// </summary>
        public string TriggerStateDescription { get; set; }

        /// <summary>
        /// Indicates that a trigger is primary for position
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}
