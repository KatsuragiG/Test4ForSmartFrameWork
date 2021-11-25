namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get Benzinga Earnings
    /// </summary>
    public class GetBenzingaEarningsContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// The maximum number of previous earnings per share to return.
        /// </summary>
        public int MaxUpcomingResults { get; set; }

        /// <summary>
        /// The maximum number of upcoming earnings per share to return.
        /// </summary>
        public int MaxPreviousResults { get; set; }
    }
}
