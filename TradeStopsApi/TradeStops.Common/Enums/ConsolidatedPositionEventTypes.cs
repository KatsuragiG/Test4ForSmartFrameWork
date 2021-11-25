namespace TradeStops.Common.Enums
{
    public enum ConsolidatedPositionEventTypes : byte
    {
        Split = 1,
        Dividend = 2,
        StockDistribution = 3,
        SpinOff = 4,
        SymbolDelisted = 5,
        OptionExpired = 6,
        OptionDelisted = 7,
        PositionOpened = 8,
        PositionClosed = 9,
        SharesAdded = 10,
        SharesClosed = 11,
        PossiblyClosedPosition = 12
    }
}
