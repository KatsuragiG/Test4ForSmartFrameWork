namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for EqualSsiRisk position sizing method 
    /// </summary>
    public class BacktesterPositionSizeByEqualSsiRiskParamsContract
    {
        /// <summary>
        /// ??? (in dollars)
        /// </summary>
        public decimal EqualDollarRisk { get; set; }
    }
}
