using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Grid parameters for financial institutions search.
    /// </summary>
    public class GridFinancialInstitutionsSearchContract : GridSearchContract
    {
        /// <summary>
        /// Vendor type.
        /// </summary>
        public VendorTypes VendorType { get; set; }
    }
}
