using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with Roc (Rate of Change) value for corresponding symbol
    /// </summary>
    public class RocValueContract
    {
        /// <summary>
        /// Symbol Id
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Symbol adjusted or not
        /// </summary>
        public bool IsAdjusted { get; set; }

        /// <summary>
        /// Trend type of symbol
        /// </summary>
        public TrendTypes? TrendType { get; set; }
    }
}
