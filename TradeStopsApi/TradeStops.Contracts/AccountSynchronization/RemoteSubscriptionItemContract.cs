namespace TradeStops.Contracts
{
    /// <summary>
    /// Subscription item from remote CRM
    /// </summary>
    public class RemoteSubscriptionItemContract
    {
        /// <summary>
        /// Snaid or AgoraCustomerNumber
        /// </summary>
        public string CustomerNumber { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Publication code (subscription item number)
        /// </summary>
        public string PubCode { get; set; }

        /// <summary>
        /// Subtype is used additionally to PubCode to separate items with the same PubCode.
        /// Used in Agora to separate Newsletters by Southbank
        /// </summary>
        public string Subtype { get; set; }
    }
}
