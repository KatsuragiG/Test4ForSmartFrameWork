namespace TradeStops.Contracts
{
    /// <summary>
    /// Some basic information about portfolio
    /// </summary>
    public class PortfolioTotalContract
    {
        /// <summary>
        /// Average portfolios gain.
        /// </summary>
        public decimal AvgGain { get; set; }

        /// <summary>
        /// Average portfolios daily gain.
        /// </summary>
        public decimal AvgGainDaily { get; set; }

        /// <summary>
        /// Average hold days of positions.
        /// </summary>
        public int AvgHold { get; set; }

        /// <summary>
        /// Total portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// Sum of the positions cost basis.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        /// Currency code of the portfolio.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Sign of the portfolio currency. If portfolios are in different currencies all values will be converted to default user currency using cross rates.
        /// </summary>
        public string CurrencySign { get; set; }

        /// <summary>
        /// Total dividends of portfolios.
        /// </summary>
        public decimal DividendsTotal { get; set; }

        /// <summary>
        /// Total daily gain of portfolios.
        /// </summary>
        public decimal GainDailyTotal { get; set; }

        /// <summary>
        /// Total position number.
        /// </summary>
        public int PositionsCount { get; set; }

        /// <summary>
        /// Total gain of portfolios.
        /// </summary>
        public decimal TotalGain { get; set; }

        /// <summary>
        /// Total value of all positions.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Total Brokerage value of all positions.
        /// </summary>
        public decimal? BrokerageValue { get; set; }
    }
}
