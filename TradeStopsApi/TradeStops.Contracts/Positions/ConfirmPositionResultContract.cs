namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of position confirmation.
    /// </summary>
    public class ConfirmPositionResultContract
    {
        /// <summary>
        /// ID of unconfirmed position.
        /// </summary>
        public int UnconfirmedPositionId { get; set; }

        /// <summary>
        /// Created confirmed position.
        /// </summary>
        public PositionContract Position { get; set; }
    }
}
