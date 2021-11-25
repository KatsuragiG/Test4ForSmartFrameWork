namespace TradeStops.Contracts
{
    /// <summary>
    /// Asset allocation group
    /// </summary>
    public class NewslettersAssetAllocationGroupContract
    {
        /// <summary>
        /// Industry or sector group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Weight of group - value in [0..1] interval
        /// </summary>
        public decimal GroupWeight { get; set; }
    }
}
