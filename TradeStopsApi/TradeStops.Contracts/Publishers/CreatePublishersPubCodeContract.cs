namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for creating a pub code
    /// </summary>
    public class CreatePublishersPubCodeContract
    {
        /// <summary>
        /// Pub code value
        /// </summary>
        public string PubCode { get; set; }

        /// <summary>
        /// Pub code owner organization value
        /// </summary>
        public string OwnOrg { get; set; }

        /// <summary>
        /// (optional) Pub code description
        /// </summary>
        public string PubTitle { get; set; }

        /// <summary>
        /// (optional) Pub code category
        /// </summary>
        public string PubCategory { get; set; }
    }
}
