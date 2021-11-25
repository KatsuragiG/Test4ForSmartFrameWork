using System;
using System.Collections.Generic;
using System.Text;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with Roc (Rate of Change) value for corresponding symbols
    /// </summary>
    public class GetRocValuesContract
    {
        /// <summary>
        /// Symbol Ids
        /// </summary>
        public List<int> SymbolIds { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Symbols adjusted or not
        /// </summary>
        public bool IsAdjusted { get; set; }
    }
}
