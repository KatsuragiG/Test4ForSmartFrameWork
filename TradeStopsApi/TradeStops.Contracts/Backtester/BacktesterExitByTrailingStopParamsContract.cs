namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to use ExitByTrailingStop exit strategy
    /// </summary>
    public class BacktesterExitByTrailingStopParamsContract
    {
        /// <summary>
        /// Trailing Stop Percent value.
        /// </summary>
        public decimal Percent { get; set; }
    }
}
