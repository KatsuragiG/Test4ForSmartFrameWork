using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create notification event.
    /// </summary>
    public class CreateNotificationEventContract
    {
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
    }
}
