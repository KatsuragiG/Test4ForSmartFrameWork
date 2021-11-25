using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search financial institutions
    /// </summary>
    public class SearchFinancialInstitutionsContract
    {
        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        /// Value to search for
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// (optional) Field to use for search value
        /// </summary>
        public SearchFinancialInstitutionsFields? SearchField { get; set; }

        /// <summary>
        /// Order by field.
        /// </summary>
        public SearchFinancialInstitutionsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
