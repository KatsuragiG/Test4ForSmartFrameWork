using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for system errors grid.
    /// </summary>
    public class SearchVendorSystemErrorsContract
    {
        /// <summary>
        ///  Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }

        /// <summary>
        ///  From date filter in UTC.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        ///  To date filter in UTC.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Financial institution name
        /// </summary>
        public string FinancialInstitutionName { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///  Search key word.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        ///  Search by field.
        /// </summary>
        public SearchVendorSystemErrorsSearchByFields? SearchByField { get; set; }

        /// <summary>
        ///  Order by field.
        /// </summary>
        public SearchVendorSystemErrorsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
