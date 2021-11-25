namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio statistic.
    /// </summary>
    public class PublishersPortfolioStatsContract
    {
        /// <summary>
        /// Portfolio Id.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Amount of all positions.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Average gain among of all positions.
        /// </summary>
        public decimal AvgGain { get; set; }

        /// <summary>
        /// Average capital gains yield.
        /// </summary>
        public decimal? AverageCapitalGainsYield { get; set; }

        /// <summary>
        /// Average gain among of all positions over the last month.
        /// </summary>
        public decimal MonthAvgGain { get; set; }

        /// <summary>
        /// Average gain among of all positions over the last quarter.
        /// </summary>
        public decimal QuarterAvgGain { get; set; }

        /// <summary>
        /// Average gain among of all positions over the last half year.
        /// </summary>
        public decimal HalfAvgGain { get; set; }

        /// <summary>
        /// Average gain among of all positions over the last year.
        /// </summary>
        public decimal YearAvgGain { get; set; }

        /// <summary>
        /// Average hold period among of all positions.
        /// </summary>
        public decimal AvgHold { get; set; }

        /// <summary>
        /// Percent of wins.
        /// </summary>
        public double? PercentWins { get; set; }

        /// <summary>
        /// Biggest winner.
        /// </summary>
        public decimal? BiggestWinner { get; set; }

        /// <summary>
        /// Biggest loser.
        /// </summary>
        public decimal? BiggestLoser { get; set; }

        /// <summary>
        /// Available cash.
        /// </summary>
        public decimal? AvailableCash { get; set; }

        /// <summary>
        /// Available cash (%).
        /// </summary>
        public decimal? AvailableCashPercent { get; set; }

        /// <summary>
        /// Portfolio value.
        /// </summary>
        public decimal PortfolioValue { get; set; }
    }
}
