namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract for confirmation of unconfirmed positions.
    /// </summary>
    public class ConfirmUnconfirmedPositionContract
    {
        /// <summary>
        /// Unconfirmed position Id.
        /// </summary>
        public int UnconfirmedPositionId { get; set; }

        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }
    }
}
