using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to calculate position size by risk for the list of symbols
    /// </summary>
    public class CalculateBulkPositionSizeContract
    {
        /// <summary>
        /// List of symbols with trade types for bulk position size
        /// </summary>
        public List<BulkPositionSizeSymbolContract> Symbols { get; set; }

        /// <summary>
        /// Position size type
        /// </summary>
        public BulkPositionSizeType PositionSizeType { get; set; }

        /// <summary>
        /// Investment amount value
        /// </summary>
        public decimal InvestmentAmount { get; set; }

        /// <summary>
        /// Investment currency ID
        /// </summary>
        public int InvestmentCurrencyId { get; set; }

        /// <summary>
        /// Determines if the fractional number of shares is allowed in the result.
        /// </summary>
        public bool AllowFractionalShares { get; set; }
    }
}
