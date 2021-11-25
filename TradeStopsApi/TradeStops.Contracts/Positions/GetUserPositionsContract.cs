using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to load user's positions
    /// </summary>
    public class GetUserPositionsContract
    {
        /// <summary>
        /// (optional) List of portfolio IDs to load positions.
        /// User's positions from open portfolios will be returned by default
        /// </summary>
        public List<int> PortfolioIds { get; set; }

        /// <summary>
        /// (optional) List of Position Status Types enumerations to load positions only for specified StatusTypes.
        /// Positions with Status Type 'Open' are returned by default.
        /// </summary>
        public List<PositionStatusTypes> StatusTypes { get; set; }

        /// <summary>
        /// (optional) Determines whether the soft-deleted positions should be also included to the list.
        /// Equals to 'false' by default
        /// </summary>
        public bool IncludeDelisted { get; set; }

        /// <summary>
        /// (optional) List of Symbols to load positions only on specified Symbols.
        /// If at least on Symbol was specifed than Pair Trade positions will be excluded from the results.
        /// User's positions on all Symbols will be returned by default.
        /// </summary>
        public List<int> SymbolIds { get; set; }
    }
}
