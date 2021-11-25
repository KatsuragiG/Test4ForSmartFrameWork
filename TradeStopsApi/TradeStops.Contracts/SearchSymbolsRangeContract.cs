using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to search symbols range.
    /// </summary>
    public class SearchSymbolsRangeContract
    {
        /// <summary>
        /// Query string to search for.
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Field to search for.
        /// </summary>
        public SearchSymbolsRangeQueryFields? SearchField { get; set; }

        /// <summary>
        /// Value defines whether the search will be performed among delisted symbols.
        /// </summary>
        public bool IsDelisted { get; set; }

        /// <summary>
        /// Symbol data type.
        /// </summary>
        public SymbolDataTypes? DataType { get; set; }

        /// <summary>
        /// Contract to search options range.
        /// </summary>
        public SearchOptionsRangeContract OptionsContract { get; set; }

        /// <summary>
        /// Field to order by.
        /// </summary>
        public SearchSymbolsRangeFields OrderField { get; set; }

        /// <summary>
        /// Order direction.
        /// </summary>
        public OrderTypes OrderType { get; set; }

        /// <summary>
        /// Items to skip count ((page number - 1) * page size).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Items to take count (page size).
        /// </summary>
        public int Limit { get; set; }
    }
}
