namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of position size calculation by risk amount
    /// </summary>
    public class PositionSizeByRiskContract
    {
        /// <summary>
        /// How much you can invest in the analyzed symbol.
        /// </summary>
        public decimal RecommendedInvestableAmount { get; set; }

        /// <summary>
        /// Amount of shares you can purchase.
        /// </summary>
        public decimal SharesToBuy { get; set; }

        /// <summary>
        /// Current Stop Price based on the Entry Price and the stop loss strategy.
        /// </summary>
        public decimal StopPrice { get; set; }

        /// <summary>
        /// Position risk percent.
        /// </summary>
        public decimal RiskPercent { get; set; }
    }
}
