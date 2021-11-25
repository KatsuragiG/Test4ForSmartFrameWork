using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio tracker portfolio statistic
    /// </summary>
    public class PortfolioTrackerPortfolioStatsContract
    {
        /// <summary>
        /// Portfolio Id of corresponding portfolio.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// PortfolioStatsTypes of the portfolio stat.
        /// </summary>
        public PortfolioStatsTypes PortfolioStatsType { get; set; }

        /// <summary>
        /// Number of the positions for the corresponding PortfolioStatsTypes.
        /// </summary>
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// Number of the alerts assigned to the positions.
        /// </summary>
        public int NumberOfAlerts { get; set; }

        /// <summary>
        /// Number of the alerts triggered in the past.
        /// </summary>
        public int NumberOfTriggeredAlerts { get; set; }        
        
        /// <summary>
        /// ID of the portolio currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Sum of the position total gain values including dividends.
        /// </summary>
        public decimal TotalGainWithDiv { get; set; }

        /// <summary>
        /// Total percent gain of all positions including dividends.
        /// </summary>
        public decimal GainWithDivPercentage { get; set; }

        /// <summary>
        /// Sum of the position total gain values excluding dividends.
        /// </summary>
        public decimal TotalGainExDiv { get; set; }

        /// <summary>
        /// Sum of the position total cost bases.
        /// </summary>
        public decimal TotalCostBasis { get; set; }

        /// <summary>
        /// Sum of the position total daily gain values.
        /// </summary>
        public decimal TotalDailyGain { get; set; }

        /// <summary>
        /// Daily gain percentage.
        /// </summary>
        public decimal DailyGainPercentage { get; set; }

        /// <summary>
        ///  Sum of the position total dividends.
        /// </summary>
        public decimal TotalDividends { get; set; }

        /// <summary>
        /// Sum of the position values.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Sum of the position previous values.
        /// </summary>
        public decimal PreviousValue { get; set; }

        /// <summary>
        /// Sum of the position absolute values.
        /// </summary>
        public decimal AbsoluteValue { get; set; }

        /// <summary>
        /// Average hold days of positions.
        /// </summary>
        public int AvgHoldPeriod { get; set; }

        /// <summary>
        /// Sum of the days held since the acquisition of all portfolio positions.
        /// </summary>
        public int HoldPeriod { get; set; }

        /// <summary>
        /// Portfolio Cash.
        /// </summary>
        public decimal Cash { get; set; }
    }
}
