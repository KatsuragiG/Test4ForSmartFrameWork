using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Dividend
    /// </summary>
    public class DividendContract
    {
        /// <summary>
        /// Dividend ID.
        /// </summary>
        public int DividendId { get; set; }

        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Dividend issue date.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// Dividend pay date.
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Dividend amount paid.
        /// </summary>
        public decimal DividendAmount { get; set; }

        /// <summary>
        /// Frequency of dividend payments.
        /// </summary>
        public decimal Frequency { get; set; }

        /// <summary>
        /// Dividend type.
        /// </summary>
        public DividendTypes Type { get; set; }
    }
}
