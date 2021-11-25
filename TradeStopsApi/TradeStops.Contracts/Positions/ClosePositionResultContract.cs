namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of closing position
    /// </summary>
    public class ClosePositionResultContract
    {
        /// <summary>
        /// Closed position values.
        /// </summary>
        public PositionContract ClosedPosition { get; set; }

        /// <summary>
        /// Part of the opened position that left after partial close.
        /// </summary>
        public PositionContract OpenPositionLeftAfterPartialClose { get; set; }
    }
}
