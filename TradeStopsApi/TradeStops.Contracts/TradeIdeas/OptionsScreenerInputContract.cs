using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to find options.
    /// </summary>
    public class OptionsScreenerInputContract
    {
        /// <summary>
        /// Parameters to specify sources that will be used to search for options.
        /// </summary>
        public OptionsScreenerSourcesContract Sources { get; set; }

        /// <summary>
        /// Parameters to specify filters that will be applied to selected sources.
        /// </summary>
        public OptionsScreenerFiltersContract Filters { get; set; }

        /// <summary>
        /// Number of options to return.
        /// </summary>
        public int NumberOfOptionsToReturn { get; set; }

        /// <summary>
        /// Field to perform sort operation.
        /// </summary>
        public string OrderByField { get; set; }

        /// <summary>
        /// Sort type, ascending or descending.
        /// </summary>
        public OrderTypes OrderType { get; set; }

        /// <summary>
        /// Indicates if multiple options of the same parent can be returned in results.
        /// </summary>
        public bool AllowMultipleOptions { get; set; }
    }
}
