using System.Collections.Generic;
using TradeStops.Common.Enums;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to create portfolio tracker subtrade
    /// </summary>
    public class CreatePtSubtradeContract
    {
        /// <summary>
        ///  Symbol Id.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Transaction trade type.
        /// </summary>
        public TradeTypes TradeType { get; set; }

        /// <summary>
        ///  List of transactions.
        /// </summary>
        public List<CreatePortfolioTrackerTransactionContract> Transactions { get; set; }
    }
}
