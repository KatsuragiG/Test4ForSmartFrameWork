namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to publish position
    /// </summary>
    public class PublishPtPositionContract
    {
        /// <summary>
        /// ID of the position.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Indicates whether position should be published (true) or unpublished (false).
        /// </summary>
        public bool IsPublished { get; set; }
    }
}
