using System;

using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Undocumented contract created before applying SA1600")]
    public class SearchOptionsContract
    {
        /// <summary>
        /// (Optional) Underline stock ID. Leave blank to search by Option Ticker.
        /// </summary>
        public int? ParentSymbolId { get; set; }

        /// <summary>
        /// (Optional) Option expiration date.
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// (Optional) Option strike price.
        /// </summary>
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// (Optional) Option type.
        /// </summary>
        public OptionTypes? Type { get; set; }

        /// <summary>
        /// Symbol was delisted.
        /// </summary>
        public bool Delisted { get; set; }  // we have Delisted = 1 in the database both for expired and delisted options

        /// <summary>
        /// (Optional) Search for all active Options with Ticker starting with this value.
        /// </summary>
        public string OptionTicker { get; set; }

        /// <summary>
        /// (Optional) Maximum number of Options to return. If not specified the default value = 9999 will be used.
        /// </summary>
        public int MaxResults { get; set; }
    }
}
