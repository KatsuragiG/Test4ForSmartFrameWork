using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Result of bulk position size
    /// </summary>
    public class BulkPositionSizeResultContract
    {
        /// <summary>
        /// Symbol contract
        /// </summary>
        public SymbolContract Symbol { get; set; }

        /// <summary>
        /// SSI values
        /// </summary>
        public SsiValueContract Ssi { get; set; }

        /// <summary>
        /// Position trade type
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        /// Volatility Quotient (VQ) for the symbol
        /// </summary>
        public decimal Vq { get; set; }

        /// <summary>
        /// Average VQ value for 30 years
        /// </summary>
        public decimal? Average30YearsVq { get; set; }

        /// <summary>
        /// Latest close
        /// </summary>
        public decimal LatestClose { get; set; }

        /// <summary>
        /// Value of the position
        /// </summary>
        public decimal PositionSize { get; set; }

        /// <summary>
        /// Percent value of the position
        /// </summary>
        public decimal PositionSizePercent { get; set; }

        /// <summary>
        /// Percent value of the risk per position
        /// </summary>
        public decimal RiskPerPosition { get; set; }

        /// <summary>
        /// Share number of the position
        /// </summary>
        public decimal Shares { get; set; }
    }
}
