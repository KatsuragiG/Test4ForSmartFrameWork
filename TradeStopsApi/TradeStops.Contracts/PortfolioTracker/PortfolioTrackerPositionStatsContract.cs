using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Total position statistics calculated using corresponding (Open, Closed, All) trades.
    /// </summary>
    public class PortfolioTrackerPositionStatsContract
    {
        /// <summary>
        /// Unique position stats Id.
        /// </summary>
        public int PositionStatsId { get; set; }

        /// <summary>
        /// Unique position Id.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Type of trades that were used to calculate position stats.
        /// </summary>
        public PositionStatsTypes StatsType { get; set; }

        /// <summary>
        /// Position trade type that were used to calculate position stats.
        /// </summary>
        public TradeTypes? TradeType { get; set; }

        /// <summary>
        /// Indicates Id of currency that were used to calculate values.
        /// </summary>
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Indicates whether position has no trades with specified type or not.
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Date of first entry transaction.
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /////// <summary>
        /////// Weighted Entry Price adjusted by all corporate actions.
        /////// </summary>
        ////public decimal? EntryPriceAdj { get; set; }

        /// <summary>
        /// Weighted Entry Price adjusted by all corporate actions except dividends.
        /// </summary>
        public decimal? EntryPriceSplitAdj { get; set; }

        /// <summary>
        /// Adjusted Quantity value that was used in calculations. 
        /// For Options this value is calculated as number of contracts multiplied by contract size.
        /// </summary>
        public decimal? QuantityAdj { get; set; }        

        /// <summary>
        /// Date of last exit transaction.
        /// </summary>
        public DateTime? ExitDate { get; set; }

        /// <summary>
        /// Weighted exit price adjusted by all corporate actions.
        /// </summary>
        public decimal? ExitPriceSplitAdj { get; set; }

        /// <summary>
        /// Latest close date on the market.
        /// </summary>
        public DateTime? LatestCloseDate { get; set; }

        /// <summary>
        /// Latest close price on the market.
        /// </summary>
        public decimal? LatestClosePrice { get; set; }

        /// <summary>
        /// Previous close price on the market.
        /// </summary>
        public decimal? PreviousClosePrice { get; set; }

        /////// <summary>
        /////// Date when highest close price was recorded on the market.
        /////// </summary>
        ////public DateTime? HighestCloseDate { get; set; }

        /////// <summary>
        /////// Highest close price on the market since Entry Date till Exit Date (for fully closed position) or Latest Close Date (for position with still opened trades)
        /////// </summary>
        ////public decimal? HighestClosePrice { get; set; }

        /////// <summary>
        /////// Date when lowest close price was recorded on the market.
        /////// </summary>
        ////public DateTime? LowestCloseDate { get; set; }

        /////// <summary>
        /////// Lowest close price on the market since Entry Date till Exit Date (for fully closed position) or Latest Close Date (for position with still opened trades)
        /////// </summary>
        ////public decimal? LowestClosePrice { get; set; }

        /////// <summary>
        /////// Maximum recorded gain percent value since entry date.
        /////// </summary>
        ////public decimal? MaxGainPercent { get; set; }

        /////// <summary>
        /////// Minimum recorded gain percent value since entry date.
        /////// </summary>
        ////public decimal? MinGainPercent { get; set; }

        /////// <summary>
        /////// Percentage that represents how much the latest close is below the max profitable close price.
        /////// </summary>
        ////public decimal? PercentOffMaxProfitableClose { get; set; }

        /// <summary>
        /// Annualized % gain.
        /// </summary>
        public double? AnnualizedGain { get; set; }

        /// <summary>
        /// Cost basis per quantity.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        /// Total cost basis.
        /// </summary>
        public decimal? TotalCostBasis { get; set; }

        /// <summary>
        /// Percent of the earned dividends relative to the Entry Price.
        /// </summary>
        public decimal? DividendsPercentage { get; set; }

        /// <summary>
        /// Total value of earned dividends.
        /// </summary>
        public decimal? TotalDividends { get; set; }

        /// <summary>
        /// Value of earned dividends per quantity.
        /// </summary>
        public decimal? Dividends { get; set; }

        /// <summary>
        /// Daily gain per quantity.
        /// </summary>
        public decimal? DailyGain { get; set; }

        /// <summary>
        /// Daily gain percentage.
        /// </summary>
        public decimal? DailyGainPercentage { get; set; }

        /// <summary>
        /// Total daily gain.
        /// </summary>
        public decimal? TotalDailyGain { get; set; }

        /// <summary>
        /// Dollar gain starting from the Entry Price, excluding dividends, per quantity.
        /// </summary>
        public decimal? GainExDiv { get; set; }

        /// <summary>
        /// Percentage gain starting from the Entry Price, excluding dividends.
        /// </summary>
        public decimal? GainExDivPercentage { get; set; }

        /// <summary>
        /// Total dollar gain starting from the Entry Price, excluding dividends.
        /// </summary>
        public decimal? TotalGainExDiv { get; set; }

        /// <summary>
        /// Dollar gain starting from the Entry Price, including dividends, per quantity.
        /// </summary>
        public decimal? GainWithDiv { get; set; }

        /// <summary>
        /// The number of calendar days since the Entry Date.
        /// </summary>
        public int HoldPeriod { get; set; }

        /// <summary>
        /// Percentage gain starting from the Entry Price, including dividends, per quantity.
        /// </summary>
        public decimal? GainWithDivPercentage { get; set; }

        /// <summary>
        /// Total gain including dividends.
        /// </summary>
        public decimal? TotalGainWithDiv { get; set; }

        /// <summary>
        /// Total value of the position.
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Total value of the position for previous trading date.
        /// </summary>
        public decimal? PreviousValue { get; set; }
    }
}
