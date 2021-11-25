using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Contract with the lists of corporate actions of different types
    /// </summary>
    public class CorporateActionsContract
    {
        /// <summary>
        /// Symbol splits.
        /// </summary>
        public IList<SplitContract> Splits { get; set; }

        /// <summary>
        /// Dividends.
        /// </summary>
        public IList<DividendContract> Dividends { get; set; }

        /// <summary>
        /// Spin offs.
        /// </summary>
        public IList<SpinOffContract> SpinOffs { get; set; }

        /// <summary>
        /// Stock distributions.
        /// </summary>
        public IList<StockDistributionContract> StockDistributions { get; set; }
    }
}
