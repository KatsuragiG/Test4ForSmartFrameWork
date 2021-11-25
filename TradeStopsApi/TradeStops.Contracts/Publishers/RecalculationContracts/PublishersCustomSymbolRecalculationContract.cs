using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Custom symbol data for calculations.
    /// </summary>
    public class PublishersCustomSymbolRecalculationContract
    {
        /// <summary>
        /// Dividends.
        /// </summary>
        public List<PublishersCustomDividendRecalculationContract> CustomDividends { get; set; }
    }
}
