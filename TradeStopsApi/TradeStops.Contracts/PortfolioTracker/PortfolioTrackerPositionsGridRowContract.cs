using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Portfolio Tracker positions grid row data
    /// </summary>
    public class PortfolioTrackerPositionsGridRowContract
    {
        /// <summary>
        /// Position ID.
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// Position portfolio ID
        /// </summary>
        public int PortfolioId { get; set; }

        /// <summary>
        /// Position portfolio name.
        /// </summary>
        public string PortfolioName { get; set; }

        /// <summary>
        /// Total position statistics calculated using corresponding (Open, Closed, All) trades.
        /// </summary>
        public PortfolioTrackerPositionStatsContract PositionStats { get; set; }

        /// <summary>
        /// Information about symbol
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// Symbol's fundamental data
        /// </summary>
        public FundamentalDataContract FundamentalData { get; set; }

        /// <summary>
        /// Currency ID of the position.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Date and time when the position has been created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Position type in the Portfolio Tracker
        /// </summary>
        public PositionTypes PositionType { get; set; }

        /// <summary>
        /// Overal Position status. Open - has opened subtrades, Closed - all subtrades are closed.
        /// </summary>
        public PositionStatusTypes StatusType { get; set; }

        /// <summary>
        /// Position trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        ///  Recommended advice type for position.
        /// </summary>
        public PositionAdviceTypes AdviceType { get; set; }

        /// <summary>
        /// Indicates whether this position is published in TradeSmith Gurus center or not.
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        ///  Additional notes for recommender advice.
        /// </summary>
        public string AdviceNotes { get; set; }

        /// <summary>
        ///  First field for additional notes for position.
        /// </summary>
        public string Notes1 { get; set; }

        /// <summary>
        /// Position close price.
        /// </summary>
        public decimal LatestClose { get; set; }

        /// <summary>
        /// Latest Intraday price.
        /// </summary>
        public decimal? LatestIntradayPrice { get; set; }

        /// <summary>
        /// Latest symbol trade volume.
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Percent of total portfolio value.
        /// </summary>
        public decimal? PercentOffPortfolio { get; set; }

        /// <summary>
        /// Dividend Yield.
        /// </summary>
        public decimal? TrailingDividendYield { get; set; }

        /// <summary>
        /// Position VQ value.
        /// </summary>
        public decimal? VolatilityQuotient { get; set; }

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
        public TimingTurnAreaContract TurnArea { get; set; }

        /// <summary>
        /// Relative Strength Index (RSI) value with period = 14.
        /// </summary>
        public decimal? Rsi14 { get; set; }

        /// <summary>
        /// Contact with SSI values.
        /// </summary>
        public SsiValueContract SsiValue { get; set; }

        /// <summary>
        /// Amount of money to risk based on SSI Stop Price. 
        /// </summary>
        public decimal? SsiAtRisk { get; set; }

        /// <summary>
        /// Alert that was chosen as primary for position. Can be null.
        /// </summary>
        public PtPositionTriggerContract PrimaryAlert { get; set; }

        /// <summary>
        /// Global rank, calculated as an average rating across all criteria.
        /// </summary>
        public GlobalRankTypes? GlobalRank { get; set; }
    }
}
