using System.Collections.Generic;

namespace TradeStops.Contracts
{
    /// <summary>
    /// Parameters to get portfolio tracker transactions.
    /// </summary>
    public class GetPortfolioTrackerTransactionsContract
    {
        /// <summary>
        /// (Optional) Find transactions matching provided transactionIds.
        /// </summary>
        public List<int> TransactionIds { get; set; }

        /// <summary>
        /// (Optional) Find transactions matching provided subtradeIds. If no ids were provided, than transactions from all user portfolios will be returned.
        /// </summary>
        public List<int> SubtradeIds { get; set; }        
    }
}
