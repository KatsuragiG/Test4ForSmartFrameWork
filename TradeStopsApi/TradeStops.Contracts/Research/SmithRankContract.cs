namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbol Smith Rank.
    /// </summary>
    public class SmithRankContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol Smith Rank
        /// </summary>
        public decimal? Rank { get; set; }
    }
}
