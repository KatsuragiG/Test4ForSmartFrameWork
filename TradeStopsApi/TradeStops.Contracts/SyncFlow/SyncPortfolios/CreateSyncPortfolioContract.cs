namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create synchronized portfolio
    /// </summary>
    public class CreateSyncPortfolioContract
    {
        /// <summary>
        ///  Portfolio name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  (optional) Portfolio notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        ///  Portfolio cash.
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        ///  Portfolio currency ID.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        ///  Financial institution ID.
        /// </summary>
        public int FinancialInstitutionId { get; set; }

        /// <summary>
        ///  Vendor account ID.
        /// </summary>
        public string VendorAccountId { get; set; }

        /// <summary>
        ///  Portfolio item ID.
        /// </summary>
        public string VendorPortfolioId { get; set; }

        /// <summary>
        ///  Portfolio total value.
        /// </summary>
        public decimal VendorPortfolioTotalValue { get; set; }

        /// <summary>
        ///  Account number.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Flag indicating whether to use the cross сourse for commissions
        /// </summary>
        public bool UseCrossCourseForCommission { get; set; }
    }
}
