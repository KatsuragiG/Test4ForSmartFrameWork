using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Market allocation group for newsletter portfolios
    /// </summary>
    public class NewsletterMarketAllocationGroupContract
    {
        /// <summary>
        /// Symbol Group (market) type
        /// </summary>
        public SymbolGroupTypes SymbolGroupTypeId { get; set; }

        /// <summary>
        /// Number of positions in requested newsletter portfolios that match this symbol Group (market) type
        /// </summary>
        public int NumberOfPositions { get; set; }

        /// <summary>
        /// Percent value of positions in this group 
        /// </summary>
        public decimal PercentOfTotal { get; set; }
    }
}
