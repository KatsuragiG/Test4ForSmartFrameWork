namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to find company profile reports
    /// </summary>
    public class SearchCompanyProfileReportsContract
    {
        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Is annual report
        /// </summary>
        public bool IsAnnual { get; set; }

        /// <summary>
        /// Is quarterly report
        /// </summary>
        public bool IsQuarterly { get; set; }

        /// <summary>
        /// Maximum number of reports to return
        /// </summary>
        public int MaxResults { get; set; }
    }
}
