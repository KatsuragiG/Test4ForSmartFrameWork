using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Push notification token Contract
    /// </summary>
    public class PushNotificationTokenContract
    {
        /// <summary>
        /// Push notification token id.
        /// </summary>
        public int PushNotificationTokenId { get; set; }

        /// <summary>
        /// TradeSmith user id.
        /// </summary>
        public int TradeSmithUserId { get; set; }

        /// <summary>
        /// Date when push notification token was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Token value.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Mobile Device unique Id.
        /// </summary>
        public int MoblieDeviceId { get; set; }
    }
}
