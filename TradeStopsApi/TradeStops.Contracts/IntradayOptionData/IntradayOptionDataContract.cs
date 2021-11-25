using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract contains intraday option data.
    /// </summary>
    public class IntradayOptionDataContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Trade date and time in UTC format.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Close price.
        /// </summary>
        public decimal? ClosePrice { get; set; }

        /// <summary>
        /// Bid value.
        /// </summary>
        public decimal? Bid { get; set; }

        /// <summary>
        /// Ask value.
        /// </summary>
        public decimal? Ask { get; set; }

        /// <summary>
        /// Implied volatility absolute value.
        /// </summary>
        public decimal? ImpliedVolatility { get; set; }

        /// <summary>
        /// Delta value.
        /// </summary>
        public decimal? Delta { get; set; }

        /// <summary>
        /// Gamma value.
        /// </summary>
        public decimal? Gamma { get; set; }

        /// <summary>
        /// Theta value.
        /// </summary>
        public decimal? Theta { get; set; }

        /// <summary>
        /// Vega value.
        /// </summary>
        public decimal? Vega { get; set; }

        /// <summary>
        /// Rho value.
        /// </summary>
        public decimal? Rho { get; set; }

        /// <summary>
        /// Implied volatility percentile absolute value.
        /// </summary>
        public decimal? ImpliedVolatilityPercentile { get; set; }

        /// <summary>
        /// Implied volatility rank absolute value.
        /// </summary>
        public decimal? ImpliedVolatilityRank { get; set; }

        /// <summary>
        /// Number of transaction elements.
        /// </summary>
        public decimal Volume { get; set; }
    }
}
