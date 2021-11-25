using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to delete portfolio tracker transactions.
    /// </summary>
    public class DeletePortfolioTrackerTransactionsContract
    {
        /// <summary>
        /// List of transaction Ids to delete.
        /// </summary>
        public List<int> TransactionIds { get; set; }        
    }
}
