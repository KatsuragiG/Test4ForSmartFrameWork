using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Notification event description.
    /// </summary>
    public class NotificationEventContract
    {
        /// <summary>
        /// Notification event ID
        /// </summary>
        public int NotificationEventId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Notification event type
        /// </summary>
        public NotificationEventTypes EventType { get; set; }

        /// <summary>
        /// Entity Id of the related notification event result:
        ///  1. Can be TaskId for tasks based features (example: Pure Quant, Backtester)
        ///  2. Any Id of the entity to get related to the notification results.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// User is informed about the notification
        /// </summary>
        public bool IsUserInformed { get; set; }
    }
}
