using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// LikeFolioValueContract class.
    /// </summary>
    public class LikeFolioValueContract
    {
        /// <summary>
        /// The unique Id of the symbol
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// The trade date to which LikeFolio value corresponds to.
        /// </summary>
        public DateTime TradeDate { get; set; }

        /// <summary>
        /// LikeFolio numeric value
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Symbol's sentiment value provided by LikeFolio.
        /// </summary>
        public LikeFolioSentimentTypes SentimentType { get; set; }
    }
}
