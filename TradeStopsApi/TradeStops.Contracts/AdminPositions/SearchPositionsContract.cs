using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search positions
    /// </summary>
    public class SearchPositionsContract
    {
        /// <summary>
        /// (optional) If true then return only delisted positions, if false - only not delisted
        /// </summary>
        public bool? IsFilteredByDelistedPositions { get; set; }

        /// <summary>
        /// (optional) Symbol type to filter positions
        /// </summary>
        public SymbolDataTypes? SymbolDataTypeFilterValue { get; set; }

        /// <summary>
        /// (optional) Trade type to filter positions
        /// </summary>
        public TradeTypes? TradeTypeFilterValue { get; set; }

        /// <summary>
        /// (optional) Position status type to filter positions
        /// </summary>
        public PositionStatusTypes? PositionStatusFilterValue { get; set; }

        /// <summary>
        /// (optional) Field to use for search value
        /// </summary>
        public SearchPositionsFields? SearchField { get; set; }

        /// <summary>
        /// (optional) Value to search for
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Order by field.
        /// </summary>
        public SearchPositionsOrderByFields OrderByField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }
    }
}
