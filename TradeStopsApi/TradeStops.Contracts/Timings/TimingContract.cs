using System;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The main information about timing
    /// </summary>
    public class TimingContract
    {
        /// <summary>
        /// Timing ID
        /// </summary>
        public int TimingId { get; set; }

        /// <summary>
        /// Symbol type
        /// </summary>
        public SymbolTypes SymbolType { get; set; }

        /// <summary>
        /// Symbol ID
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Publication date
        /// </summary>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// Indicates that the generation of automated comments for this timing is active or inactive.
        /// </summary>
        public bool IsActiveAutomatedDescription { get; set; }
    }
}
