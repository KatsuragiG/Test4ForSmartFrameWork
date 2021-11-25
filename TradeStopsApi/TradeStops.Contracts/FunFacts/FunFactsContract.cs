namespace TradeStops.Contracts
{
    /// <summary>
    /// Set of fun facts by symbol.
    /// </summary>
    public class FunFactsContract
    {
        /// <summary>
        /// Symbol ID.
        /// </summary>
        public int SymbolId { get; set; }

        /// <summary>
        /// Drawdown fun fact.
        /// </summary>
        public DrawdownFactContract DrawdownFact { get; set; }

        /// <summary>
        /// Likelihood of stock dropping fun fact.
        /// </summary>
        public LikelihoodDropFactContract LikelihoodDropFact { get; set; }

        /// <summary>
        /// Biggest drop fun fact.
        /// </summary>
        public BiggestDropFactContract BiggestDropFact { get; set; }
    }
}
