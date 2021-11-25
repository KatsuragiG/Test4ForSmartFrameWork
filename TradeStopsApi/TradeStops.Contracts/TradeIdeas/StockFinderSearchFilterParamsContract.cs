using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters for each filter
    /// </summary>
    public class StockFinderSearchFilterParamsContract
    {
        /// <summary>
        /// Lower filter threshold
        /// </summary>
        public decimal? Decimal1 { get; set; }

        /// <summary>
        /// Higher filter threshold
        /// </summary>
        public decimal? Decimal2 { get; set; }

        /// <summary>
        /// Item list
        /// </summary>
        public List<int> List1 { get; set; }

        /// <summary>
        /// Filter flag
        /// </summary>
        public bool? Bool1 { get; set; }

        /// <summary>
        /// Filter value
        /// </summary>
        public int? Int1 { get; set; }

        /// <summary>
        /// List of nested items
        /// </summary>
        public List<MultipleFilterItemContract> TupleList1 { get; set; }

        /// <summary>
        /// Item 2 list
        /// </summary>
        public List<int> List2 { get; set; }
    }
}
