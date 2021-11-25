namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for FixedRisk position sizing method 
    /// </summary>
    public class BacktesterPositionSizeByFixedRiskParamsContract
    {
        /// <summary>
        /// ??? (in percents)
        /// </summary>
        public decimal FixedRiskPercent { get; set; }

        /// <summary>
        /// ??? (in dollars)
        /// </summary>
        public decimal EqualDollarRisk { get; set; }
    }
}
