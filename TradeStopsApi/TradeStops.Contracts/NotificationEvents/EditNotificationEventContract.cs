using TradeStops.Contracts.Types;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Notification event fields to edit. Initialize only fields you want to patch.
    /// </summary>
    public class EditNotificationEventContract
    {
        /// <summary>
        /// (required) Notification event ID.
        /// </summary>
        public int NotificationEventId { get; set; }

        /// <summary>
        /// User is informed about the notification
        /// </summary>
        public Optional<bool> IsUserInformed { get; set; }
    }
}
