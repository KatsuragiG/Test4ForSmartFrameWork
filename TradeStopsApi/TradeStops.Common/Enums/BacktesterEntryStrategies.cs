namespace TradeStops.Common.Enums
{
    /// <summary>
    /// Entry strategy is the way to determine date when to buy position in backtesting process.
    /// </summary>
    public enum BacktesterEntryStrategies
    {
        /// <summary>
        /// Original entry means that we just use the date when position was purchased.
        /// </summary>
        OriginalEntry = 1,

        ////EntryBySsi = 2,
    }
}
