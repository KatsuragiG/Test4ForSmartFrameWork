using System;
using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Option research statistic.
    /// </summary>
    public class OptionResearchStatisticContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Parent symbol ID.
        /// </summary>
        public int ParentSymbolId { get; set; }

        /// <summary>
        /// Option expiration date.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Option Type.
        /// </summary>
        public OptionTypes OptionType { get; set; }

        /// <summary>
        /// Maximum expected profit.
        /// </summary>
        public decimal? MaxProfit { get; set; }

        /// <summary>
        /// Maximum expected loss.
        /// </summary>
        public decimal? MaxLoss { get; set; }

        /// <summary>
        /// Position size value.
        /// </summary>
        public decimal? PositionSize { get; set; }

        /// <summary>
        /// Probability of profit value.
        /// </summary>
        public decimal? ProbabilityOfProfit { get; set; }

        /// <summary>
        /// Return on Investment (ROI) value.
        /// </summary>
        public decimal? Roi { get; set; }

        /// <summary>
        /// DecayType value.
        /// </summary>
        public DecayTypes DecayType { get; set; }

        /// <summary>
        /// Earnings date.
        /// </summary>
        public DateTime? EarningsDate { get; set; }

        /// <summary>
        /// Announced dividend dates.
        /// </summary>
        public List<DateTime> DividendDates { get; set; }

        /// <summary>
        /// Strike price value.
        /// </summary>
        public decimal StrikePrice { get; set; }
    }
}
