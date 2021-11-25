using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Position
    /// </summary>
    public class PositionContract
    {
        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Portfolio ID of the position.
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Position symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Date and time when the position was originally created in the database.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Position symbol values. Null for PairTrade position
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Annualized % Gain.
        /// </summary>
        public double? AnnualizedGain { get; set; }

        /// <summary>
        /// Date of the latest close price.
        /// </summary>
        public DateTime CloseDate { get; set; }

        /// <summary>
        /// Brokerage commission for closing the position.
        /// </summary>
        public decimal CloseFee { get; set; }

        /// <summary>
        /// Position close price.
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// Position cost basis.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        /// Position cost basis per each position share.
        /// </summary>
        public decimal? CostBasisPerShare { get; set; }

        /// <summary>
        /// Position is deleted in the database.
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        /// Adjusted total dividends value per share.
        /// </summary>
        public decimal DividendAdj { get; set; }

        /// <summary>
        /// Percent of the earned dividends relative to the Entry Price.
        /// </summary>
        public decimal DividendsPercentage { get; set; }

        /// <summary>
        /// Total dividends value.
        /// </summary>
        public decimal? DividendsTotal { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public decimal? DollarGain { get; set; }

        /// <summary>
        /// Daily gain change per one position share.
        /// </summary>
        public decimal GainDailyPerShare { get; set; }

        /// <summary>
        /// Daily Gain Percentage.
        /// </summary>
        public decimal GainDailyPercentage { get; set; }

        /// <summary>
        /// Daily Gain Total.
        /// </summary>
        public decimal GainDailyTotal { get; set; }

        /// <summary>
        /// Dollar Gain starting from the Entry Price, excluding Dividends.
        /// </summary>
        public decimal? GainExDiv { get; set; }

        /// <summary>
        /// Percentage Gain starting from the Entry Price, excluding Dividends.
        /// </summary>
        public decimal? GainExDivPercentage { get; set; }

        /// <summary>
        /// Daily Gain per Share.
        /// </summary>
        public decimal? GainPerShare { get; set; }

        /// <summary>
        /// Dollar Gain per share starting from the Entry Price, excluding Dividends.
        /// </summary>
        public decimal? GainPerShareExDiv { get; set; }

        /// <summary>
        /// The number of calendar days since the Entry Date.
        /// </summary>
        public int HoldPeriod { get; set; }

        /// <summary>
        /// Position is not adjusted by dividends.
        /// </summary>
        public bool IgnoreDividend { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public decimal InitialShares { get; set; }

        /// <summary>
        /// Value to identify that the positions were deleted together with the portfolio. Used for restoring deleted portfolios.
        /// </summary>
        public bool IsDeletedWithPortfolio { get; set; }

        /// <summary>
        /// User's notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Brokerage commission to open a position.
        /// </summary>
        public decimal OpenFee { get; set; }

        /// <summary>
        /// Dollar Gain starting from the Entry Price, including Dividends.
        /// </summary>
        public decimal? PercentGain { get; set; }

        /// <summary>
        /// Percentage that represents how much the latest close is below the high close.
        /// </summary>
        public decimal PercentOffHLClose { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public decimal PercentOffHLHigh { get; set; }

        /// <summary>
        /// Position type in the TradeStops.
        /// </summary>
        public PositionTypes PositionType { get; set; }

        /// <summary>
        /// Previous close price.
        /// </summary>
        public decimal PreviousClose { get; set; }

        /// <summary>
        /// Previous position value.
        /// </summary>
        public decimal PreviousValue { get; set; }

        /// <summary>
        /// Position purchase date.
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Position purchase price not adjusted by corporate actions.
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Purchase price adjusted by all corporate actions.
        /// </summary>
        public decimal? PurchasePriceAdj { get; set; }

        /// <summary>
        /// Number of the position shares.
        /// </summary>
        public decimal Shares { get; set; }

        /// <summary>
        /// Purchase price not adjusted by dividends.
        /// </summary>
        public decimal? SplitsAdj { get; set; }

        /// <summary>
        /// Position status type.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

        /// <summary>
        /// Total gain including dividends.
        /// </summary>
        public decimal? TotalGain { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Total value of the position.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// ID of the position currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Currency values.
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Latest Intraday price.
        /// </summary>
        public decimal? LatestIntradayPrice { get; set; }

        /// <summary>
        /// Determines if the intraday price has to be used.
        /// </summary>
        public bool UseIntraday { get; set; }

        /// <summary>
        /// Internal TradeStops values for synchronized positions. Null for manual positions.
        /// </summary>
        public ImportedPositionFields ImportedPosition { get; set; }

        /// <summary>
        /// Position subtrades. Not null for Pair Trade. Null for Regular position
        /// </summary>
        public List<SubtradeContract> Subtrades { get; set; }

        /// <summary>
        /// Composite information about PairTrade symbols. Not null for PairTrade. Null for Regular position 
        /// </summary>
        public PairTradeSymbolContract PairTradeSymbol { get; set; }
    }
}
