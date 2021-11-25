namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about the company.
    /// </summary>
    public class CompanyProfileContract
    {
        /// <summary>
        /// Detailed information about the company.
        /// </summary>
        public CompanyDetailsContract CompanyDetails { get; set; }

        /// <summary>
        /// Company's industry information.
        /// </summary>
        public CompanyGicsContract CompanyGics { get; set; }
    }
}
