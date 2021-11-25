using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbol global rank.
    /// </summary>
    public class GlobalRankContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Global rank final value
        /// </summary>
        public double FinalRank { get; set; }

        /// <summary>
        /// Global rank
        /// </summary>
        public GlobalRankTypes GlobalRank { get; set; }

        /// <summary>
        /// Rank by billionaire
        /// </summary>
        public GlobalRankTypes BillionaireRank { get; set; }

        /// <summary>
        /// Number of billionaires holding symbol
        /// </summary>
        public int? NetBillionaireHolding { get; set; }

        /// <summary>
        /// Rank by newsletter
        /// </summary>
        public GlobalRankTypes NewsletterRank { get; set; }

        /// <summary>
        /// Number of newsletters holding symbol
        /// </summary>
        public int? NumberOfNewsletters { get; set; }

        /// <summary>
        /// Rank by strategies
        /// </summary>
        public GlobalRankTypes StrategyRank { get; set; }

        /// <summary>
        /// The number of strategies that include symbol
        /// </summary>
        public int? NumberOfStrategies { get; set; }

        /// <summary>
        /// Rank by health based on SSI status for Long adjusted position
        /// </summary>
        public GlobalRankTypes LongAdjHealthRank { get; set; }

        /// <summary>
        /// SSI status for Long position adjusted by dividends, splits, stock distributions, spin-offs
        /// </summary>
        public SsiStatuses? LongAdjSsiStatus { get; set; }

        /// <summary>
        /// Rank by eps
        /// </summary>
        public GlobalRankTypes EpsRank { get; set; }

        /// <summary>
        /// Earnings per share and basic and trailing twelve months
        /// </summary>
        public double? EpsAndBasicAndTTM { get; set; }

        /// <summary>
        /// Rank by timing
        /// </summary>
        public GlobalRankTypes TimingRank { get; set; }

        /// <summary>
        /// Type of timing turn area
        /// </summary>
        public TimingTurnAreaTypes? TurnAreaType { get; set; }

        /// <summary>
        /// Rank by health trend
        /// </summary>
        public GlobalRankTypes HealthTrendRank { get; set; }

        /// <summary>
        /// Trend type of symbol
        /// </summary>
        public TrendTypes? TrendType { get; set; }

        /// <summary>
        /// Rank by sector relationship based on SSI status for Long adjusted position
        /// </summary>
        public GlobalRankTypes LongAdjSectorRank { get; set; }

        /// <summary>
        /// Sector bull/bear indicator 
        /// </summary>
        public BullBearIndicatorStatuses? SectorIndicator { get; set; }

        /// <summary>
        /// Rank by likefolio
        /// </summary>
        public GlobalRankTypes LikeFolioRank { get; set; }

        /// <summary>
        /// Symbol's sentiment value provided by LikeFolio
        /// </summary>
        public LikeFolioSentimentTypes? LikeFolioStatus { get; set; }

        /// <summary>
        /// Rank by RSI
        /// </summary>
        public GlobalRankTypes RsiRank { get; set; }

        /// <summary>
        /// Symbol RSI value
        /// </summary>
        public double? RsiValue { get; set; }

        /// <summary>
        /// Rank by FScore
        /// </summary>
        public GlobalRankTypes FScoreRank { get; set; }

        /// <summary>
        /// Symbol FScore value
        /// </summary>
        public int? FScoreValue { get; set; }
    }
}
