namespace TradeStops.Contracts
{
    /// <summary>
    /// Detailed information about the company.
    /// </summary>
    public class CompanyDetailsContract
    {
        /// <summary>
        /// Company Id
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Company address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Company zip code
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// Company city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Company country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Company phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Company web site url
        /// </summary>
        public string WebSiteUrl { get; set; }

        /// <summary>
        /// Company net income
        /// </summary>
        public decimal? NetIncome { get; set; }

        /// <summary>
        /// Company market cap
        /// </summary>
        public decimal? MarketCap { get; set; }

        /// <summary>
        /// Company sector name
        /// </summary>
        public string SectorName { get; set; }

        /// <summary>
        /// Company industry name
        /// </summary>
        public string IndustryName { get; set; }

        /// <summary>
        /// Company business description
        /// </summary>
        public string BusinessDescription { get; set; }

        /// <summary>
        /// URL to get company logo.
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// Latitude of company location
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Longitude of company location
        /// </summary>
        public decimal? Longitude { get; set; }
    }
}
