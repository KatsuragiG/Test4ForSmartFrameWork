using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Symbol data.
    /// </summary>
    public class SymbolRangeContract
    {
        /// <summary>
        /// Symbol number on the page.
        /// </summary>
        public int SymbolNumber { get; set; }

        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Company name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Symbol type.
        /// </summary>
        [Obsolete("2020-10-26. Use SymbolType instead.")]
        public SymbolDataTypes DataType { get; set; }

        /// <summary>
        /// Symbol start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Symbol finish date.
        /// </summary>
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// Symbol was delisted.
        /// </summary>
        public bool Delisted { get; set; }

        /// <summary>
        /// Symbol exchange name.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Symbol exchange ID.
        /// </summary>
        public int ExchangeId { get; set; }

        /// <summary>
        /// SymbolType (stock, option, fund, etc.)
        /// </summary>
        public SymbolTypes SymbolType { get; set; }
    }
}
