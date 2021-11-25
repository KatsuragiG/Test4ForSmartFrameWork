using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Newsletter Position
    /// </summary>
    public class NewsletterPositionContract
    {
        /// <summary>
        /// ID of the newsletter position
        /// </summary>
        public int? PositionId { get; set; }

        /// <summary>
        /// ID of the newsletter portfolio
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// The status of billionaire position
        /// </summary>
        public BillionairePositionStatuses? BillionaireStatus { get; set; }

        /// <summary>
        /// Date of the report
        /// </summary>
        public DateTime? ReportDate { get; set; }

        /// <summary>
        /// The ticker of newsletter position
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// The name of newsletter position
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The trade type of newsletter position
        /// </summary>
        public TradeTypes? TradeType { get; set; }

        /// <summary>
        /// Ref date value
        /// </summary>
        public DateTime? RefDate { get; set; }

        /// <summary>
        /// Ref price value
        /// </summary>
        public decimal? RefPrice { get; set; }

        /// <summary>
        /// Date and time in UTC when position was added to portfolio
        /// </summary>
        public DateTime? PublishedDateUtc { get; set; }

        /// <summary>
        /// Percent of the shares difference
        /// </summary>
        public decimal? PercentOfShares { get; set; }

        /// <summary>
        /// Number of the newsletter position shares
        /// </summary>
        public decimal? Shares { get; set; }

        /// <summary>
        /// Latest price value of the symbol
        /// </summary>
        public decimal? LatestPrice { get; set; }

        /// <summary>
        /// Latest intraday price.
        /// </summary>
        public decimal? LatestIntradayPrice { get; set; }

        /// <summary>
        /// ID of the symbol
        /// </summary>
        public int? SymbolId { get; set; }

        /// <summary>
        /// Symbol values
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// The name of portfolio
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        /// The name of sub-portfolio
        /// </summary>
        public string SubPortfolioName { get; set; }

        /// <summary>
        /// The link of open date
        /// </summary>
        public string OpenDateLink { get; set; }

        /// <summary>
        /// The name of trade group
        /// </summary>
        public string TradeGroup { get; set; }

        /// <summary>
        /// Exit strategy name
        /// </summary>
        public string ExitStrategyName { get; set; }

        /// <summary>
        /// Exit strategy desciption
        /// </summary>
        public string ExitStrategyDesciption { get; set; }

        /// <summary>
        /// Exit strategy value
        /// </summary>
        public double? ExitStrategyValue { get; set; }

        /// <summary>
        /// The sign of currency
        /// </summary>
        public string CurrencySign { get; set; }

        /// <summary>
        /// Position advice
        /// </summary>
        public string Advice { get; set; }

        /// <summary>
        /// The type of newsletter position
        /// </summary>
        public NewsletterPositionTypes? PositionType { get; set; }

        /// <summary>
        /// Return value
        /// </summary>
        public double? Return { get; set; }

        /// <summary>
        /// The name of currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Total dividends value
        /// </summary>
        public double? Dividends { get; set; }

        /// <summary>
        /// List of sub-trades
        /// </summary>
        public List<NewsletterPositionContract> SubTrades { get; set; }

        /// <summary>
        /// Latest vq value
        /// </summary>
        public decimal? LatestVq { get; set; }

        /// <summary>
        /// Ssi current values
        /// </summary>
        public SsiCurrentValueContract SsiCurrentValue { get; set; }

        /// <summary>
        /// The type of like folio sentiment
        /// </summary>
        public LikeFolioSentimentTypes? LikeFolioSentimentType { get; set; }

        /// <summary>
        /// The trigger based on Exit Strategy name from Portfolio Tracker.
        /// Primary alert in case of PT2 portfolio
        /// </summary>
        public TriggerFieldsContract Trigger { get; set; }

        /// <summary>
        /// Trigger description
        /// </summary>
        public string TriggerDescription { get; set; } // AlertTriggerTypeTitle

        /// <summary>
        /// Position status type.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

        /// <summary>
        /// Date when position was sold.
        /// </summary>
        public DateTime? ExitDate { get; set; }

        /// <summary>
        /// Price at which position was sold.
        /// </summary>
        public decimal? ExitPrice { get; set; }

        /// <summary>
        /// ID of the symbol's currency.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// The type of advice for position.
        /// </summary>
        public PositionAdviceTypes AdviceType { get; set; }

        /// <summary>
        /// Current state of primary alert.
        /// </summary>
        public string AlertCurrentState { get; set; }

        /// <summary>
        /// Number of days when alert was triggered.
        /// </summary>
        public int? AlertDaysTriggered { get; set; }

        /// <summary>
        /// The date when primary alert was triggered for the first time.
        /// </summary>
        public DateTime? AlertFirstTimeTriggered { get; set; }

        /// <summary>
        /// The date when primary alert was triggered for the last time.
        /// </summary>
        public DateTime? AlertLastTriggered { get; set; }

        /// <summary>
        /// Stop price for Primary alert.
        /// </summary>
        public decimal? TriggerPrice { get; set; }

        /// <summary>
        /// Symbol's fundamental data
        /// </summary>
        public FundamentalDataContract FundamentalData { get; set; }

        /// <summary>
        /// Latest symbol trade volume.
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Dividend Yield.
        /// </summary>
        public decimal? TrailingDividendYield { get; set; }

        /// <summary>
        /// Average VQ value fot 30 years
        /// </summary>
        public decimal? Average30YearsVolatilityQuotient { get; set; }

        /// <summary>
        /// Ssi trend.
        /// </summary>
        public RocValueContract SsiTrend { get; set; }

        /// <summary>
        /// Current timing turn area.
        /// </summary>
        public TimingTurnAreaContract TurnArea { get; set; } //TODO: Waiting for Timing Turn Areas refactoring to be finished.

        /// <summary>
        /// Relative Strength Index (RSI) value with period = 14.
        /// </summary>
        public decimal? Rsi14 { get; set; }

        /// <summary>
        /// Amount of money to risk based on SSI Stop Price. 
        /// </summary>
        public decimal? SsiAtRisk { get; set; }

        /// <summary>
        /// Total position statistics calculated using corresponding (Open, Closed, All) trades. Not null for PT2 positions.
        /// </summary>
        public PortfolioTrackerPositionStatsContract PositionStats { get; set; }

        /// <summary>
        ///  First field for additional notes for position. Can be not null for PT2.
        /// </summary>
        public string Notes1 { get; set; }

        /// <summary>
        /// Global rank, calculated as an average rating across all criteria.
        /// </summary>
        public GlobalRankTypes? GlobalRank { get; set; }
    }
}
