using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Stock Finder result
    /// </summary>
    public class StockFinderResultContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Ticker
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Currency values.
        /// </summary>
        public CurrencyContract Currency { get; set; }

        /// <summary>
        /// Current SSI value
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// Current VQ value
        /// </summary>
        public decimal CurrentVq { get; set; }

        /// <summary>
        /// Average VQ value for 30 years
        /// </summary>
        public decimal Average30YearsVq { get; set; }

        /// <summary>
        /// VQ ratio
        /// </summary>
        public decimal VqRatio { get; set; }

        /// <summary>
        /// SSI trend type
        /// </summary>
        public TrendTypes? SsiTrend { get; set; }

        /// <summary>
        /// Latest price
        /// </summary>
        public decimal LatestClose { get; set; }

        /// <summary>
        /// Price/Earnings ratio
        /// </summary>
        public decimal? PriceEarningsRatio { get; set; }

        /// <summary>
        /// Revenue
        /// </summary>
        public decimal? Revenue { get; set; }

        /// <summary>
        /// Enterprice/Revenue ratio
        /// </summary>
        public decimal? EnterpriseRevenueRatio { get; set; }

        /// <summary>
        /// Net Income
        /// </summary>
        public decimal? NetIncome { get; set; }

        /// <summary>
        /// EPS
        /// </summary>
        public decimal? Eps { get; set; }

        /// <summary>
        /// Total Access
        /// </summary>
        public decimal? TotalAssets { get; set; }

        /// <summary>
        /// Shares Outstanding
        /// </summary>
        public decimal? SharesOutstanding { get; set; }

        /// <summary>
        /// Enterprice Value
        /// </summary>
        public decimal? EnterpriseValue { get; set; }

        /// <summary>
        /// Price/Book ratio
        /// </summary>
        public decimal? PriceBookRatio { get; set; }

        /// <summary>
        /// Price/EarningsToGrowth ratio
        /// </summary>
        public decimal? PriceEarningsToGrowthRatio { get; set; }

        /// <summary>
        /// EBITDA
        /// </summary>
        public decimal? Ebitda { get; set; }

        /// <summary>
        /// Enterprice/EBITDA ratio
        /// </summary>
        public decimal? EnterpriceEbitdaRatio { get; set; }

        /// <summary>
        /// Market Cap
        /// </summary>
        public decimal? MarketCap { get; set; }

        /// <summary>
        /// Dividend Yield
        /// </summary>
        public decimal? DividendYield { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public long? Volume { get; set; }

        /// <summary>
        /// Sector
        /// </summary>
        public string Sector { get; set; }

        /// <summary>
        /// SubSector
        /// </summary>
        public string SubSector { get; set; }

        /// <summary>
        /// Industry
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// SubIndustry
        /// </summary>
        public string SubIndustry { get; set; }

        /// <summary>
        /// One year high
        /// </summary>
        public decimal? OneYearHigh { get; set; }

        /// <summary>
        /// One day change in percent
        /// </summary>
        public decimal? OneDayChangeInPercent { get; set; }

        /// <summary>
        /// One week change in percent
        /// </summary>
        public decimal? OneWeekChangeInPercent { get; set; }

        /// <summary>
        /// One month change in percent
        /// </summary>
        public decimal? OneMonthChangeInPercent { get; set; }

        /// <summary>
        /// One year change in percent
        /// </summary>
        public decimal? OneYearChangeInPercent { get; set; }

        /// <summary>
        /// Three years change in percent
        /// </summary>
        public decimal? ThreeYearsChangeInPercent { get; set; }

        /// <summary>
        /// Five years change in percent
        /// </summary>
        public decimal? FiveYearsChangeInPercent { get; set; }

        /// <summary>
        /// Entry date
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// Early Entry date
        /// </summary>
        public DateTime? EarlyEntryDate { get; set; }

        /// <summary>
        /// Timing turn area type
        /// </summary>
        public TimingTurnAreaTypes? TurnAreaType { get; set; }

        /// <summary>
        /// Timing turn strength
        /// </summary>
        public TimingTurnStrengthTypes? TurnStrength { get; set; }

        /// <summary>
        /// List of investment strategies that contain current symbol in results
        /// </summary>
        public List<InvestmentStrategyContract> Strategies { get; set; }

        /// <summary>
        /// List of newsletter portfolios that contain current symbol
        /// </summary>
        public List<NewslettersPortfolioWithSymbolRefDateContract> NewsletterPortfolios { get; set; }

        /// <summary>
        /// Latest RSI(14) value
        /// </summary>
        public decimal? LatestRsi14 { get; set; }

        /// <summary>
        /// Geo mean rank by selected strategies
        /// </summary>
        public int? StrategyRank { get; set; }

        /// <summary>
        /// Global Rank
        /// </summary>
        public GlobalRankTypes GlobalRank { get; set; }
    }
}