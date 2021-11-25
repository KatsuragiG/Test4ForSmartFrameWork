using System;

namespace TradeStops.Contracts
{
    /// <summary>
    /// The contract returns symbol configuration.
    /// </summary>
    public class SymbolConfigurationContract
    {
        /// <summary>
        /// Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Configuration type.
        /// </summary>
        public virtual int DataType { get; set; }

        /// <summary>
        /// Last price update.
        /// </summary>
        public virtual DateTime? LastPriceUpdate { get; set; }

        /// <summary>
        /// Symbol configuration updatable or not.
        /// </summary>
        public virtual bool Updatable { get; set; }

        /// <summary>
        /// Intraday prices is available or not for symbol.
        /// </summary>
        public bool HasPrices { get; set; }
    }
}
