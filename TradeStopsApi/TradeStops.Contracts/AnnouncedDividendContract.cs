using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Information about announced dividend.
    /// </summary>
    public class AnnouncedDividendContract
    {
        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Ex-dividend date on which all shares bought and sold no longer come attached with the right to receive the most recently declared dividend.
        /// </summary>
        public DateTime ExDate { get; set; }
    }
}
