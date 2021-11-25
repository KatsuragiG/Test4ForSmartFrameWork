namespace TradeStops.Contracts
{
    /// <summary>
    /// Country
    /// </summary>
    public class CountryContract
    {
        /// <summary>
        /// Country ID
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 2-letters ISO country code
        /// </summary>
        public string IsoCode { get; set; }

        /// <summary>
        /// 3-letters ISO country code
        /// </summary>
        public string Iso3Code { get; set; }
    }
}
