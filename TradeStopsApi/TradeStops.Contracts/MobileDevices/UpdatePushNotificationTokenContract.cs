namespace TradeStops.Contracts
{ 
    /// <summary>
    /// Contract to update Token that will be used for push notifications
    /// </summary>
    public class UpdatePushNotificationTokenContract
    {
        /// <summary>
        /// Token value
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Device identifier
        /// </summary>
        public string DeviceId { get; set; }
    }
}
