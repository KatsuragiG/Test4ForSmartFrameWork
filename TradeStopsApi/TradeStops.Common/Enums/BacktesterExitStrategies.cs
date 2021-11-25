namespace TradeStops.Common.Enums
{
    /// <summary>
    /// Exit strategy is the way to determine the date when to sell (close) position in backtesting process.
    /// </summary>
    public enum BacktesterExitStrategies
    {
        /// <summary>
        /// Original exit means that we just use the date when position was sold.
        /// </summary>
        OriginalExit = 1,
        
        /// <summary>
        /// Exit by VQ means that we use VQ to determine exit date.
        /// </summary>
        ExitByVq = 2,

        /// <summary>
        /// Exit by SSI means that we use SSI (Health) to determine exit date.
        /// </summary>
        ExitBySsi = 3,

        /// <summary>
        /// Exit by Trailing Stop means that we sell position when it's current price X% lower than the highest price after purchase date.
        /// </summary>
        ExitByTrailingStop = 4,
    }
}
