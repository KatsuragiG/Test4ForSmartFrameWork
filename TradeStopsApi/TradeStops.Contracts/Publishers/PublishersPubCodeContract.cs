namespace TradeStops.Contracts
{
    /// <summary>
    /// Pub code contract
    /// </summary>
    public class PublishersPubCodeContract
    {
        /// <summary>
        /// Pub code Id
        /// </summary>
        public int PubCodeId { get; set; }

        /// <summary>
        /// Pub code value
        /// </summary>
        public string PubCode { get; set; }

        /// <summary>
        /// Own org value
        /// </summary>
        public string OwnOrg { get; set; }

        /// <summary>
        /// Pub code description
        /// </summary>
        public string PubTitle { get; set; }

        /// <summary>
        /// Pub code category
        /// </summary>
        public string PubCategory { get; set; }
    }
}
