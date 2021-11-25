namespace TradeStops.Contracts
{
    /// <summary>
    /// Information regarding PortfolioLite Partner.
    /// </summary>
    public class PortfolioLitePartnerContract
    {
        /// <summary>
        /// PortfolioLite Partner identifier.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// The name of the PortfolioLite Partner.
        /// </summary>
        public string Name { get; set; }

        ////public virtual Guid LicenseKey { get; set; }

        ////public virtual DateTime ActiveFromUtc { get; set; }

        ////public virtual DateTime ActiveToUtc { get; set; }
    }
}
