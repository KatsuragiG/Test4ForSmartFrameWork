using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class PositionsGridRowContract
    {
        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Number of alerts created on this position.
        /// </summary>
        public int AlertsCount { get; set; }

        /// <summary>
        /// Annualized % Gain.
        /// </summary>
        public double? AnnualizedGain { get; set; }

        /// <summary>
        /// Date of the latest close price.
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Brokerage commission for closing the position.
        /// </summary>
        public decimal CloseFee { get; set; }

        /// <summary>
        /// Position close price.
        /// </summary>
        public decimal? ClosePrice { get; set; }

        /// <summary>
        /// Option contract size. Available only for options.
        /// </summary>
        public decimal? ContractSize { get; set; }

        /// <summary>
        /// Position cost basis.
        /// </summary>
        public decimal? CostBasis { get; set; }

        /// <summary>
        /// Currency code of the position. If portfolios are in different currencies all values will be converted to default user currency using cross rates.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Sign of the position currency.
        /// </summary>
        public string CurrencySign { get; set; }

        /// <summary>
        /// Currency ID of the position.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Adjusted total dividends value per share.
        /// </summary>
        public decimal? DividendAdj { get; set; }

        /// <summary>
        /// Percent of the earned dividends relative to the Entry Price.
        /// </summary>
        public decimal? DividendsPercentage { get; set; }

        /// <summary>
        /// Total dividends value.
        /// </summary>
        public decimal? DividendsTotal { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public decimal? DollarGain { get; set; }

        /// <summary>
        /// Enterprise value and FQ.
        /// </summary>
        public long? EnterpriseValueAndFQ { get; set; }

        /// <summary>
        /// Enterprise value EBITD and TTM.
        /// </summary>
        public decimal? EnterpriseValueEBITDAndTTM { get; set; }

        /// <summary>
        /// Enterprise value to revenue.
        /// </summary>
        public decimal? EnterpriseValueToRevenue { get; set; }

        /// <summary>
        /// Option expiration date. Available only for options.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Daily gain change per one position share.
        /// </summary>
        public decimal GainDailyPerShare { get; set; }

        /// <summary>
        /// Daily Gain Percentage.
        /// </summary>
        public decimal GainDailyPercentage { get; set; }

        /// <summary>
        /// Total Daily Gain.
        /// </summary>
        public decimal? GainDailyTotal { get; set; }

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
        /// Number of triggered allerts created on this position.
        /// </summary>
        public int TriggeredAlertsCount { get; set; }

        /// <summary>
        /// Date when the max profitable close price was reached.
        /// </summary>
        public DateTime? MaxProfitableCloseDate { get; set; }

        /// <summary>
        /// The max profitable closing price since entry.
        /// </summary>
        public decimal? MaxProfitableClosePrice { get; set; }

        /// <summary>
        /// The number of calendar days since the Entry Date.
        /// </summary>
        public int HoldPeriod { get; set; }

        /// <summary>
        /// Company industry name.
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// Internal TradeStops value.Always false.
        /// </summary>
        public bool IsCustomSymbol { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public bool IsPossibleToClose { get; set; }

        /// <summary>
        /// Position is synchronized.
        /// </summary>
        public bool IsSyncPosition { get; set; }

        /// <summary>
        /// Position from synchronized portfolio.
        /// </summary>
        public bool IsSyncPortfolio { get; set; }

        /// <summary>
        /// Position from portfolio with Investment type.
        /// </summary>
        public bool IsInvestmentPortfolio { get; set; }

        /// <summary>
        /// Last annual EPS.
        /// </summary>
        public decimal? LastAnnualEPS { get; set; }

        /// <summary>
        /// Last annual Net income.
        /// </summary>
        public decimal? LastAnnualNetIncome { get; set; }

        /// <summary>
        /// Last annual revenue.
        /// </summary>
        public decimal? LastAnnualRevenue { get; set; }

        /// <summary>
        /// Last annual total assets.
        /// </summary>
        public decimal? LastAnnualTotalAssets { get; set; }

        /// <summary>
        /// Position close price.
        /// </summary>
        public decimal? LatestClose { get; set; }

        /// <summary>
        /// Market Cap.
        /// </summary>
        public decimal? MarketCap { get; set; }

        /// <summary>
        /// Max Gain %.
        /// </summary>
        public decimal? MaxGainPercent { get; set; }

        /// <summary>
        /// Min Gain %.
        /// </summary>
        public decimal? MinGainPercent { get; set; }

        /// <summary>
        /// 10 day Simple Moving Average.
        /// </summary>
        public decimal? MovingAvg10 { get; set; }

        /// <summary>
        /// 200 day Simple Moving Average.
        /// </summary>
        public decimal? MovingAvg200 { get; set; }

        /// <summary>
        /// 50 day Simple Moving Average.
        /// </summary>
        public decimal? MovingAvg50 { get; set; }

        /// <summary>
        /// Custom position name (obsolete).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Position notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Number of сompany employees.
        /// </summary>
        public int? NumberOfEmployees { get; set; }

        /// <summary>
        /// Brokerage commission to open a position.
        /// </summary>
        public decimal OpenFee { get; set; }

        /// <summary>
        /// Underline symbol of option. Alaivable only for options.
        /// </summary>
        public string OptionSymbol { get; set; }

        /// <summary>
        /// Dollar Gain starting from the Entry Price, including Dividends.
        /// </summary>
        public decimal? PercentGain { get; set; }

        /// <summary>
        /// Percentage that represents how much the latest close is below the high close.
        /// </summary>
        public decimal? PercentOffHLClose { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public decimal? PercentOffHLHigh { get; set; }

        /// <summary>
        /// Position percent of portfolio without cash.
        /// </summary>
        public decimal? PercentOffPortfolio { get; set; }

        /// <summary>
        /// Position percent of portfolio without cash.
        /// </summary>
        public decimal? PercentOffPortfolioWithoutCash { get; set; }

        /// <summary>
        /// Position portfolio ID
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Position portfolio name.
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        /// Previous close price.
        /// </summary>
        public decimal PreviousClose { get; set; }

        /// <summary>
        /// Price book and FQ.
        /// </summary>
        public decimal? PriceBookAndFQ { get; set; }

        /// <summary>
        /// Price earnings and TTM.
        /// </summary>
        public decimal? PriceEarningsAndTTM { get; set; }

        /// <summary>
        /// Price earnings to growth and TTM.
        /// </summary>
        public decimal? PriceEarningsToGrowthAndTTM { get; set; }

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
        /// Sector name.
        /// </summary>
        public string SectorName { get; set; }

        /// <summary>
        /// Number of the position shares.
        /// </summary>
        public decimal? Shares { get; set; }

        /// <summary>
        /// Position VQ value.
        /// </summary>
        public decimal? VolatilityQuotient { get; set; }

        /// <summary>
        /// Average VQ value fot 30 years
        /// </summary>
        public decimal? Average30YearsVolatilityQuotient { get; set; }

        /// <summary>
        /// Purchase price not adjusted by dividends.
        /// </summary>
        public decimal? SplitsAdj { get; set; }

        /// <summary>
        /// Position status type.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

        /// <summary>
        /// Internal TradeStops value
        /// </summary>
        public decimal? StopPrice { get; set; }

        /// <summary>
        /// Position strike price.Available only for options.
        /// </summary>
        public decimal StrikePrice { get; set; }

        /// <summary>
        /// Option strike type. Valid values P for put and C for call. Available only for options.
        /// </summary>
        public string StrikeType { get; set; }

        /// <summary>
        /// Position symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Position symbol has been delisted.
        /// </summary>
        public bool SymbolDelisted { get; set; }

        /// <summary>
        /// Symbol type
        /// </summary>
        [Obsolete("2020-10-26. Use SymbolType instead.")]
        public SymbolDataTypes SymbolDataType { get; set; }

        /// <summary>
        /// SymbolType (stock, option, fund, etc.)
        /// </summary>
        public SymbolTypes SymbolType { get; set; }

        /// <summary>
        /// Position symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Company name.
        /// </summary>
        public string SymbolName { get; set; }

        /// <summary>
        /// Position symbol exchange.
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Tags that were assigned on this position.
        /// </summary>
        public List<PositionTagContract> Tags { get; set; }

        /// <summary>
        /// Total gain including dividends.
        /// </summary>
        public decimal? TotalGain { get; set; }

        /// <summary>
        /// Total shares outstanding.
        /// </summary>
        public long? TotalSharesOutstanding { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Internal TradeStops value. Always null.
        /// </summary>
        public decimal? TrailingStopPercent { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public string UpdateDate { get; set; }

        /// <summary>
        /// Total value of the position.
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Latest symbol trade volume.
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Contact with SSI values.
        /// </summary>
        public SsiValueContract SsiValue { get; set; }

        /// <summary>
        /// Amount of money to risk based on SSI Stop Price. 
        /// </summary>
        public decimal? SsiAtRisk { get; set; }

        /// <summary>
        /// Days before position expiration. Available only for options.
        /// </summary>
        public int DaysBeforeExpiration { get; set; }

        /// <summary>
        /// Internal TradeStops value.
        /// </summary>
        public bool ChangingSharesPermission { get; set; }

        /// <summary>
        /// Position is not adjusted by dividends.
        /// </summary>
        public bool IgnoreDividends { get; set; }

        /// <summary>
        /// Position type in the TradeStops
        /// </summary>
        public PositionTypes PositionType { get; set; }

        /// <summary>
        /// Symbol has announced corporate actions. Prices will be corrected.
        /// </summary>
        public bool HasDelayedCorporateActions { get; set; }

        /// <summary>
        /// Date and time when the position has been created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Latest Intraday price.
        /// </summary>
        public decimal? LatestIntradayPrice { get; set; }

        /// <summary>
        /// Dividend Yield.
        /// </summary>
        public decimal? TrailingDividendYield { get; set; }

        /// <summary>
        /// Sub Sector name.
        /// </summary>
        public string SubSectorName { get; set; }

        /// <summary>
        /// Sub Industry name.
        /// </summary>
        public string SubIndustryName { get; set; }

        /// <summary>
        /// Ssi trend.
        /// </summary>
        public RocValueContract SsiTrend { get; set; }

        /// <summary>
        /// Current timing turn area.
        /// </summary>
        public TimingTurnAreaContract TurnArea { get; set; }

        /// <summary>
        /// Relative Strength Index (RSI) value with period = 14.
        /// </summary>
        public decimal? Rsi14 { get; set; }

        /// <summary>
        /// List of position issues.
        /// </summary>
        public List<PositionGridIssueTypes> PositionIssues { get; set; }

        /// <summary>
        /// Id of parent position.
        /// </summary>
        public int? ParentPositionId { get; set; }

        /// <summary>
        /// Global rank, calculated as an average rating across all criteria.
        /// </summary>
        public GlobalRankTypes? GlobalRank { get; set; }

        /// <summary>
        /// Implied volatility absolute value.
        /// </summary>
        public decimal? ImpliedVolatility { get; set; }

        /// <summary>
        /// Implied volatility rank absolute value.
        /// </summary>
        public decimal? ImpliedVolatilityRank { get; set; }

        /// <summary>
        /// Implied volatility percentile absolute value.
        /// </summary>
        public decimal? ImpliedVolatilityPercentile { get; set; }

        /// <summary>
        /// Bid value.
        /// </summary>
        public decimal? Bid { get; set; }

        /// <summary>
        /// Ask value.
        /// </summary>
        public decimal? Ask { get; set; }

        /// <summary>
        /// Delta value.
        /// </summary>
        public decimal? Delta { get; set; }

        /// <summary>
        /// Theta value.
        /// </summary>
        public decimal? Theta { get; set; }

        /// <summary>
        /// Gamma value.
        /// </summary>
        public decimal? Gamma { get; set; }

        /// <summary>
        /// Vega value.
        /// </summary>
        public decimal? Vega { get; set; }

        /// <summary>
        /// Bid/Ask spread.
        /// </summary>
        public decimal? BidAskSpread { get; set; }
    }
}
