using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Option-specific data
    /// </summary>
    public class OptionContract
    {
        /// <summary>
        /// Option symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Option contract size.
        /// </summary>
        public decimal ContractSize { get; set; }

        /// <summary>
        /// Option expiration date.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Underline stock ID.
        /// </summary>
        public int ParentSymbolId { get; set; }

        /// <summary>
        /// Option strike price.
        /// </summary>
        public decimal StrikePrice { get; set; }

        /// <summary>
        /// Option type (P or C).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Information about parent stock symbol
        /// </summary>
        public StockContract ParentStock { get; set; }
    }
}
