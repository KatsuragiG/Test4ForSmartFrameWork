namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to use EqualVqRisk position sizing method.
    /// </summary>
    public class BacktesterPositionSizeByEqualVqRiskParamsContract
    {
        /// <summary>
        /// Max VQ (Usually it's 95)
        /// </summary>
        public decimal MaxVq { get; set; }

        /// <summary>
        /// Max VQ (Usually it's 5)
        /// </summary>
        public decimal MinVq { get; set; }

        /// <summary>
        /// Max VQ (Usually it's 25)
        /// </summary>
        public decimal DefaultVq { get; set; }

        /// <summary>
        /// ??? (in dollars)
        /// </summary>
        public decimal EqualDollarRisk { get; set; }
    }
}
