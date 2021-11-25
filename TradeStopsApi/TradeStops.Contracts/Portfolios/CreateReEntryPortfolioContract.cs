namespace TradeStops.Contracts
{
    /// <summary>
    /// Options to create Re-entry portfolio
    /// </summary>
    public class CreateReEntryPortfolioContract
    {
        /// <summary>
        /// Re-entry portfolio name that will be used by default.
        /// If portfolio with the same name exists then it will be created with the number in the end, like 'Same Name (2)'
        /// </summary>
        public string DefaultPortfolioName { get; set; }
    }
}
