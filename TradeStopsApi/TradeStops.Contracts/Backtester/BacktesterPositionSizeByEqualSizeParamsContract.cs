namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for EqualSize position sizing method 
    /// </summary>
    public class BacktesterPositionSizeByEqualSizeParamsContract
    {
        /// <summary>
        /// ??? (in dollars)
        /// </summary>
        public decimal EqualDollarRisk { get; set; }
    }
}
